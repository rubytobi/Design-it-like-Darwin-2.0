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
using PortableGeneticAlgorithm.Analytics;

namespace Bpm
{
    /// <summary>
    ///     Represents informations about a found solution, useed for exchang within the programm
    /// </summary>
    public class BpmSolution : Solution
    {
        /// <summary>
        ///     dummy contstructor, do not use for any data handling!
        /// </summary>
        public BpmSolution()
        {
        }

        public BpmSolution(double evaluationTime, int generation,
            double fitness, string process, bool validGenome, string activityList, double mueP, double mueNpv,
            double sigma2P, double sigma2Npv, double timeFitness, double costFitness)
            : base(evaluationTime, generation, fitness)
        {
            Process = process;
            ValidGenome = validGenome;
            ActivityList = activityList;
            MueP = mueP;
            MueNpv = mueNpv;
            Sigma2P = sigma2P;
            Sigma2Npv = sigma2Npv;
            TimeFitness = timeFitness;
            CostFitness = costFitness;
        }

        public double CostFitness { get; set; }
        public double TimeFitness { get; set; }
        public string Process { get; set; }
        public bool ValidGenome { get; set; }
        public string ActivityList { get; set; }
        public double MueP { get; set; }
        public double MueNpv { get; set; }
        public double Sigma2P { get; set; }
        public double Sigma2Npv { get; set; }

        /// <inheritdoc />
        public override string ToString()
        {
            return ("" + Fitness).PadLeft(20) + "    " + Process;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is BpmSolution)
                return Process.Equals(((BpmSolution) obj).Process) && Fitness.Equals(((BpmSolution) obj).Fitness);
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