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
using PortableGeneticAlgorithm.Termination;

namespace PortableGeneticAlgorithm.Predefined
{
    public class TerminationCombiner : TerminationBase
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the expected fitness to consider that termination has been reached.
        /// </summary>
        private readonly List<ITermination> Terminations = new List<ITermination>();

        #endregion

        #region implemented abstract members of TerminationBase

        /// <summary>
        ///     Proofes if the specified GeneticAlgorithm algorithm reached the termination condition.
        /// </summary>
        /// <returns>True if termination has been reached, otherwise false.</returns>
        /// <param name="geneticAlgorithm">The genetic algorithm.</param>
        protected override bool PerformHasReached(GeneticAlgorithm geneticAlgorithm)
        {
            if (Terminations.Any(x => x.HasReached(geneticAlgorithm)))
                return true;
            return false;
        }

        public new void Reset()
        {
            foreach (var termination in Terminations)
                termination.Reset();
        }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="TerminationCombiner" /> class.
        /// </summary>
        /// <param name="expectedFitness">Expected fitness.</param>
        public TerminationCombiner(params ITermination[] terminations)
        {
            if (terminations.Length == 0)
                throw new Exception("at least one termination has to be defined for the termination combiner");

            if (terminations.Any(x => x == null))
                throw new Exception("null terminations are not accepted");

            Terminations.AddRange(terminations);
        }

        public void AddTermination(ITermination termination)
        {
            Terminations.Add(termination);
        }

        #endregion
    }
}