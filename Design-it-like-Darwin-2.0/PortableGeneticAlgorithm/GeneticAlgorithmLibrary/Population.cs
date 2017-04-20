using System;
using System.Collections.Generic;
using PortableGeneticAlgorithm.Analytics;
using PortableGeneticAlgorithm.Interfaces;
using System.Diagnostics;

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

            genomes.AddRange(GeneticAlgorithm.Instance().GetModel().GetGenerationEvolver().EvolveInitialGeneration(genomes));

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