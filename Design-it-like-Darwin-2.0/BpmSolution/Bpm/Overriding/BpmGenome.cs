using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Bpm.Helpers;
using PortableGeneticAlgorithm.Interfaces;

namespace Bpm
{
    /// <summary>
    ///     Genome class for bpmn processes
    /// </summary>
    public class BpmGenome : IGenome
    {
        #region Constructor

        /// <summary>
        ///     Creates a new bpmn genome
        /// </summary>
        public BpmGenome()
        {
            Id = Guid.NewGuid().ToString();
        }

        #endregion //Constructor

        #region Properties

        /// <summary>
        ///     Random string
        /// </summary>
        public string Id { get; }

        /// <summary>
        ///     The RootGene has no Parent (Parent == null).
        /// </summary>
        public BpmGene RootGene { get; set; }

        /// <summary>
        ///     Fitness value of the Genome.
        /// </summary>
        public double? Fitness { get; set; }

        /// <summary>
        ///     Gets the Number of Genes/Nodes in the GenomeTree by Depth-First Search.
        /// </summary>
        public int NumberOfGenes
        {
            get
            {
                if (RootGene == null)
                    throw new NullReferenceException(nameof(NumberOfGenes));
                return RootGene.GetNumberOfNodes(gen => gen.Children);
            }
        }

        #endregion //Properties

        #region Methods

        /// <summary>
        ///     https://bytes.com/topic/c-sharp/answers/671528-c-random-alphanumeric-strings
        /// </summary>
        /// <param name="maxSize">requested random string length</param>
        /// <returns>random string of specified length</returns>
        [Obsolete]
        public static string GetUniqueKey(int maxSize)
        {
            var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            var data = new byte[1];

            var crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            data = new byte[maxSize];
            crypto.GetNonZeroBytes(data);

            var result = new StringBuilder(maxSize);
            foreach (var b in data)
                result.Append(chars[b % (chars.Length - 1)]);

            return result.ToString();
        }

        /// <summary>
        ///     Adds a BpmGene as a child of
        ///     <param name="parent"></param>
        ///     and gets the children of the
        ///     <param name="parent"></param>
        ///     as its
        ///     own children.
        /// </summary>
        /// <param name="parent">Parent of the added gene.</param>
        /// <param name="newGene">Gene to add to the tree.</param>
        public void AddGene(BpmGene parent, BpmGene newGene)
        {
            newGene.Parent = parent;
            newGene.Children = parent.Children;
            parent.Children = new List<BpmGene> {newGene};
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "<PI;" + RootGene + ";PO>";
        }

        public IGenome Clone()
        {
            var thisGenomeAsString = ToString();
            return thisGenomeAsString.ParseBpmGenome();
        }

        #endregion //Methods
    }
}