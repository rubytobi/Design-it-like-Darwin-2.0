using System.Collections.Generic;

namespace PortableGeneticAlgorithm.Interfaces
{
    public interface ICrossover
    {
        IList<IGenome> PerformCrossover(IGenome parentOne, IGenome parentTwo);
    }
}