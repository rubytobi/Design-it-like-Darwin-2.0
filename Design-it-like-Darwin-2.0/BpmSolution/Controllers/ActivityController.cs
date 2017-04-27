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
        private DataHelper.ActivityHelper _instance = DataHelper.ActivityHelper.Instance();

        [Route("")]
        [HttpGet]
        public ActivityModel[] GetActivities()
        {
            return _instance.Models.ToArray();
        }

        [Route("")]
        [HttpPost]
        public ActivityModel CreateActivity([FromBody] ActivityModel model)
        {
            _instance.Models.Add(model);
            return model;
        }

        [Route("{id:guid}")]
        [HttpGet]
        public ActivityModel GetActivity(string id)
        {
            return _instance.Models.Single(x => x.name.Equals(id));
        }

        [Route("{id:guid}")]
        [HttpDelete]
        public HttpStatusCode DeleteActivity(Guid id)
        {
            var model = _instance.Models.Single(x => x.id.Equals(id));
            var success = _instance.Models.Remove(model);

            if (success)
                return HttpStatusCode.NotFound;
            return HttpStatusCode.OK;
        }
    }
}