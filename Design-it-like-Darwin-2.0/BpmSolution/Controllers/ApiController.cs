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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PortableGeneticAlgorithm;
using PortableGeneticAlgorithm.GeneticAlgorithmLibrary;
using PortableGeneticAlgorithm.Predefined;
using ViewRenderEngine;

namespace BpmApi.Controllers
{
    [RoutePrefix("api")]
    public class AlgorithmController : ApiController
    {
        [HttpGet]
        [Route("evolver")]
        public JObject GetNumberOfEvolvedGenomes()
        {
            var evo = new JObject();
            evo.Add("max", ModelHelper.GetGePrModel().GetPopulationSize());
            evo.Add("current", ModelHelper.GetGePrModel().GetGenerationEvolver().i);

            return evo;
        }

        [HttpPost]
        [Route("dummydata")]
        public Status LoadDummyData()
        {
            if (GeneticAlgorithm.Instance().Status.Initialisation || GeneticAlgorithm.Instance().Status.Ready)
                DataHelper.LoadDummyData();

            return GeneticAlgorithm.Instance().Status;
        }

        [HttpGet]
        [Route("settings/bpm")]
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
                PopulationMultiplicator = model.GetPopulationMultiplicator(),
                T = model.GetT()
            };
        }

        [HttpPost]
        [Route("settings/bpm")]
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
        [Route("dataexport")]
        public HttpResponseMessage DownloadData()
        {
            var info = new BulkInfo
            {
                activities = DataHelper.ActivityHelper.Instance().GetAllModels().ToArray(),
                objects = DataHelper.ObjectHelper.Instance().GetAllModels().ToArray(),
                attributes = DataHelper.ActivityAttributeHelper.Instance().GetAllModels().ToArray(),
                covers = DataHelper.CoverHelper.Instance().GetAllModels().ToArray(),
                inputs = DataHelper.ActivityInputHelper.Instance().GetAllModels().ToArray(),
                outputs = DataHelper.ActivityOutputHelper.Instance().GetAllModels().ToArray()
            };

            var result = Request.CreateResponse(HttpStatusCode.OK);
            result.Content = new StringContent(JsonConvert.SerializeObject(info));
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "data.json"
            };

            return result;
        }

        [HttpGet]
        [Route("graphdownload")]
        public HttpResponseMessage DownloadGraphBpmn20()
        {
            var bestSolution = BpmAnalytics.Instance().BestSolution();

            var response = new HttpResponseMessage(HttpStatusCode.OK);

            if (bestSolution != null)
                response.Content = new StringContent(
                    XmlHelper.BpmnToXml(bestSolution.Process.ParseBpmGenome()).OuterXml, Encoding.UTF8,
                    "application/xml");

            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "graphXml.xml"
            };


            return response;
        }


        [HttpPost]
        [Route("dataimport")]
        public HttpStatusCode UploadData()
        {
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    var filePath = HttpContext.Current.Server.MapPath("~/" + postedFile.FileName);
                    postedFile.SaveAs(filePath);

                    var bulkinfo = JsonConvert.DeserializeObject<BulkInfo>(File.ReadAllText(filePath));
                    DataHelper.ActivityHelper.Instance().Models.AddRange(bulkinfo.activities);
                    DataHelper.ActivityInputHelper.Instance().Models.AddRange(bulkinfo.inputs);
                    DataHelper.ActivityOutputHelper.Instance().Models.AddRange(bulkinfo.outputs);
                    DataHelper.ActivityAttributeHelper.Instance().Models.AddRange(bulkinfo.attributes);
                    DataHelper.CoverHelper.Instance().Models.AddRange(bulkinfo.covers);
                }

                return HttpStatusCode.OK;
            }
            return HttpStatusCode.BadRequest;
        }

        /// <summary>
        ///     Load a bpm and genetic programming model
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("settings/algorithm")]
        public Status UpdateAlgorithmSettings([FromBody] SettingsAlgorithmModel settings)
        {
            if (!GeneticAlgorithm.Instance().Status.Initialisation)
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

        [HttpGet]
        [Route("status")]
        public HttpResponseMessage GetStatus()
        {
            var status = GeneticAlgorithm.Instance().Status;

            var accept = Request.Headers
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

            return Request.CreateResponse(HttpStatusCode.OK,
                status);
        }

        /// <summary>
        ///     Starts the Genetic Algorithm to search for a solution
        ///     GET /api/algorithm/start
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("start")]
        public Status Start()
        {
            if (!(GeneticAlgorithm.Instance().Status.Ready || GeneticAlgorithm.Instance().Status.Stopped))
                return GeneticAlgorithm.Instance().Status;

            BpmAnalytics.Instance().Clear();

            GeneticAlgorithm.StartInNewTask();

            return GeneticAlgorithm.Instance().Status;
        }

        /// <summary>
        ///     Stops the Genetic Algorithm searcing for a solution
        ///     GET /api/algorithm/stop
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("stop")]
        public Status Stop()
        {
            if (GeneticAlgorithm.Instance().Status.Running)
                GeneticAlgorithm.Instance().RequestStop();

            return GeneticAlgorithm.Instance().Status;
        }
    }
}