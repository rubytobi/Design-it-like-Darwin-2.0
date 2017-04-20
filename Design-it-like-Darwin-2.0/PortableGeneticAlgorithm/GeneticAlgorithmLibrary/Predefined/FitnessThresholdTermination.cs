using PortableGeneticAlgorithm.Termination;

namespace PortableGeneticAlgorithm.Predefined
{
    public class FitnessThresholdTermination : TerminationBase
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the expected fitness to consider that termination has been reached.
        /// </summary>
        public double ExpectedFitness { get; set; }

        #endregion

        #region implemented abstract members of TerminationBase

        /// <summary>
        ///     Proofes if the specified GeneticAlgorithm algorithm reached the termination condition.
        /// </summary>
        /// <returns>True if termination has been reached, otherwise false.</returns>
        /// <param name="geneticAlgorithm">The genetic algorithm.</param>
        protected override bool PerformHasReached(GeneticAlgorithm geneticAlgorithm)
        {
            return geneticAlgorithm.BestGenome.Fitness >= ExpectedFitness;
        }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="FitnessThresholdTermination" /> class.
        /// </summary>
        /// <remarks>
        ///     The default expected fitness is 1.00.
        /// </remarks>
        public FitnessThresholdTermination() : this(1.00)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="FitnessThresholdTermination" /> class.
        /// </summary>
        /// <param name="expectedFitness">Expected fitness.</param>
        public FitnessThresholdTermination(double expectedFitness)
        {
            ExpectedFitness = expectedFitness;
        }

        #endregion
    }
}