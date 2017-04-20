using System;
using System.Collections.Generic;
using System.Linq;
using PortableGeneticAlgorithm.Interfaces;

namespace PortableGeneticAlgorithm.Predefined
{
    /// <summary>
    ///     Implementation of Roulette Wheel Selection
    ///     <see href="http://en.wikipedia.org/wiki/Fitness_proportionate_selection">Wikipedia</see>
    /// </summary>
    public class RouletteWheelSelection : ISelection
    {
        #region ISelection implementation

        /// <summary>
        ///     Selects one IGenome form the specified generation.
        /// </summary>
        /// <param name="generation">Generation to select genome from.</param>
        /// <returns></returns>
        public IGenome SelectGenome(List<IGenome> generation)
        {
            var genomes = generation;
            var rouletteWheel = new List<double>();

            var minFitness = genomes.Min(x => x.Fitness.Value);

            // Calculate cumulative percent fitness, adjusted with minFitness (mostly negative!) plus one to get at least
            // one part of the pie ;-)
            var sumFitness = genomes.Sum(c => c.Fitness.Value + minFitness + 1);
            var cumulativePercent = 0.0;
            foreach (var genome in genomes)
            {
                cumulativePercent += (genome.Fitness.Value + minFitness + 1) / sumFitness;
                rouletteWheel.Add(cumulativePercent);
            }

            // Select from wheel
            // TODO stimmt das hier noch mit der Auswahl? Habe oben minFitness jeweils drauf gezählt wg. negativer Werte...
            var randomDouble = Helper.RandomGenerator.NextDouble();
            var chromosomeIndex = rouletteWheel
                .Select((value, index) => new {Value = value, Index = index})
                .FirstOrDefault(r => r.Value >= randomDouble)
                .Index;
            var selected = genomes[chromosomeIndex];
            if (selected == null)
                throw new NullReferenceException(nameof(selected));
            return selected.Clone();
        }
    }

    #endregion
}