using PortableGeneticAlgorithm.Analytics;
using System;

namespace Bpm
{
    /// <summary>
    ///     Represents informations about a found solution, useed for exchang within the programm
    /// </summary>
    public class BpmSolution : Solution
    {
        /// <summary>
        /// dummy contstructor, do not use for any data handling!
        /// </summary>
        public BpmSolution() : base()
        {

        }

        public BpmSolution(double evaluationTime, int generation,
            double fitness, string process, bool validGenome, string activityList, double mueP, double mueNpv, double sigma2P, double sigma2Npv, double timeFitness, double costFitness)
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
        public Boolean ValidGenome { get; set; }
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
                return Process.Equals(((BpmSolution)obj).Process) && Fitness.Equals(((BpmSolution)obj).Fitness);
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