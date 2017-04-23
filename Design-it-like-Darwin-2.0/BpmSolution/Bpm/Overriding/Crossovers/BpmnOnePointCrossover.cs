using System;
using System.Collections.Generic;
using System.Diagnostics;
using Bpm.Helpers;
using PortableGeneticAlgorithm.Interfaces;

namespace Bpm.Crossovers
{
    public class BpmnOnePointCrossover : ICrossover
    {
        public IList<IGenome> PerformCrossover(IGenome parent1, IGenome parent2)
        {
            BpmGenome parentOne;
            BpmGenome parentTwo;
            try
            {
                parentOne = parent1 as BpmGenome;
                parentTwo = parent2 as BpmGenome;
            }
            catch (FormatException)
            {
                throw new FormatException($"Couldn't parse either {parent1} or {parent2} to BpmGenome!");
            }
            catch (NullReferenceException)
            {
                throw new Exception($"Either parameter {parent1} or {parent2} were null!");
            }


            BpmGene crossoverPointOne = null;
            BpmGene crossoverPointTwo = null;
            var max = 0;
            do
            {
                // Get crossover indices
                var randomOne = TreeHelper.RandomGenerator.Next(0, parentOne.NumberOfGenes);
                var randomTwo = TreeHelper.RandomGenerator.Next(0, parentTwo.NumberOfGenes);

                // Renumber indices accoding to Depth-First Search, because they are broken after crossing
                parentOne.RootGene.RenumberIndicesOfBpmTree(gen => gen.Children);
                parentTwo.RootGene.RenumberIndicesOfBpmTree(gen => gen.Children);
                // Crossover parents
                // Depth-first search for indices
                // Select subtree one
                crossoverPointOne = parentOne.RootGene.DepthFirstSearch(n => n.Children,
                    element => element.Index == randomOne);
                // Save nodes in tmp variable
                // select subtre two
                crossoverPointTwo = parentTwo.RootGene.DepthFirstSearch(n => n.Children,
                    element => element.Index == randomTwo);

                var depth1 = crossoverPointOne.CalculateNodeDepth() +
                             crossoverPointTwo.DepthOfDeepestLeaf.Count;
                var depth2 = crossoverPointTwo.CalculateNodeDepth() +
                             crossoverPointOne.DepthOfDeepestLeaf.Count;
                max = depth1;
                if (depth2 > depth1)
                    max = depth2;
            } while (max > ModelHelper.GetBpmModel().GetMaxDepthRandomGenome());

            // Set new parent node for tmp varianbles
            try
            {
                // remember/protect original parents 
                var tempCrossoverPointParentOne = crossoverPointOne.Parent;
                var tempCrossoverPointParentTwo = crossoverPointTwo.Parent;

                var childIndex = TreeHelper.GetChildIndex(crossoverPointTwo.Parent, crossoverPointTwo);

                if (childIndex == -1)
                    parentTwo.RootGene = crossoverPointOne;
                else
                    crossoverPointTwo.Parent.Children[childIndex] = crossoverPointOne;

                childIndex = TreeHelper.GetChildIndex(crossoverPointOne.Parent, crossoverPointOne);

                if (childIndex == -1)
                    parentOne.RootGene = crossoverPointTwo;
                else
                    crossoverPointOne.Parent.Children[childIndex] = crossoverPointTwo;

                // top down relashions done
                // now handle down top relashions

                crossoverPointOne.Parent = tempCrossoverPointParentTwo;
                crossoverPointTwo.Parent = tempCrossoverPointParentOne;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw new Exception($"{ex.Message}");
            }

            // Renumber indices accoding to Depth-First Search, because they are broken after crossing
            parentOne.RootGene.RenumberIndicesOfBpmTree(gen => gen.Children);
            parentTwo.RootGene.RenumberIndicesOfBpmTree(gen => gen.Children);

            ProcessHelper.Validator.CheckForBpmnXorFailture(parentOne.RootGene);
            ProcessHelper.Validator.CheckForBpmnXorFailture(parentTwo.RootGene);

            // Return parents as children after exchanging subtrees at random crossover point.
            return new List<IGenome> {parentOne, parentTwo};
        }
    }
}