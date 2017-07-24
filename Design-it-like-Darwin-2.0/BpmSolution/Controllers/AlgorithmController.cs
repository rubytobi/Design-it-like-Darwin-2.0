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
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Http;
using Bpm;
using Bpm.Helpers;
using MoreLinq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PortableGeneticAlgorithm;
using PortableGeneticAlgorithm.GeneticAlgorithmLibrary;
using ViewRenderEngine;

namespace BpmApi.Controllers
{
    [RoutePrefix("api")]
    public class AlgorithmController : ApiController
    {
        [HttpGet]
        [Route("evolver/population")]
        public JObject GetNumberOfEvolvedGenomes()
        {
            var evo = new JObject();
            evo.Add("max", ModelHelper.GetGePrModel().GetPopulationSize());
            evo.Add("current", ModelHelper.GetGePrModel().GetGenerationEvolver().i);

            return evo;
        }

        [HttpGet]
        [Route("evolver/generation")]
        public JObject GetNumberOfEvolvedGenerations()
        {
            var evo = new JObject();
            evo.Add("max", ModelHelper.GetGePrModel().GetMaximumNumberOfGenerations());
            evo.Add("current", BpmAnalytics.Instance().GetNumberOfEvolvedGenerations());

            return evo;
        }

        [HttpGet]
        [Route("data/export")]
        public HttpResponseMessage DownloadData()
        {
            var info = new BulkInfo
            {
                activities = DataHelper.ActivityHelper.Instance().Models.ToArray(),
                objects = DataHelper.ObjectHelper.Instance().Models.ToArray(),
                attributes = DataHelper.ActivityAttributeHelper.Instance().Models.ToArray(),
                covers = DataHelper.CoverHelper.Instance().Models.ToArray(),
                inputs = DataHelper.ActivityInputHelper.Instance().Models.ToArray(),
                outputs = DataHelper.ActivityOutputHelper.Instance().Models.ToArray()
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

        // TODO löschen
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
        [Route("data/import")]
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

                    bulkinfo.activities.ForEach(x => DataHelper.ActivityHelper.Instance().Models.Add(x));
                    bulkinfo.objects.ForEach(x => DataHelper.ObjectHelper.Instance().Models.Add(x));
                    bulkinfo.inputs.ForEach(x => DataHelper.ActivityInputHelper.Instance().Models.Add(x));
                    bulkinfo.outputs.ForEach(x => DataHelper.ActivityOutputHelper.Instance().Models.Add(x));
                    bulkinfo.attributes.ForEach(x => DataHelper.ActivityAttributeHelper.Instance().Models.Add(x));
                    bulkinfo.covers.ForEach(x => DataHelper.CoverHelper.Instance().Models.Add(x));
                }

                return HttpStatusCode.OK;
            }
            return HttpStatusCode.BadRequest;
        }

        [HttpGet]
        [Route("status")]
        public HttpResponseMessage GetStatus()
        {
            var status = GeneticAlgorithm.Instance().Status;

            if (status.Ready && DataHelper.ActivityHelper.Instance().Models.Count <= 0)
            {
                status = new Status();

                status.Initialisation = true;
                status.Ready = false;
            }

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
            if ((GeneticAlgorithm.Instance().Status.Ready || GeneticAlgorithm.Instance().Status.Stopped)
                && DataHelper.ActivityHelper.Instance().Models.Count > 0)
            {
                BpmAnalytics.Instance().Clear();

                GeneticAlgorithm.StartInNewTask();

                return GeneticAlgorithm.Instance().Status;
            }

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