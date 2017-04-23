using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using Bpm.Helpers;
using BpmApi.Models;

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
            return DataHelper.ActivityHelper.Instance().Models.Single(x => x.name.Equals(id));
        }

        [Route("{id:guid}")]
        [HttpDelete]
        public HttpStatusCode DeleteActivity(Guid id)
        {
            var model = DataHelper.ActivityHelper.Instance().Models.Single(x => x.id.Equals(id));
            var success = DataHelper.ActivityHelper.Instance().Models.Remove(model);

            if (success)
                return HttpStatusCode.NotFound;
            return HttpStatusCode.OK;
        }
    }
}