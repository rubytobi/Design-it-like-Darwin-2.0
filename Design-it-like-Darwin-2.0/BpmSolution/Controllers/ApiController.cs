using Bpm;
using Bpm.Fitnesses;
using Bpm.Helpers;
using PortableGeneticAlgorithm;
using PortableGeneticAlgorithm.GeneticAlgorithmLibrary;
using PortableGeneticAlgorithm.Predefined;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using BpmApi.Models;
using ViewRenderEngine;
using System.Net;
using System.Text;
using System;
using Newtonsoft.Json.Linq;

namespace BpmApi.Controllers
{
    [RoutePrefix("api")]
    public class AlgorithmController : ApiController
    {
        [HttpGet]
        [Route("evolver")]
        public JObject GetNumberOfEvolvedGenomes()
        {
            JObject evo = new JObject();
            evo.Add("max", ModelHelper.GetGePrModel().GetPopulationSize());
            evo.Add("current", ModelHelper.GetGePrModel().GetGenerationEvolver().i);

            return evo;
        }

        [HttpPost]
        [Route("dummydata")]
        public Status LoadDummyData()
        {
            if (GeneticAlgorithm.Instance().Status.Initialisation || GeneticAlgorithm.Instance().Status.Ready)
            {
                DataHelper.LoadDummyData();
            }

            return GeneticAlgorithm.Instance().Status;
        }

        [HttpGet]
        [Route("settings/bpm")]
        public SettingsBpmModel GetBpmSettings()
        {
            BpmModel model = ModelHelper.GetBpmModel();

            return new SettingsBpmModel()
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
                PopulationMultiplicator = model.GetPopulationMultiplicator(),
                T = model.GetT()
            };
        }

        [HttpPost]
        [Route("settings/bpm")]
        public Status UpdateBpmSettings([FromBody] SettingsBpmModel settings)
        {
            if (!GeneticAlgorithm.Instance().Status.Initialisation)
            {
                return GeneticAlgorithm.Instance().Status;
            }

            // basic bpmn model settings
            BpmModel.Builder bpmModelBuilder = new BpmModel.Builder()
            .SetAlpha(settings.Alpha)
            .SetI(settings.I)
            .SetIfix(settings.Ifix)
            .SetIvar(settings.Ivar)
            .SetMaxDepthRandomGenome(settings.MaxDepthRandomGenome)
            .SetN(settings.N)
            .SetOnlyValidSolutions(settings.OnlyValidSolutions)
            .SetOnlyValidSolutionsAtStart(settings.OnlyValidSolutionsAtStart)
            .SetPainFactor(settings.PainFactor)
            .SetPopulationMultiplicator(settings.PopulationMultiplicator)
            .SetT(settings.T);

            // load specific settings provided by user
            ModelHelper.SetBpmModel(bpmModelBuilder.Build());

            return GeneticAlgorithm.Instance().Status;
        }


        [HttpGet]
        [Route("settings/algorithm")]
        public SettingsAlgorithmModel GetAlgorithmSettings()
        {
            GePrModel model = ModelHelper.GetGePrModel();

            return new SettingsAlgorithmModel()
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
        /// Load a bpm and genetic programming model
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("settings/algorithm")]
        public Status UpdateAlgorithmSettings([FromBody]SettingsAlgorithmModel settings)
        {
            if (!GeneticAlgorithm.Instance().Status.Initialisation)
            {
                return GeneticAlgorithm.Instance().Status;
            }

            // basic algorithm settings
            GePrModel model = new GePrModel.Builder()
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

        [HttpGet]
        [Route("status")]
        public HttpResponseMessage GetStatus()
        {
            var status = GeneticAlgorithm.Instance().Status;

            string accept = Request.Headers
                                    .GetValues("Accept")
                                    .FirstOrDefault();

            if (!string.IsNullOrEmpty(accept) &&
                accept.ToLower().Contains("text/html"))
            {
                var html = ViewRenderer
                            .RenderPartialView(
                                  "~/views/partialviews/StatusBar.cshtml",
                                  status);
                var message = new HttpResponseMessage(HttpStatusCode.OK);
                message.Content = new StringContent(html, Encoding.UTF8,
                                                    "text/html");
                return message;
            }

            return Request.CreateResponse<Status>(HttpStatusCode.OK,
                                                    status);
        }

        /// <summary>
        /// Starts the Genetic Algorithm to search for a solution
        /// GET /api/algorithm/start
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("start")]
        public Status Start()
        {
            if (!(GeneticAlgorithm.Instance().Status.Ready || GeneticAlgorithm.Instance().Status.Stopped))
            {
                return GeneticAlgorithm.Instance().Status;
            }

            BpmAnalytics.Instance().Clear();

            GeneticAlgorithm.StartInNewTask();

            return GeneticAlgorithm.Instance().Status;
        }

        /// <summary>
        /// Stops the Genetic Algorithm searcing for a solution
        /// GET /api/algorithm/stop
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("stop")]
        public Status Stop()
        {
            if (GeneticAlgorithm.Instance().Status.Running)
            {
                GeneticAlgorithm.Instance().RequestStop();
            }

            return GeneticAlgorithm.Instance().Status;
        }
    }
}
