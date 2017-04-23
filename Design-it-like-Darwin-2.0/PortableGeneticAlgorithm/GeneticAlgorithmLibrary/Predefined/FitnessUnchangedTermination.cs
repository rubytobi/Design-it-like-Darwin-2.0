using System.Collections.Generic;
using PortableGeneticAlgorithm.Termination;

namespace PortableGeneticAlgorithm.Predefined
{
    public class FitnessUnchangedTermination : TerminationBase
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="FitnessThresholdTermination" /> class.
        /// </summary>
        /// <param name="expectedFitness">Expected fitness.</param>
        public FitnessUnchangedTermination(int rounds)
        {
            UnchangedRounds = rounds;
        }

        #endregion

        #region implemented abstract members of TerminationBase

        /// <summary>
        ///     Proofes if the specified GeneticAlgorithm algorithm reached the termination condition.
        /// </summary>
        /// <returns>True if termination has been reached, otherwise false.</returns>
        /// <param name="geneticAlgorithm">The genetic algorithm.</param>
        protected override bool PerformHasReached(GeneticAlgorithm geneticAlgorithm)
        {
            if (geneticAlgorithm.BestGenome.Fitness == null)
                return false;

            var fitness = geneticAlgorithm.BestGenome.Fitness.Value;

            if (!PastFitnesses.Contains(fitness))
            {
                // found new best fitness, dismiss old ones
                PastFitnesses.Clear();
                PastFitnesses.Add(fitness);
                return false;
            }

            PastFitnesses.Add(fitness);
            return PastFitnesses.Count == UnchangedRounds - 1;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the expected fitness to consider that termination has been reached.
        /// </summary>
        public double UnchangedRounds { get; }

        private readonly List<double> PastFitnesses = new List<double>();

        #endregion
    }
}