#region license
// MIT License
// 
// Copyright (c) [2017] [Tobias Ruby]
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
#endregion
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