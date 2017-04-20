using PortableGeneticAlgorithm.Analytics;

namespace PortableGeneticAlgorithm.Interfaces
{
    public interface IFitness
    {
        Solution Evaluate(IGenome genome);
    }

}