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
using PortableGeneticAlgorithm.Termination;

namespace PortableGeneticAlgorithm.Predefined
{
    public class FitnessThresholdTermination : TerminationBase
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the expected fitness to consider that termination has been reached.
        /// </summary>
        public double ExpectedFitness { get; set; }

        #endregion

        #region implemented abstract members of TerminationBase

        /// <summary>
        ///     Proofes if the specified GeneticAlgorithm algorithm reached the termination condition.
        /// </summary>
        /// <returns>True if termination has been reached, otherwise false.</returns>
        /// <param name="geneticAlgorithm">The genetic algorithm.</param>
        protected override bool PerformHasReached(GeneticAlgorithm geneticAlgorithm)
        {
            return geneticAlgorithm.BestGenome.Fitness >= ExpectedFitness;
        }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="FitnessThresholdTermination" /> class.
        /// </summary>
        /// <remarks>
        ///     The default expected fitness is 1.00.
        /// </remarks>
        public FitnessThresholdTermination() : this(1.00)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="FitnessThresholdTermination" /> class.
        /// </summary>
        /// <param name="expectedFitness">Expected fitness.</param>
        public FitnessThresholdTermination(double expectedFitness)
        {
            ExpectedFitness = expectedFitness;
        }

        #endregion
    }
}