using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Http;
using Bpm;
using Bpm.Fitnesses;
using Bpm.Helpers;
using BpmApi.Models;
using MoreLinq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PortableGeneticAlgorithm;
using PortableGeneticAlgorithm.GeneticAlgorithmLibrary;
using PortableGeneticAlgorithm.Predefined;
using ViewRenderEngine;

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
            if (!GeneticAlgorithm.Instance().Status.Initialisation)
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
                .SetT(settings.T);

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