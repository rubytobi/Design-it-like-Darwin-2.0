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
using System.IO;
using Bpm.Helpers;
using PortableGeneticAlgorithm.Interfaces;

namespace Bpm.Mutations
{
    /// <summary>
    ///     Mutates a handovered genome, by:
    ///     1. finding a random mutation point in the tree
    ///     2. removing the subtree beneath the mutation point (inclusive the mutationpoint node.)
    ///     3. adding a randomly generated subtree beneath the parent of the chosen mutationpoint.
    /// </summary>
    public class BpmnMutation : IMutation
    {
        /// <summary>
        ///     Mutates a handovered genome, by:
        ///     1. finding a random mutation point in the tree
        ///     2. removing the subtree beneath the mutation point (inclusive the mutationpoint node.)
        ///     3. adding a randomly generated subtree beneath the parent of the chosen mutationpoint.
        /// </summary>
        /// <param name="genome">Genome to mutate.</param>
        /// <param name="mutationProbability">Probability if handovered genome is mutated or not.</param>
        public IGenome Mutate(IGenome genome, double mutationProbability)
        {
            var mutate = genome as BpmGenome;
            if (mutate != null)
            {
                var genomeToMutate = (BpmGenome) mutate.Clone();

                // Find gene at random mutation point in tree
                var mutationPointIndex = TreeHelper.RandomGenerator.Next(genomeToMutate.NumberOfGenes);
                genomeToMutate.RootGene.RenumberIndicesOfBpmTree(n => n.Children);
                var mutationPointGene = genomeToMutate.RootGene.DepthFirstSearch(n => n.Children,
                    element => element.Index == mutationPointIndex);

                var mutationPointGeneDepth = mutationPointGene.CalculateNodeDepth();
                mutationPointGeneDepth = Math.Max(0,
                    ModelHelper.GetBpmModel().GetMaxDepthRandomGenome() - mutationPointGeneDepth);
                ProcessHelper.ProcessGenerator.GenerateRandomValidBpmGenome(mutationPointGeneDepth,
                    mutationPointGene.Parent, genomeToMutate,
                    mutationProbability);

                ProcessHelper.Validator.CheckForBpmnXorFailture(genomeToMutate.RootGene);
                return genomeToMutate;
            }
            throw new InvalidDataException($"{genome} should be of type BpmGenome, but is {genome.GetType()}");
        }
    }
}