using Bpm.Fitnesses;
using PortableGeneticAlgorithm;
using PortableGeneticAlgorithm.Predefined;

namespace Bpm.Helpers
{
    public class ModelHelper
    {
        private static BpmModel _bpmModel;
        private static readonly BpmModel _defaultBpmModel = new BpmModel.Builder()
        .SetCostWeight(2)
            .SetTimeWeight(1)
            .SetAlpha(0.01)
            .SetI(0.025)
            .SetIfix(0.0)
            .SetIvar(0.0)
            .SetT(5)
            .SetN(100)
            .SetMaxDepthRandomGenome(5)
            .SetOnlyValidSolutions(false)
            .SetOnlyValidSolutionsAtStart(true)
            .SetPainFactor(10).Build();

        private static GePrModel _gePrModel;
        private static readonly GePrModel _defaultGePrModel = new GePrModel.Builder()
            .SetAdditionalAnalytics(BpmAnalytics.Instance())
            .SetEnableAnalytics(true)
            .SetFitness(typeof(BpmnFitness))
            .SetGenerationEvolver(new BpmnGenerationEvolver())
            .SetSolutionType(typeof(BpmSolution))
            .SetUseParalell(true)
            .SetCrossoverProbability(1)
            .SetInitialGenome(null)
            .SetMaximumNumberOfGenerations(50)
            .SetMutationProbability(0)
            .SetPopulationSize(100)
            .SetSeed(42)
            .SetTermination(new IterationTermination(50))
            .SetTournamentSize(3)
            .Build();

        public static void SetBpmModel(BpmModel model)
        {
            _bpmModel = model;
        }

        public static void SetGePrModel(GePrModel model)
        {
            _gePrModel = model;
        }

        public static BpmModel GetDefaultBpmModel()
        {
            return _defaultBpmModel;
        }

        public static GePrModel GetDefaultGePrModel()
        {
            return _defaultGePrModel;
        }

        public static GePrModel GetGePrModel()
        {
            if (_gePrModel == null)
                return _defaultGePrModel;

            return _gePrModel;
        }

        public static BpmModel GetBpmModel()
        {
            if (_bpmModel == null)
                return _defaultBpmModel;

            return _bpmModel;
        }
    }
}