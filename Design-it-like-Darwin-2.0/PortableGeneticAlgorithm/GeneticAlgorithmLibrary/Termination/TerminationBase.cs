using System;
using PortableGeneticAlgorithm.Interfaces;

namespace PortableGeneticAlgorithm.Termination
{
    /// <summary>
    ///     Base class for ITerminations implementations.
    /// </summary>
    public abstract class TerminationBase : ITermination
    {
        #region Fields

        private bool _hasReached;

        #endregion

        #region Methods

        /// <summary>
        ///     Proofs if the specified GeneticAlgorithm reached the termination condition.
        /// </summary>
        /// <returns>True if termination condition has been reached, else false.</returns>
        /// <param name="geneticAlgorithm">The genetic proramming algorithm.</param>
        public bool HasReached(GeneticAlgorithm geneticAlgorithm)
        {
            if (geneticAlgorithm == null)
                throw new NullReferenceException(nameof(geneticAlgorithm));

            _hasReached = PerformHasReached(geneticAlgorithm);

            return _hasReached;
        }

        public override string ToString()
        {
            return $"{GetType().Name} (HasReached: {_hasReached})";
        }

        /// <summary>
        ///     Proofs if the specified GeneticAlgorithm reached the termination condition.
        /// </summary>
        /// <returns>True if termination has been reached, else false.</returns>
        /// <param name="geneticAlgorithm">The genetic programming algorithm.</param>
        protected abstract bool PerformHasReached(GeneticAlgorithm geneticAlgorithm);

        #endregion
    }
}