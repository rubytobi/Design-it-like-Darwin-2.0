namespace PortableGeneticAlgorithm.Interfaces
{
    public interface ITermination
    {
        #region Methods

        /// <summary>
        ///     Proofs if the specified GeneticAlgorithm reached the termination condition.
        /// </summary>
        /// <returns>True if termination condition has been reached, else false.</returns>
        /// <param name="geneticAlgorithm">The genetic proramming algorithm.</param>
        bool HasReached(GeneticAlgorithm geneticAlgorithm);

        #endregion
    }
}