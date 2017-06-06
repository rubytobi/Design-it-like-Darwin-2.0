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
using System.Diagnostics;
using PortableGeneticAlgorithm.Interfaces;

namespace PortableGeneticAlgorithm
{
    public class Population : IPopulation
    {
        public Population(IGenome initialGenome)
        {
            if (initialGenome == null)
                Debug.WriteLine("no initial genome specified.");

            CreationDate = DateTime.Now;
            InitialGenome = initialGenome;
            Generations = new List<Generation>();
        }

        #region Events

        /// <summary>
        ///     Occurs when best chromosome changed.
        /// </summary>
        public event EventHandler BestGenomeChanged;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the current generation.
        /// </summary>
        /// <value>The current generation.</value>
        public Generation CurrentGeneration { get; protected set; }

        /// <summary>
        ///     Gets or sets the total number of generations executed.
        /// </summary>
        public int NumberOfGenerations { get; protected set; }

        public List<Generation> Generations { get; set; }

        public IGenome InitialGenome { get; set; }

        public IGenome BestGenome { get; protected set; }

        public DateTime CreationDate { get; set; }

        #endregion //Properties

        #region Methods

        /// <summary>
        ///     Creates the initial generation.
        /// </summary>
        public virtual void CreateInitialGeneration(IGenome initialGenome)
        {
            var genomes = new List<IGenome>();

            if (initialGenome != null)
                genomes.Add(initialGenome);

            genomes.AddRange(GeneticAlgorithm.Instance()
                .GetModel()
                .GetGenerationEvolver()
                .EvolveInitialGeneration(genomes));

            CreateNewGeneration(genomes);
        }

        /// <summary>
        ///     Creates a new generation.
        /// </summary>
        /// <param name="genes">The genes to build the new generation of.</param>
        public virtual void CreateNewGeneration(IList<IGenome> genes)
        {
            if (genes == null)
                throw new NullReferenceException(nameof(genes));

            CurrentGeneration = new Generation(++NumberOfGenerations, genes);
            Generations.Add(CurrentGeneration);
        }

        /// <summary>
        ///     Ends the current generation.
        /// </summary>
        public virtual void EndCurrentGeneration()
        {
            CurrentGeneration.End(CurrentGeneration.Genomes.Count);

            if (BestGenome != CurrentGeneration.BestGenome)
            {
                BestGenome = CurrentGeneration.BestGenome;

                OnBestGenomeChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Raises the best chromosome changed event.
        /// </summary>
        /// <param name="args">The event arguments.</param>
        protected virtual void OnBestGenomeChanged(EventArgs args)
        {
            BestGenomeChanged?.Invoke(this, args);
        }

        #endregion
    }
}