using PortableGeneticAlgorithm.Interfaces;
using PortableGeneticAlgorithm.Termination;
using System.Collections.Generic;
using System.Linq;

namespace PortableGeneticAlgorithm.Predefined
{
    public class TerminationCombiner : TerminationBase
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the expected fitness to consider that termination has been reached.
        /// </summary>
        private List<ITermination> Terminations = new List<ITermination>();

        #endregion

        #region implemented abstract members of TerminationBase

        /// <summary>
        ///     Proofes if the specified GeneticAlgorithm algorithm reached the termination condition.
        /// </summary>
        /// <returns>True if termination has been reached, otherwise false.</returns>
        /// <param name="geneticAlgorithm">The genetic algorithm.</param>
        protected override bool PerformHasReached(GeneticAlgorithm geneticAlgorithm)
        {
            if (Terminations.Any(x => x.HasReached(geneticAlgorithm)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="TerminationCombiner" /> class.
        /// </summary>
        /// <param name="expectedFitness">Expected fitness.</param>
        public TerminationCombiner(params ITermination[] terminations)
        {
            if (terminations.Length == 0)
            {
                throw new System.Exception("at least one termination has to be defined for the termination combiner");
            }

            if (terminations.Any(x => x == null))
            {
                throw new System.Exception("null terminations are not accepted");
            }

            Terminations.AddRange(terminations);
        }

        public void AddTermination(ITermination termination)
        {
            Terminations.Add(termination);
        }

        #endregion
    }
}