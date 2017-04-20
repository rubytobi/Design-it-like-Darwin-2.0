using System.Collections.Generic;

namespace PortableGeneticAlgorithm.Interfaces
{
    public interface IGenerationEvolver
    {
        int i { get; }
        List<IGenome> EvolveGeneration(List<IGenome> lastGenerationGenomes);
        List<IGenome> EvolveInitialGeneration(List<IGenome> preExistingGenomes);
    }
}