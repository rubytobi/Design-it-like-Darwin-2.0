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

namespace PortableGeneticAlgorithm.Analytics
{
    public class FinishedSolution : Solution
    {
    }

    /// <summary>
    ///     Represents informations about a found solution, useed for exchang within the programm
    /// </summary>
    /*[Serializable]*/
    public abstract class Solution
    {
        /// <summary>
        ///     Creates a dummy solution!
        /// </summary>
        protected Solution()
        {
            Id = Guid.NewGuid().ToString();
            Fitness = double.NaN;
            EvaluationTime = double.NaN;
            Generation = int.MinValue;
        }

        protected Solution(double evaluationTime, int generation, double fitness) : this()
        {
            EvaluationTime = evaluationTime;
            Generation = generation;
            Fitness = fitness;
        }

        public string Id { get; set; }
        public double Fitness { get; set; }
        public double EvaluationTime { get; set; }
        public int Generation { get; set; }

        /// <inheritdoc />
        public override string ToString()
        {
            return Id;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is Solution)
                return Id.Equals(((Solution) obj).Id);
            return false;
        }

        /// <summary>
        ///     Returns a hashcode for the solution
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}