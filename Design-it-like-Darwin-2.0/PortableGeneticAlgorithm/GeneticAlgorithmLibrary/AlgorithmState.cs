namespace PortableGeneticAlgorithm.GeneticAlgorithmLibrary
{
    public class Status
    {
        public bool Initialisation { get; internal set; }
        public bool Ready { get; internal set; }
        public bool Running { get; internal set; }
        public bool Stopped { get; internal set; }
        public bool StopRequested { get; internal set; }
    }
}