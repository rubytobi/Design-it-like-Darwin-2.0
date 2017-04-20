using System.Collections.Generic;
using System.Linq;
using PortableGeneticAlgorithm.Interfaces;

namespace PortableGeneticAlgorithm.Predefined
{
    public sealed class ElitistSeletion : ISelection
    {
        #region Constructor

        #endregion //Constructor

        #region ISelection implementation

        /// <summary>
        ///     Performs the elitist selection on the specified generation.
        /// </summary>
        /// <param name="generation">The generation, the genome is selected out of.</param>
        /// <returns>The select chromosomes.</returns>
        public IGenome SelectGenome(List<IGenome> generation)
        {
            var ordered = generation.OrderByDescending(c => c.Fitness);
            return ordered.First().Clone();
        }

        #endregion
    }
}