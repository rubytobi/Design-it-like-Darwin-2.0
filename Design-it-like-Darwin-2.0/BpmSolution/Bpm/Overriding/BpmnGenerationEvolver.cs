using System.Collections.Generic;
using System.Linq;
using Bpm.Crossovers;
using Bpm.Helpers;
using Bpm.Mutations;
using PortableGeneticAlgorithm;
using PortableGeneticAlgorithm.Analytics;
using PortableGeneticAlgorithm.Interfaces;
using PortableGeneticAlgorithm.Predefined;
using System.Diagnostics;

namespace Bpm
{
    internal class BpmnGenerationEvolver : IGenerationEvolver
    {
        public int i { get; private set; } = 0;

        private IList<IGenome> DoCrossover(ISelection Selection, List<IGenome> lastGenerationGenomes)
        {
            ICrossover Crossover = new BpmnOnePointCrossover();
            int failures = 0;

            IList<IGenome> crossoverResult = new List<IGenome>();
            do
            {

                var parentOne = Selection.SelectGenome(lastGenerationGenomes);
                var parentTwo = Selection.SelectGenome(lastGenerationGenomes);
                crossoverResult = Crossover.PerformCrossover(parentOne, parentTwo);
            } while (ModelHelper.GetBpmModel().GetOnlyValidSolutions() &&
                     (!ProcessHelper.Validator.ValidateGenome((BpmGenome)crossoverResult[0], ref failures)
                      || !ProcessHelper.Validator.ValidateGenome((BpmGenome)crossoverResult[1], ref failures)));

            Debug.WriteLine("Crossover: " + crossoverResult.ElementAt(0));
            Debug.WriteLine("Crossover: " + crossoverResult.ElementAt(1));

            return crossoverResult;
        }

        private ISelection RandomSelect()
        {
            IList<ISelection> all = new List<ISelection>()
            {
                new ElitistSeletion(),
                new RouletteWheelSelection(),
                new TournamentSelection(ModelHelper.GetGePrModel().GetTournamentSize())
            };

            return all.ElementAt(TreeHelper.RandomGenerator.Next(all.Count));
        }

        private IGenome DoMutation(ISelection Selection, List<IGenome> lastGenerationGenomes)
        {
            int failures = 0;
            var mutation = new BpmnMutation();
            IGenome genomeToMutateOne = new BpmGenome();
            var mutatedGenome = new BpmGenome();

            do
            {
                genomeToMutateOne = Selection.SelectGenome(lastGenerationGenomes);
                mutatedGenome = mutation.Mutate(genomeToMutateOne, 1f) as BpmGenome;
            } while (ModelHelper.GetBpmModel().GetOnlyValidSolutions() && !ProcessHelper.Validator.ValidateGenome(mutatedGenome, ref failures));

            Debug.WriteLine("Mutation mutated " + genomeToMutateOne + " to " + mutatedGenome);

            return mutatedGenome;
        }

        public List<IGenome> EvolveGeneration(List<IGenome> lastGenerationGenomes)
        {
            var newGenerationGenomes = new List<IGenome>();

            // keep best solution
            var selectedGenomeElitist = new ElitistSeletion().SelectGenome(lastGenerationGenomes);
            newGenerationGenomes.Add(selectedGenomeElitist);

            // fill population
            for (i = 1; i < ModelHelper.GetGePrModel().GetPopulationSize(); i++)
            {
                ISelection selection = RandomSelect();

                int x = TreeHelper.RandomGenerator.Next(1);

                if (ModelHelper.GetGePrModel().GetPopulationSize() - i == 1)
                {
                    // fix remaing genome
                    x = 1;
                }

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
                        RootGene = ProcessHelper.ProcessGenerator.GenerateRandomValidBpmGenome2(ModelHelper.GetBpmModel().GetMaxDepthRandomGenome(), null)
                    };
                    genome = TreeHelper.ParseBpmGenome(genome.ToString());
                    failures = 0;
                } while (ModelHelper.GetBpmModel().GetOnlyValidSolutionsAtStart() && !ProcessHelper.Validator.ValidateGenome(genome, ref failures));

                genomes.Add(genome);
            }

            return genomes;
        }
    }
}