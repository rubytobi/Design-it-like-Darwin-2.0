using System;
using System.Diagnostics;
using Bpm.Helpers;
using PortableGeneticAlgorithm.Interfaces;

namespace BpmApi.Models
{
    public class SettingsAlgorithmModel
    {
        private static SettingsAlgorithmModel _default = new SettingsAlgorithmModel
        {
            CrossoverProbability = 1.0,
            InitialGenome = "",
            MaximumNumberOfGenerations = 50,
            MutationProbability = 0,
            PopulationSize = 100,
            Seed = 42,
            TournamentSize = 5
        };

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