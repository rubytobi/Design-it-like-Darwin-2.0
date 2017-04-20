using System.Collections.Generic;

namespace PortableGeneticAlgorithm.Interfaces
{
    public interface IPopulation
    {
        int NumberOfGenerations { get; }
        Generation CurrentGeneration { get; }
        IGenome BestGenome { get; }
        IGenome InitialGenome { get; }
        void CreateInitialGeneration(IGenome initialGenome);
        void CreateNewGeneration(IList<IGenome> genes);
        void EndCurrentGeneration();
    }
}