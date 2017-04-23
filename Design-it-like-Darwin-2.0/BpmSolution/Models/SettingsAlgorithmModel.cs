using System;
using System.Diagnostics;
using Bpm.Helpers;
using PortableGeneticAlgorithm.Interfaces;

namespace BpmApi.Models
{
    public class SettingsAlgorithmModel
    {
        public double CrossoverProbability { get; set; }
        public string InitialGenome { get; set; }
        public int MaximumNumberOfGenerations { get; set; }
        public double MutationProbability { get; set; }
        public int PopulationSize { get; set; }
        public int Seed { get; set; }
        public int TournamentSize { get; set; }

        public IGenome ToIGenome()
        {
            try
            {
                return InitialGenome.ParseBpmGenome();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }
    }
}