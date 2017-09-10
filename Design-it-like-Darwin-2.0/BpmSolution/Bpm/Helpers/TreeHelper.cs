#region license
// MIT License
// 
// Copyright (c) [2017] [Tobias Ruby]
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
#endregion
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Bpm.NotationElements;
using Bpm.NotationElements.Gateways;
using PortableGeneticAlgorithm.Interfaces;

namespace Bpm.Helpers
{
    /// <summary>
    ///     Helper class for tree operations
    /// </summary>
    public static class TreeHelper
    {
        /// <summary>
        ///     Gets a new instance of Random()
        /// </summary>
        //public static Random RandomGenerator { get; } = new Random(42);
        public static Random RandomGenerator { get; } = new Random();

        /// <summary>
        ///     Depth first search implementation.
        /// </summary>
        /// <typeparam name="T">Type of tree structure item.</typeparam>
        /// <typeparam name="TChilds">Type of childs collection.</typeparam>
        /// <param name="node">Starting node to search.</param>
        /// <param name="childsProperty">Property to return child node.</param>
        /// <param name="match">Predicate for matching.</param>
        /// <returns>The instance of matched result, null if not found.</returns>
        public static T DepthFirstSearch<T, TChilds>(this T node, Func<T, TChilds> childsProperty, Predicate<T> match)
            where T : class
        {
            if (childsProperty(node) == null)
                return node;

            var stack = new Stack<T>();
            stack.Push(node);
            while (stack.Count > 0)
            {
                var thisNode = stack.Pop();
                if (match(thisNode))
                    return thisNode;

                if (childsProperty(thisNode) != null)
                    foreach (var child in ((IEnumerable<T>)childsProperty(thisNode)).Reverse())
                        stack.Push(child);
            }
            return null;
        }

        /// <summary>
        ///     Counts the number of nodes in a tree.
        /// </summary>
        /// <typeparam name="T">Type of tree structure item.</typeparam>
        /// <typeparam name="TChilds">Type of childs collection.</typeparam>
        /// <param name="node">Starting node to search.</param>
        /// <param name="childsProperty">Property to return child node.</param>
        /// <returns></returns>
        public static int GetNumberOfNodes<T, TChilds>(this T node, Func<T, TChilds> childsProperty)
            where T : class
        {
            Debug.WriteLine("calculate the function GetNumberOfNodes: " + node);
            //if (!(childsProperty(node) is IEnumerable<T>))
            //{
            //    throw new ArgumentException("ChildsProperty must be IEnumerable<T>");
            //}

            var counter = 0;

            var stack = new Stack<T>();
            stack.Push(node);
            counter++;
            while (stack.Count > 0)
            {
                var thisNode = stack.Pop();

                if (childsProperty(thisNode) != null)
                    foreach (var child in ((IEnumerable<T>)childsProperty(thisNode)).Reverse())
                    {
                        stack.Push(child);
                        counter++;
                    }
            }
            return counter;
        }

        /// <summary>
        ///     Resets the index of each node according to Deepth-First Search.
        /// </summary>
        /// <param name="node">Starting node to search.</param>
        /// <param name="childsProperty">Property to return child node.</param>
        /// <returns></returns>
        public static void RenumberIndicesOfBpmTree(this BpmGene node,
            Func<BpmGene, IEnumerable<BpmGene>> childsProperty)
        {
            Debug.WriteLine("Renumbering the following gene: " + node);

            var counter = 0;

            var stack = new Stack<BpmGene>();
            node.Index = counter;
            stack.Push(node);
            while (stack.Count > 0)
            {
                var thisNode = stack.Pop();

                if (childsProperty(thisNode) != null)
                    foreach (var child in childsProperty(thisNode))
                    {
                        child.Index = ++counter;
                        stack.Push(child);
                    }
            }
        }

        public static double CalculateLongestTime(BpmGene rootGene)
        {
            if (rootGene is BpmnSeq)
            {
                var time = 0.0;

                rootGene.Children.ForEach(x => time += CalculateLongestTime(x));

                return time;
            }

            if (rootGene is BpmnAnd)
            {
                if (rootGene.Children.Count <= 0)
                {
                    return 0;
                }

                return rootGene.Children.Max(x => CalculateLongestTime(x));
            }

            return DataHelper.ActivityHelper.Instance().GetTime((rootGene as BpmnActivity).Name);
        }

        /// <summary>
        ///     sucht und findet Blätter mit dem speziellen Index, ansonsten return null
        /// </summary>
        /// <param name="index">Gene index</param>
        /// <param name="gene">starting gene</param>
        /// <returns></returns>
        public static BpmGene FindLeaf(this BpmGene gene, int index)
        {
            if (gene.Index == index)
                if (gene.GetType() == typeof(BpmnActivity))
                    return gene;
                else
                    return null;

            // falsches blatt gefunden
            if (gene.Children == null || gene.Children.Count == 0)
                return null;

            for (var i = gene.Children.Count - 1; i >= 0; i--)
                if (gene.Children[i].Index <= index)
                    return gene.Children[i].FindLeaf(index);

            return null;
        }

        /// <summary>
        ///     calculates, depending on the type of the random gene, the number of children
        /// </summary>
        /// <param name="randomGene"></param>
        /// <returns>minimum 1, for xor maximum 2</returns>
        public static int CalculateRandomNumberOfChildren(this Type randomGene)
        {
            if (randomGene == typeof(BpmnXor))
                return RandomGenerator.Next(2) + 1;

            // TODO aus Excel als Parameter einlesen?
            return RandomGenerator.Next((int)(DataHelper.ActivityHelper.Instance().GetAll().Count / 1.5)) + 1;
        }

        /// <summary>
        /// </summary>
        /// <param name="initialGenome"></param>
        /// <returns></returns>
        public static List<BpmnActivity> ListActivities(this IGenome initialGenome)
        {
            var list = new List<BpmnActivity>();

            if (initialGenome is BpmGenome)
                ListActivities(((BpmGenome)initialGenome).RootGene, list);

            return list;
        }

        private static void ListActivities(BpmGene gene, List<BpmnActivity> list)
        {
            if (gene is BpmnActivity)
                list.Add((BpmnActivity)gene);
            else
                foreach (var g in gene.Children)
                    ListActivities(g, list);
        }

        /// <summary>
        ///     Parses a *valid* process into a tree representation
        /// </summary>
        /// <param name="s">process string</param>
        /// <returns>genome</returns>
        public static BpmGenome ParseBpmGenome(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return null;

            Debug.WriteLine("Parsing now the following string: " + s);

            var genome = new BpmGenome();
            BpmGene parent = null;

            s = s.Replace(" ", "");

            var index = 0;

            while (s.Length > 0)
                if (s.StartsWith("<PI;"))
                {
                    s = s.Substring(4);
                }
                else if (s.StartsWith(";PO>"))
                {
                    s = s.Substring(4);
                }
                else if (s.StartsWith("AND("))
                {
                    s = s.Substring(4);

                    var and = new BpmnAnd(index, parent);
                    parent?.Children.Add(and);

                    if (genome.RootGene == null)
                        genome.RootGene = and;

                    parent = and;
                    index++;
                }
                else if (s.StartsWith("SEQ("))
                {
                    s = s.Substring(4);
                    var seq = new BpmnSeq(index, parent);

                    // Only necessary if genome is one seq
                    if (parent == null)
                        genome.RootGene = seq;
                    else
                        parent.Children.Add(seq);

                    parent = seq;
                    index++;
                }
                else if (s.StartsWith("XOR("))
                {
                    s = s.Substring(4);
                    var decision = s.Substring(0, s.IndexOf(";"));
                    s = s.Substring(decision.Length);

                    decision = decision.Substring(2);
                    decision = decision.Substring(0, decision.Length - 1);

                    var decisionId = decision.Split(',')[0];
                    var decisionValue = decision.Split(',')[1];

                    var probability = DataHelper.ActivityAttributeHelper.Instance()
                        .GetDecisionProbability(decisionId,
                            decisionValue);

                    var xor = new BpmnXor(index, parent, decisionId, decisionValue, probability);

                    // Only necessary if genome is one xor
                    if (parent == null)
                        genome.RootGene = xor;
                    else
                        parent.Children.Add(xor);

                    parent = xor;
                    index++;
                }
                else if (s.StartsWith(";"))
                {
                    s = s.Substring(1);
                }
                else if (s.StartsWith(")"))
                {
                    parent = parent.Parent;
                    s = s.Substring(1);
                }
                else
                {
                    char[] seperators = { ';', ')' };
                    var name = s.Substring(0, s.IndexOfAny(seperators));
                    s = s.Substring(s.IndexOfAny(seperators));

                    var activity = new BpmnActivity(index, parent, name);

                    // Only necessary if genome is one activity
                    if (parent == null)
                        genome.RootGene = activity;
                    else
                        parent.Children.Add(activity);

                    index++;
                }

            if (parent != null)
                throw new Exception("Illegal input string");

            return genome;
        }

        /// <summary>
        ///     Counts number of hops to root node, alias depth
        /// </summary>
        /// <param name="gene">the gene to calculate for</param>
        /// <returns></returns>
        public static int CalculateNodeDepth(this BpmGene gene)
        {
            var parent = gene;

            var count = 0;
            while (parent != null)
            {
                parent = parent.Parent;
                count++;
            }

            return count;
        }

        /// <summary>
        ///     Calculates the index of the specified child in parents children
        /// </summary>
        /// <param name="parent">the parent where to look</param>
        /// <param name="child">the child which to look for</param>
        /// <returns></returns>
        public static int GetChildIndex(BpmGene parent, BpmGene child)
        {
            if (parent == null)
                return -1;

            for (var i = 0; i < parent.Children.Count; i++)
                if (parent.Children[i].Index == child.Index)
                    return i;

            return -1;
        }

        /// <summary>
        ///     Count number of nodes in tree
        /// </summary>
        /// <param name="root">root node from tree</param>
        /// <param name="nodeType">node type looking for</param>
        /// <returns></returns>
        public static int CountSpecificNodes(this BpmGene root, Type nodeType)
        {
            if (root == null)
                return 0;

            var sum = 0;

            if (root.GetType() == nodeType)
                sum++;

            if (root is BpmnActivity)
                return sum;

            if (root is BpmnAnd || root is BpmnSeq)
                return sum + root.Children.Sum(x => CountSpecificNodes(x, nodeType));

            // XOR
            return sum + root.Children.Sum(x => CountSpecificNodes(x, nodeType));
        }
    }
}