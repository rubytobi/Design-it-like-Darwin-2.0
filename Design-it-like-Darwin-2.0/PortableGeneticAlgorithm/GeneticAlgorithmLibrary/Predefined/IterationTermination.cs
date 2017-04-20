using PortableGeneticAlgorithm.Termination;

namespace PortableGeneticAlgorithm.Predefined
{
    public class IterationTermination : TerminationBase
    {
        #region Fields

        private readonly int _iterationThreshold;

        #endregion //Fields

        #region Constructor

        public IterationTermination(int iterationThreshold)
        {
            _iterationThreshold = iterationThreshold;
        }

        #endregion //Constructor

        protected override bool PerformHasReached(GeneticAlgorithm geneticAlgorithm)
        {
            return geneticAlgorithm.Population.CurrentGeneration.GenerationIndex >= _iterationThreshold;
        }
    }
}