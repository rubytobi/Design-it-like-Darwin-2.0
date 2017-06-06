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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Bpm.Crossovers;
using Bpm.Helpers;
using Bpm.Mutations;
using PortableGeneticAlgorithm.Interfaces;
using PortableGeneticAlgorithm.Predefined;

namespace Bpm
{
    internal class BpmnGenerationEvolver : IGenerationEvolver
    {
        public int i { get; private set; }

        public List<IGenome> EvolveGeneration(List<IGenome> lastGenerationGenomes)
        {
            var newGenerationGenomes = new List<IGenome>();

            // keep best solution
            var selectedGenomeElitist = new ElitistSeletion().SelectGenome(lastGenerationGenomes);
            newGenerationGenomes.Add(selectedGenomeElitist);

            // fill population
            for (i = 1; i < ModelHelper.GetGePrModel().GetPopulationSize(); i++)
            {
                var selection = RandomSelect();

                var x = TreeHelper.RandomGenerator.Next(1);

                if (ModelHelper.GetGePrModel().GetPopulationSize() - i == 1)
                    x = 1;

                switch (x)
                {
                    case 0:
                        newGenerationGenomes.AddRange(DoCrossover(selection, lastGenerationGenomes));
                        i++; // as we create two new genomes
                        break;
                    case 1:
                        newGenerationGenomes.Add(DoMutation(selection, lastGenerationGenomes));
                        break;
                }
            }

            return newGenerationGenomes;
        }

        public List<IGenome> EvolveInitialGeneration(List<IGenome> preExistingGenomes)
        {
            var genomes = new List<IGenome>();

            for (i = preExistingGenomes.Count; i < ModelHelper.GetGePrModel().GetPopulationSize(); i++)
            {
                var failures = 0;
                BpmGenome genome = null;
                do
                {
                    genome = new BpmGenome
                    {
                        RootGene =
                            ProcessHelper.ProcessGenerator.GenerateRandomValidBpmGenome2(
                                ModelHelper.GetBpmModel().GetMaxDepthRandomGenome(), null)
                    };
                    genome = genome.ToString().ParseBpmGenome();
                    failures = 0;
                } while (ModelHelper.GetBpmModel().GetOnlyValidSolutionsAtStart() &&
                         !ProcessHelper.Validator.ValidateGenome(genome, ref failures));

                genomes.Add(genome);
            }

            return genomes;
        }

        private IList<IGenome> DoCrossover(ISelection Selection, List<IGenome> lastGenerationGenomes)
        {
            ICrossover Crossover = new BpmnOnePointCrossover();
            var failures = 0;

            IList<IGenome> crossoverResult = new List<IGenome>();
            do
            {
                var parentOne = Selection.SelectGenome(lastGenerationGenomes);
                var parentTwo = Selection.SelectGenome(lastGenerationGenomes);
                crossoverResult = Crossover.PerformCrossover(parentOne, parentTwo);
            } while (ModelHelper.GetBpmModel().GetOnlyValidSolutions() &&
                     (!ProcessHelper.Validator.ValidateGenome((BpmGenome) crossoverResult[0], ref failures)
                      || !ProcessHelper.Validator.ValidateGenome((BpmGenome) crossoverResult[1], ref failures)));

            Debug.WriteLine("Crossover: " + crossoverResult.ElementAt(0));
            Debug.WriteLine("Crossover: " + crossoverResult.ElementAt(1));

            return crossoverResult;
        }

        private ISelection RandomSelect()
        {
            IList<ISelection> all = new List<ISelection>
            {
                new ElitistSeletion(),
                new RouletteWheelSelection(),
                new TournamentSelection(ModelHelper.GetGePrModel().GetTournamentSize())
            };

            return all.ElementAt(TreeHelper.RandomGenerator.Next(all.Count));
        }

        private IGenome DoMutation(ISelection Selection, List<IGenome> lastGenerationGenomes)
        {
            var failures = 0;
            var mutation = new BpmnMutation();
            IGenome genomeToMutateOne = new BpmGenome();
            var mutatedGenome = new BpmGenome();

            do
            {
                genomeToMutateOne = Selection.SelectGenome(lastGenerationGenomes);
                mutatedGenome = mutation.Mutate(genomeToMutateOne, 1f) as BpmGenome;
            } while (ModelHelper.GetBpmModel().GetOnlyValidSolutions() &&
                     !ProcessHelper.Validator.ValidateGenome(mutatedGenome, ref failures));

            Debug.WriteLine("Mutation mutated " + genomeToMutateOne + " to " + mutatedGenome);

            return mutatedGenome;
        }
    }
}