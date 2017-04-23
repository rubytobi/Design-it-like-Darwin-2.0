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
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace BpmApi.Controllers
{
    [RoutePrefix("api/activities")]
    public class ActivityController : ApiController
    {
        [Route("")]
        [HttpGet]
        public HashSet<ActivityModel> GetActivities()
        {
            return DataHelper.ActivityHelper.Instance().GetAllModels();
        }

        [Route("")]
        [HttpPost]
        public ActivityModel CreateActivity([FromBody] ActivityModel model)
        {
            model.id = Guid.NewGuid();

            DataHelper.ActivityHelper.Instance().Add(model);
            return model;
        }

        [Route("{id:guid}")]
        [HttpGet]
        public ActivityModel GetActivity(string id)
        {
            return DataHelper.ActivityHelper.Instance().Models.Where(x => x.name.Equals(id)).FirstOrDefault();
        }

        [Route("{id:guid}")]
        [HttpDelete]
        public HttpStatusCode DeleteActivity(Guid id)
        {
            var model = DataHelper.ActivityHelper.Instance().Models.Where(x => x.id.Equals(id)).FirstOrDefault();
            var success = DataHelper.ActivityHelper.Instance().Models.Remove(model);

            if (success)
            {
                return HttpStatusCode.NotFound;
            }
            else
            {
                return HttpStatusCode.OK;
            }
        }
    }
}
