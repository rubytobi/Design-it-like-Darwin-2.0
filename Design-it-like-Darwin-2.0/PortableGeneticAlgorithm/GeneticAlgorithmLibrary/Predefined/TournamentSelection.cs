using System;
using System.Collections.Generic;
using System.Linq;
using PortableGeneticAlgorithm.Interfaces;

namespace PortableGeneticAlgorithm.Predefined
{
    /// <summary>
    ///     Tournament selection involves running several "tournaments" among a few individuals chosen at random from the
    ///     population.
    ///     The winner of each tournament (the one with the best fitness) is selected for crossover.
    ///     <remarks>
    ///         Selection pressure is easily adjusted by changing the tournament size.
    ///         If the tournament size is larger, weak individuals have a smaller chance to be selected.
    ///     </remarks>
    /// </summary>
    public class TournamentSelection : ISelection
    {
        #region Constructors

        public TournamentSelection(int size) : this(size, true)
        {
        }

        public TournamentSelection(int size, bool allowWinnerCompeteNextTournament)
        {
            Size = size;
            AllowWinnerCompeteNextTournament = allowWinnerCompeteNextTournament;
        }

        #endregion

        #region Properties

        public int Size { get; set; }

        public bool AllowWinnerCompeteNextTournament { get; set; }

        #endregion

        #region Methods

        public IGenome SelectGenome(List<IGenome> generation)
        {
            var candidates = generation.ToList();
            var randomIndexes = GetUniqueInts(Size, 0, candidates.Count);
            return
                candidates.Where((c, i) => randomIndexes.Contains(i)).OrderByDescending(c => c.Fitness).First().Clone();
        }

        /// <summary>
        ///     Gets an integer array with unique values between minimum value (inclusive) and maximum value (exclusive).
        /// </summary>
        /// <returns>The integer array.</returns>
        /// <param name="size">The array length.</param>
        /// <param name="min">Minimum value (inclusive).</param>
        /// <param name="max">Maximum value (exclusive).</param>
        public virtual int[] GetUniqueInts(int size, int min, int max)
        {
            var diff = max - min;

            if (diff < size)
                throw new ArgumentOutOfRangeException(
                    nameof(size),
                    $"The length is {size}, but the possible unique values between {min} (inclusive) and {max} (exclusive) are {diff}.");

            var orderedValues = Enumerable.Range(min, diff).ToList();
            var ints = new int[size];

            for (var i = 0; i < size; i++)
            {
                var removeIndex = Helper.RandomGenerator.Next(0, orderedValues.Count);
                ints[i] = orderedValues[removeIndex];
                orderedValues.RemoveAt(removeIndex);
            }

            return ints;
        }

        #endregion
    }
}