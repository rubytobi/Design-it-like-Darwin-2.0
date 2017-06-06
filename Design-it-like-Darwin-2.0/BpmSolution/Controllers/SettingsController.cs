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
using System.Web.Http;
using Bpm;
using Bpm.Fitnesses;
using Bpm.Helpers;
using BpmApi.Models;
using PortableGeneticAlgorithm;
using PortableGeneticAlgorithm.GeneticAlgorithmLibrary;
using PortableGeneticAlgorithm.Predefined;

namespace BpmApi.Controllers
{
    [RoutePrefix("api/settings")]
    public class SettingsController : ApiController
    {
        [HttpGet]
        [Route("bpm/default")]
        public SettingsBpmModel GetDefaultBpmSettings()
        {
            var model = ModelHelper.GetDefaultBpmModel();

            return new SettingsBpmModel
            {
                TimeWeight = model.GetTimeWeight(),
                CostWeight = model.GetCostWeight(),
                Alpha = model.GetAlpha(),
                I = model.GetI(),
                Ifix = model.GetIfix(),
                Ivar = model.GetIvar(),
                MaxDepthRandomGenome = model.GetMaxDepthRandomGenome(),
                N = model.GetN(),
                OnlyValidSolutions = model.GetOnlyValidSolutions(),
                OnlyValidSolutionsAtStart = model.GetOnlyValidSolutionsAtStart(),
                PainFactor = model.GetPainFactor(),
                T = model.GetT()
            };
        }

        [HttpGet]
        [Route("bpm")]
        public SettingsBpmModel GetBpmSettings()
        {
            var model = ModelHelper.GetBpmModel();

            return new SettingsBpmModel
            {
                TimeWeight = model.GetTimeWeight(),
                CostWeight = model.GetCostWeight(),
                Alpha = model.GetAlpha(),
                I = model.GetI(),
                Ifix = model.GetIfix(),
                Ivar = model.GetIvar(),
                MaxDepthRandomGenome = model.GetMaxDepthRandomGenome(),
                N = model.GetN(),
                OnlyValidSolutions = model.GetOnlyValidSolutions(),
                OnlyValidSolutionsAtStart = model.GetOnlyValidSolutionsAtStart(),
                PainFactor = model.GetPainFactor(),
                T = model.GetT()
            };
        }

        [HttpPost]
        [Route("bpm")]
        public Status UpdateBpmSettings([FromBody] SettingsBpmModel settings)
        {
            if (!GeneticAlgorithm.Instance().Status.Initialisation && !GeneticAlgorithm.Instance().Status.Ready)
                return GeneticAlgorithm.Instance().Status;

            // basic bpmn model settings
            var bpmModelBuilder = new BpmModel.Builder()
                .SetAlpha(settings.Alpha)
                .SetI(settings.I)
                .SetIfix(settings.Ifix)
                .SetIvar(settings.Ivar)
                .SetMaxDepthRandomGenome(settings.MaxDepthRandomGenome)
                .SetN(settings.N)
                .SetOnlyValidSolutions(settings.OnlyValidSolutions)
                .SetOnlyValidSolutionsAtStart(settings.OnlyValidSolutionsAtStart)
                .SetPainFactor(settings.PainFactor)
                .SetT(settings.T)
                .SetCostWeight(settings.CostWeight)
                .SetTimeWeight(settings.TimeWeight);

            // load specific settings provided by user
            ModelHelper.SetBpmModel(bpmModelBuilder.Build());

            return GeneticAlgorithm.Instance().Status;
        }


        [HttpGet]
        [Route("algorithm")]
        public SettingsAlgorithmModel GetAlgorithmSettings()
        {
            var model = ModelHelper.GetGePrModel();

            return new SettingsAlgorithmModel
            {
                CrossoverProbability = model.GetCrossoverProbability(),
                InitialGenome = model.GetInitialGenomeString(),
                MaximumNumberOfGenerations = model.GetMaximumNumberOfGenerations(),
                MutationProbability = model.GetMutationProbability(),
                PopulationSize = model.GetPopulationSize(),
                Seed = model.GetSeed(),
                TournamentSize = model.GetTournamentSize()
            };
        }

        [HttpGet]
        [Route("algorithm/default")]
        public SettingsAlgorithmModel GetDefaultAlgorithmSettings()
        {
            var model = ModelHelper.GetDefaultGePrModel();

            return new SettingsAlgorithmModel
            {
                CrossoverProbability = model.GetCrossoverProbability(),
                InitialGenome = model.GetInitialGenomeString(),
                MaximumNumberOfGenerations = model.GetMaximumNumberOfGenerations(),
                MutationProbability = model.GetMutationProbability(),
                PopulationSize = model.GetPopulationSize(),
                Seed = model.GetSeed(),
                TournamentSize = model.GetTournamentSize()
            };
        }

        /// <summary>
        ///     Load a bpm and genetic programming model
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("algorithm")]
        public Status UpdateAlgorithmSettings([FromBody] SettingsAlgorithmModel settings)
        {
            if (!GeneticAlgorithm.Instance().Status.Initialisation && !GeneticAlgorithm.Instance().Status.Ready)
                return GeneticAlgorithm.Instance().Status;

            // basic algorithm settings
            var model = new GePrModel.Builder()
                .SetAdditionalAnalytics(BpmAnalytics.Instance())
                .SetEnableAnalytics(true)
                .SetFitness(typeof(BpmnFitness))
                .SetGenerationEvolver(new BpmnGenerationEvolver())
                .SetSolutionType(typeof(BpmSolution))
                .SetUseParalell(true)
                .SetCrossoverProbability(settings.CrossoverProbability)
                .SetInitialGenome(settings.ToIGenome())
                .SetMaximumNumberOfGenerations(settings.MaximumNumberOfGenerations)
                .SetMutationProbability(settings.MutationProbability)
                .SetPopulationSize(settings.PopulationSize)
                .SetSeed(settings.Seed)
                .SetTermination(new IterationTermination(settings.MaximumNumberOfGenerations))
                .SetTournamentSize(settings.TournamentSize)
                .Build();

            ModelHelper.SetGePrModel(model);

            GeneticAlgorithm.Instance().SetModel(model);

            return GeneticAlgorithm.Instance().Status;
        }
    }
}