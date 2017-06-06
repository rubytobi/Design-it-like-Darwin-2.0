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
using Bpm.NotationElements;

namespace Bpm
{
    /// <summary>
    ///     Represents an bpm gene
    /// </summary>
    public class BpmGene
    {
        #region Constructor

        /// <summary>
        ///     Creates an instance of the
        /// </summary>
        /// <param name="index"></param>
        /// <param name="parent"></param>
        public BpmGene(int index, BpmGene parent)
        {
            Index = index;
            Parent = parent;
        }

        #endregion //Constructor

        #region Properties

        /// <summary>
        ///     Index is been set by the system according to depth first search.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        ///     Parent of the gene in the BPMN-Prozess-Tree
        /// </summary>
        public BpmGene Parent { get; set; }

        /// <summary>
        ///     Children of the gene in the BPMN-Process-Tree
        /// </summary>
        public virtual List<BpmGene> Children { get; internal set; } = new List<BpmGene>();

        /// <summary>
        ///     BPMN-Activities that produce input documents needed for this.
        /// </summary>
        public virtual List<BpmnActivity> RequiredInputBpmActivities { get; internal set; }

        public string Id { get; } = Guid.NewGuid().ToString();

        public List<BpmGene> DepthOfDeepestLeaf
        {
            get
            {
                var path = new List<BpmGene>();
                if (Children != null)
                    foreach (var node in Children)
                    {
                        var tmp = node.DepthOfDeepestLeaf;
                        if (tmp.Count > path.Count)
                            path = tmp;
                    }

                path.Insert(0, this);
                return path;
            }
        }

        /// <summary>
        ///     Execution probability is needed to calculate the FitnessValue according
        ///     to μ-σ-Principle.
        /// </summary>
        public double ExecutionProbability { get; set; } = 1;

        #endregion //Properties
    }
}