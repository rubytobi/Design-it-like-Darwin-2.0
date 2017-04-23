using System.Collections.Generic;
using PortableGeneticAlgorithm.Analytics;

namespace PortableGeneticAlgorithm.Interfaces
{
    public interface IAnalytics
    {
        void ConsumeSolution(Solution solution);
        void ConsumeSolutions(List<Solution> allSolutions);
    }
}