namespace PortableGeneticAlgorithm.Interfaces
{
    public interface IGenome
    {
        double? Fitness { get; set; }
        int NumberOfGenes { get; }
        string Id { get; }
        IGenome Clone();
    }
}