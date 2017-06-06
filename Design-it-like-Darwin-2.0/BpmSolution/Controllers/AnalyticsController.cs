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
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Bpm;
using Bpm.Helpers;
using Newtonsoft.Json.Linq;
using ViewRenderEngine;

namespace BpmApi.Controllers
{
    [RoutePrefix("api/analytics")]
    public class AnalyticsController : ApiController
    {
        [HttpGet]
        [Route("solution/{id:guid}")]
        public HttpResponseMessage GetSolutionViewer([FromUri] Guid id)
        {
            var solution = BpmAnalytics.Instance().Get(id) as BpmSolution;

            var accept = Request.Headers
                .GetValues("Accept")
                .FirstOrDefault();

            if (!string.IsNullOrEmpty(accept) &&
                accept.ToLower().Contains("text/html"))
            {
                var html = ViewRenderer
                    .RenderPartialView(
                        "~/views/partialviews/SolutionViewer.cshtml",
                        id
                    );
                var message = new HttpResponseMessage(HttpStatusCode.OK);
                message.Content = new StringContent(html, Encoding.UTF8,
                    "text/html");
                return message;
            }

            return Request.CreateResponse(HttpStatusCode.OK,
                solution);
        }

        // GET api/Analytics/BestSolution
        [HttpGet]
        [Route("bestsolution")]
        public HttpResponseMessage GetBestSolution()
        {
            var bestSolution = BpmAnalytics.Instance().BestSolution();

            var response = new HttpResponseMessage(HttpStatusCode.OK);

            var accept = Request.Headers
                .GetValues("Accept")
                .FirstOrDefault();


            if (!string.IsNullOrEmpty(accept) &&
                accept.ToLower().Contains("application/xml")
                && bestSolution != null)
            {
                response.Content = new StringContent(
                    XmlHelper.BpmnToXml(bestSolution.Process.ParseBpmGenome()).OuterXml, Encoding.UTF8,
                    "application/xml");
                return response;
            }
            return Request.CreateResponse(HttpStatusCode.OK,
                bestSolution);
        }

        [HttpGet]
        [Route("fitness/{generation:int}")]
        public JObject Fitness([FromUri] int generation)
        {
            var minArray = BpmAnalytics.Instance().MinFitness();
            minArray = minArray.GetLast(generation);

            var avgArray = BpmAnalytics.Instance().AvgFitness();
            avgArray = avgArray.GetLast(generation);

            var maxArray = BpmAnalytics.Instance().MaxFitness();
            maxArray = maxArray.GetLast(generation);

            var jObject = new JObject();
            jObject.Add("min", new JArray(minArray));
            jObject.Add("avg", new JArray(avgArray));
            jObject.Add("max", new JArray(maxArray));

            return jObject;
        }

        [HttpGet]
        [Route("fitnessValidOnly/{generation:int}")]
        public JObject FitnessValidOnly([FromUri] int generation)
        {
            var minArray = BpmAnalytics.Instance().MinFitnessValidOnly();
            minArray = minArray.GetLast(generation);

            var avgArray = BpmAnalytics.Instance().AvgFitnessValidOnly();
            avgArray = avgArray.GetLast(generation);

            var maxArray = BpmAnalytics.Instance().MaxFitnessValidOnly();
            maxArray = maxArray.GetLast(generation);

            var jObject = new JObject();
            jObject.Add("min", new JArray(minArray));
            jObject.Add("avg", new JArray(avgArray));
            jObject.Add("max", new JArray(maxArray));

            return jObject;
        }

        [HttpGet]
        [Route("runtime/{generation:int}")]
        public JObject Runtime([FromUri] int generation)
        {
            var minArray = BpmAnalytics.Instance().MinRuntime();
            minArray = minArray.GetLast(generation);

            var avgArray = BpmAnalytics.Instance().AvgRuntime();
            avgArray = avgArray.GetLast(generation);

            var maxArray = BpmAnalytics.Instance().MaxRuntime();
            maxArray = maxArray.GetLast(generation);

            var jObject = new JObject();
            jObject.Add("min", new JArray(minArray));
            jObject.Add("avg", new JArray(avgArray));
            jObject.Add("max", new JArray(maxArray));

            return jObject;
        }

        [HttpGet]
        [Route("validgenomes/{generation:int}")]
        public JArray ValidGenomes([FromUri] int generation)
        {
            var array = BpmAnalytics.Instance().ValidGenomes();
            array = array.GetLast(generation);

            return new JArray(array);
        }

        [HttpGet]
        [Route("")]
        public IEnumerable<BpmSolution> All()
        {
            return BpmAnalytics.Instance().GetAll();
        }

        // GET api/analytics/bestSolution
        [HttpGet]
        [Route("topsolutions")]
        public IEnumerable<BpmSolution> TopSolutions()
        {
            return BpmAnalytics.Instance().GetTopSolutions(10);
        }
    }
}