namespace PortableGeneticAlgorithm.Interfaces
{
    public interface IMutation
    {
        IGenome Mutate(IGenome genome, double mutationProbability);
    }
}