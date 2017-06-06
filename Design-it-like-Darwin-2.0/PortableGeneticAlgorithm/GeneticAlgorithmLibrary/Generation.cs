﻿#region license
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

namespace PortableGeneticAlgorithm
{
    /// <summary>
    ///     Represents a Generation of a Population.
    /// </summary>
    public class Generation
    {
        #region Constructor

        /// <summary>
        ///     Initializes a new instance of the <see cref="Generation" /> class.
        /// </summary>
        /// <param name="generationIndex">
        ///     Index of the Generation. Index starts in 0 and expands with evolving, Generation by
        ///     Generation.
        /// </param>
        /// <param name="genomes">List of Genomes of the Generation</param>
        public Generation(int generationIndex, IList<IGenome> genomes)
        {
            if (GenerationIndex < 0)
                throw new ArgumentOutOfRangeException(
                    nameof(generationIndex),
                    $"The number {generationIndex} is not valid as a generation index. It must be positive and start in 0.");
            if (genomes == null || genomes.Count < 2)
                throw new ArgumentOutOfRangeException(nameof(genomes),
                    "A generation must have at least 2 chromosomes.");

            GenerationIndex = generationIndex;
            Genomes = genomes;
        }

        #endregion //Constructor

        #region Properties

        /// <summary>
        ///     Gets Number of the Generation. Index starts at 0 and increases with evolving, Generation by Generation.
        /// </summary>
        public int GenerationIndex { get; set; }

        /// <summary>
        ///     Gets All Genomes which are Members of the Generation.
        /// </summary>
        public IList<IGenome> Genomes { get; internal set; }

        /// <summary>
        ///     Gets Genome with the best Fitness in an finished Generation.
        /// </summary>
        public IGenome BestGenome { get; internal set; }

        #endregion //Properties

        #region Methods

        /// <summary>
        ///     Finishes Generation, by ordering Genomes by Fitness descending and
        ///     setting the BestGenome Property.
        /// </summary>
        public void FinishGeneration()
        {
            Genomes = Genomes
                .OrderByDescending(c => c.Fitness.Value)
                .ToList();

            // TODO: Write AnalyseData

            BestGenome = Genomes.First();
        }

        /// <summary>
        ///     Ends the generation.
        /// </summary>
        /// <param name="genomesNumber">Genomes number to keep on generation.</param>
        public void End(int genomesNumber)
        {
            Genomes = Genomes
                .Where(ValidateChromosome)
                .OrderByDescending(c => c.Fitness.Value)
                .ToList();

            if (Genomes.Count > genomesNumber)
                Genomes = Genomes.Take(genomesNumber).ToList();

            BestGenome = Genomes.First();
        }

        /// <summary>
        ///     Validates the genome.
        /// </summary>
        /// <param name="genome">The genome to validate.</param>
        /// <returns>True if a genome has a fitness value.</returns>
        private static bool ValidateChromosome(IGenome genome)
        {
            if (!genome.Fitness.HasValue)
                throw new InvalidOperationException("At least one of the genomes has no fitness value!");

            return true;
        }

        #endregion //Methods
    }
}