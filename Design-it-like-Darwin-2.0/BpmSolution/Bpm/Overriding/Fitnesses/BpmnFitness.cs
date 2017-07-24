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
using System.Linq;
using Bpm.Helpers;
using Bpm.NotationElements;
using Bpm.NotationElements.Gateways;
using PortableGeneticAlgorithm;
using PortableGeneticAlgorithm.Analytics;
using PortableGeneticAlgorithm.Interfaces;

namespace Bpm.Fitnesses
{
    /// <summary>
    ///     A class to calculate a fitness value for an BPMN-Genome
    /// </summary>
    public class BpmnFitness : IFitness
    {
        private readonly bool pretty;
        private double painFactor;

        public BpmnFitness()
        {
            pretty = true;
        }

        public BpmnFitness(bool pretty = true)
        {
            this.pretty = pretty;
        }

        /// <summary>
        ///     Evaluates the Genome according to mü-sigma and the Preference-Functional
        /// </summary>
        /// <param name="genome">Genome to evaluate</param>
        /// <returns>
        ///     Double.NegativeInfinity in case of (1) there was no genome, (2) it was not an BPMN-Genome, (3) the genome has
        ///     not root-element, (4) the object-dependencies are not correct or (5) the probability for every activity was not
        ///     correct
        /// </returns>
        public Solution Evaluate(IGenome genome)
        {
            // Check if genome is null, not an BpmGenome or invalide
            if (genome == null || genome.GetType() != typeof(BpmGenome) || ((BpmGenome) genome).RootGene == null)
                throw new Exception("no valid Genome received");

            return Evaluate(genome as BpmGenome);
        }

        private BpmSolution Evaluate(BpmGenome genome)
        {
            // start time measurement
            var startTime = DateTime.Now;

            Debug.WriteLine("Evaluating: " + genome);

            // load pain factor
            painFactor = ModelHelper.GetBpmModel().GetPainFactor();

            #region costevaluation

            // calculate all process paths
            // - splitted by XOR for dependent probabilities
            var splitterXor = new ProcessHelper.PathSplitter(genome);
            var pathsForProbabilities = splitterXor.GetPaths();

            // remove paths with 0.0 percentage probability
            pathsForProbabilities.RemoveAll(x => x.Probability == 0);

            // calculate probabilities for each activity

            //

            // [2] Wahrscheinlichkeiten für jede Activity
            var activitieProbabilities = new HashSet<BpmnActivity>();

            var painFactorActivityAttributeCovering = false;
            var painFactorObjectDependencies = 0;

            // Calculate probabilities for every activity
            CalculateActivityProbabilities(ref activitieProbabilities, pathsForProbabilities);

            // Check resulting probabilities with input data
            painFactorActivityAttributeCovering = !CheckActivityAttributeCovering(genome.RootGene);

            // Check input/ouput realtionships from activities to objects
            //painFactorObjectDependencies = !CheckObjectDependencies();
            var valide = ProcessHelper.Validator.ValidateGenome(genome, ref painFactorObjectDependencies);

            // Calculate µNPV
            var mueP = CalculateMueP(valide, activitieProbabilities);
            var mueNpv = CalculateMueNpv(genome, mueP);

            // Calculate σ^2NPV
            var sigma2P = CalculateSigma2P(mueP, activitieProbabilities, pathsForProbabilities);
            var sigma2Npv = CalculateSigma2Npv(sigma2P);

            // Calculate preference function
            var costFitness = mueNpv - ModelHelper.GetBpmModel().GetAlpha() / 2 * sigma2Npv;

            Debug.WriteLine("costFitness: " + costFitness + " = " + mueNpv + " - (" +
                            ModelHelper.GetBpmModel().GetAlpha() + "/2) * " + sigma2Npv);

            #endregion

            #region timeevaluation

            double timeFitness = 0;
            foreach (var bpa in DataHelper.ActivityAttributeHelper.Instance().GetAll())
                timeFitness += bpa.DecisionProbability * CalculateTime(bpa, genome.RootGene);

            if (DataHelper.ActivityAttributeHelper.Instance().GetAll().Count <= 0)
            {
                timeFitness = TreeHelper.CalculateLongestTime(genome.RootGene);
            }
            #endregion

            #region fitness merge

            var fitness = costFitness * ModelHelper.GetBpmModel().GetCostWeight();
            fitness += -1 * timeFitness * ModelHelper.GetBpmModel().GetTimeWeight();

            #endregion

            if (painFactorActivityAttributeCovering) fitness -= ModelHelper.GetBpmModel().GetPainFactor();
            if (painFactorObjectDependencies > 0)
                fitness -= ModelHelper.GetBpmModel().GetPainFactor() * painFactorObjectDependencies;

            // only when pretty process enabled
            if (pretty) fitness -= ModelHelper.GetBpmModel().GetPainFactor() * ProcessHelper.PrettyPrint.Check(genome);

            var endTime = DateTime.Now;

            var activities = activitieProbabilities.Select(x => x.Name).Distinct().ToList();

            var index = GeneticAlgorithm.Instance()?.Population?.CurrentGeneration?.GenerationIndex;

            if (!index.HasValue)
                index = -1;

            var solution = new BpmSolution(
                (endTime - startTime).TotalMilliseconds,
                index.Value,
                fitness,
                genome.ToString(),
                valide,
                "[" + string.Join(", ", activities.ToArray()) + "]",
                mueP,
                mueNpv,
                sigma2P,
                sigma2Npv,
                timeFitness,
                costFitness
            );

            return solution;
        }

        private double CalculateTime(BpmnProcessAttribute bpa, BpmGene gene)
        {
            if (gene == null)
                return 0;
            if (gene is BpmnActivity)
                return DataHelper.ActivityHelper.Instance().GetTime((gene as BpmnActivity).Name);
            if (gene is BpmnAnd)
            {
                var max = 0.0;

                foreach (var child in gene.Children)
                    max = Math.Max(max, CalculateTime(bpa, child));

                return max;
            }
            if (gene is BpmnSeq)
            {
                var sum = 0.0;

                foreach (var child in gene.Children)
                    sum += CalculateTime(bpa, child);

                return sum;
            }
            if ((gene as BpmnXor).ToProcessAttribute().Equals(bpa))
                if (gene.Children == null || gene.Children.Count == 0)
                    return 0;
                else
                    return CalculateTime(bpa, gene.Children[0]);
            if (gene.Children.Count > 1)
                return CalculateTime(bpa, gene.Children[1]);
            return 0;
        }

        private static double CalculateSigma2Npv(double sigma2P)
        {
            // sigma starts at 0.0
            var sigma2Npv = 0.0;

            // discont
            for (var t = 0; t < ModelHelper.GetBpmModel().GetT(); t++)
                sigma2Npv += ModelHelper.GetBpmModel().GetN() * sigma2P /
                             Math.Pow(1 + ModelHelper.GetBpmModel().GetN(), 2 * t);

            Debug.WriteLine("sigma2Npv: " + sigma2Npv);

            return sigma2Npv;
        }

        private static double CalculateMueNpv(BpmGenome genome, double mueP)
        {
            // mue starts at 0.0
            var mueNpv = 0.0;

            // calculate variable costs for new activities in reference to the initial genome
            var listActivities = new List<BpmnActivity>();

            try
            {
                // TODO
                //listActivities =
                //TreeHelper.ListActivities(TreeHelper.ParseBpmGenome(BpmModel.Model.GetString("startProcess")));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            var currentList = genome.ListActivities();

            currentList.RemoveAll(activity => listActivities.Contains(activity));
            var countNewActivities = currentList.Count;

            // define initial costs 
            var I = ModelHelper.GetBpmModel().GetIfix() + countNewActivities * ModelHelper.GetBpmModel().GetIvar();
            Debug.WriteLine("initial costs: " + I);

            // substract initial invest
            mueNpv -= I;

            // discont
            for (var t = 0; t < ModelHelper.GetBpmModel().GetT(); t++)
                mueNpv += ModelHelper.GetBpmModel().GetN() * mueP / Math.Pow(1 + ModelHelper.GetBpmModel().GetI(), t);

            Debug.WriteLine("mueNPV: " + mueNpv);

            return mueNpv;
        }

        private static double CalculateMueP(bool isValidProcess, HashSet<BpmnActivity> activitieProbabilities)
        {
            // gehe alle beinhaltreten prozesse X durch
            // summiere dabei mueActivityX * probabilityActivityX auf
            var mueP = 0.0;

            // collect all created objects
            var soldObjects = new List<BpmnObject>();

            foreach (var entry in activitieProbabilities)
            {
                // activities mue/cashflow
                var activityMue = DataHelper.ActivityHelper.Instance().GetCashflow(entry.Name);

                // activities mue/cashflow
                double objectPiceSum = 0;
                foreach (var objectName in DataHelper.ActivityOutputHelper.Instance().ProvidedOutputObjects(entry))
                {
                    var price = DataHelper.ObjectHelper.Instance().GetOutputPrice(objectName.Name);

                    if (isValidProcess && !soldObjects.Contains(objectName))
                    {
                        // sell object
                        objectPiceSum += price;
                        soldObjects.Add(objectName);
                    }
                }

                // probability of appearance
                var activityProbability = entry.ExecutionProbability;

                mueP += (objectPiceSum + activityMue) * activityProbability;
            }

            Debug.WriteLine("mueP: " + mueP);

            return mueP;
        }

        private double CalculateSigma2P(double mueP, HashSet<BpmnActivity> activitieProbabilities,
            List<Path> paths)
        {
            var sigma2P = 0.0;
            var sum1 = 0.0;
            var sum2 = 0.0;


            sigma2P += -Math.Pow(mueP, 2);

            foreach (var entry in activitieProbabilities)
            {
                var activityVariance = DataHelper.ActivityHelper.Instance().GetVariance(entry.Name);
                var activityCashflow = DataHelper.ActivityHelper.Instance().GetCashflow(entry.Name);

                sum1 += (activityVariance + Math.Pow(activityCashflow, 2)) * entry.ExecutionProbability;
            }

            sigma2P += sum1;

            foreach (var geneA in activitieProbabilities)
            foreach (var geneB in activitieProbabilities)
                if (geneA.Index < geneB.Index)
                    //                  if (string.CompareOrdinal(geneA.ToString(), geneB.ToString()) < 0)
                {
                    var activityCashflowA = DataHelper.ActivityHelper.Instance().GetCashflow(geneA.ToString());
                    var activityCashflowB = DataHelper.ActivityHelper.Instance().GetCashflow(geneB.ToString());

                    // wahrscheinlichkeiten berechnen??
                    var probabilityGenesAb = CalculateProbabilities2ProcessActivities(geneA, geneB, paths);

                    sum2 += activityCashflowA * activityCashflowB * probabilityGenesAb;
                }

            sigma2P += 2 * sum2;

            Debug.WriteLine("sigma2P: " + sigma2P);

            return sigma2P;
        }

        private void CalculateActivityProbabilities(ref HashSet<BpmnActivity> activitieProbabilities,
            List<Path> paths)
        {
            activitieProbabilities = new HashSet<BpmnActivity>();

            foreach (var path in paths)
            foreach (var activity in path.path)
                if (activitieProbabilities.Contains(activity))
                {
                    activity.ExecutionProbability += path.Probability;
                }
                else
                {
                    activitieProbabilities.Add(activity);
                    activity.ExecutionProbability = path.Probability;
                }
        }

        /// <summary>
        ///     Checks, wheter all attributes are covered
        /// </summary>
        /// <param name="decision"></param>
        /// <param name="attributes"></param>
        /// <param name="gene"></param>
        /// <returns>false - not all attributes are covered, true - all attributes are covered</returns>
        private bool CheckActivityAttributeCovering(BpmnProcessAttribute decision, List<string> attributes,
            BpmGene gene)
        {
            if (gene == null)
                return true;

            // in case of an gateway, with does not seperate attributes
            if (gene is BpmnAnd || gene is BpmnSeq)
            {
                foreach (var child in gene.Children)
                    if (CheckActivityAttributeCovering(decision, attributes, child) == false)
                        return false;

                return gene.Children.Count > 0;
            }

            // in case of an gateway, seperating attributes (like XOR)
            if (gene is BpmnXor)
            {
                var xor = (BpmnXor) gene;
                var decisionId = xor.DecisionId;
                var decisionValue = xor.DecisionValue;

                var seperate = new List<string> {decisionValue};

                attributes.Remove(decisionValue);

                if (xor.Children == null || xor.Children.Count < 2)
                    return false;

                // if-case
                if (CheckActivityAttributeCovering(decision, seperate, xor.Children.ElementAtOrDefault(0)) == false)
                    return false;
                // else-case
                if (CheckActivityAttributeCovering(decision, attributes, xor.Children.ElementAtOrDefault(1)) == false)
                    return false;
            }

            // in case of an activity, checking if all necessary attributes are covered
            if (gene is BpmnActivity)
                foreach (var attribute in attributes)
                {
                    var activity = gene as BpmnActivity;
                    var isCovering =
                        DataHelper.CoverHelper.Instance()
                            .CheckIfActivityCoversAttribute(decision.DecisionId,
                                attribute,
                                activity.Name);
                    if (!isCovering) return false;
                }

            return true;
        }

        private bool CheckActivityAttributeCovering(BpmGene root)
        {
            var decisions = DataHelper.ActivityAttributeHelper.Instance().GetAll();

            foreach (var decision in decisions)
            {
                var decisionValues = DataHelper.ActivityAttributeHelper.Instance()
                    .GetDecisionValues(decision.DecisionId);

                if (CheckActivityAttributeCovering(decision, decisionValues, root) == false)
                    return false;
            }

            return true;
        }

        // berechnen der Wahrscheinlichkeit, das Aktivität a & b im gleichen Prozess sind über die generierten Pfade und Pfadwahrscheinlichkeiten
        private static double CalculateProbabilities2ProcessActivities(BpmGene a, BpmGene b,
            List<Path> paths)
        {
            var probability = 0.0;

            foreach (var path in paths)
                if (path.path.Contains(a) && path.path.Contains(b))
                    probability += path.Probability;

            return probability;
        }
    }
}