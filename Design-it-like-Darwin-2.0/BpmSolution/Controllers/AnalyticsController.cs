using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using Bpm;
using Bpm.Helpers;

namespace BpmApi.Controllers
{
    [RoutePrefix("api/analytics")]
    public class AnalyticsController : ApiController
    {
        // GET api/Analytics/BestSolution
        [HttpGet]
        [Route("bestsolution")]
        public HttpResponseMessage GetBestSolution()
        {
            BpmSolution bestSolution = BpmAnalytics.Instance().BestSolution();

            var response = new HttpResponseMessage(HttpStatusCode.OK);

            string accept = Request.Headers
                                    .GetValues("Accept")
                                    .FirstOrDefault();


            if (!string.IsNullOrEmpty(accept) &&
                accept.ToLower().Contains("application/xml")
                && bestSolution != null)
            {
                // TODO
                //response.Content = new StringContent(XmlHelper.Dummy().OuterXml, Encoding.UTF8, "application/xml");
                response.Content = new StringContent(XmlHelper.BpmnToXml(TreeHelper.ParseBpmGenome(bestSolution.Process)).OuterXml, Encoding.UTF8, "application/xml");
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK,
                                                    bestSolution);
            }
        }

        [HttpGet]
        [Route("fitness/{generation:int}")]
        public JObject Fitness([FromUri]int generation)
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
        [Route("runtime/{generation:int}")]
        public JObject Runtime([FromUri]int generation)
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
        public JArray ValidGenomes([FromUri]int generation)
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