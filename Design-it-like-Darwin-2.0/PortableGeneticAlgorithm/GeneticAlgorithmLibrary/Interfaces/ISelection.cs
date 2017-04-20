using System.Collections.Generic;

namespace PortableGeneticAlgorithm.Interfaces
{
    public interface ISelection
    {
        /// <summary>
        ///     Select one genome from the generation specified.
        /// </summary>
        /// <returns>The selected genome.</returns>
        IGenome SelectGenome(List<IGenome> generation);
    }
}