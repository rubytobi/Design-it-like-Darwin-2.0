using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using Bpm.Helpers;
using BpmApi.Models;

namespace BpmApi.Controllers
{
    [RoutePrefix("api/activityattributes")]
    public class ActivityAttributeController : ApiController
    {
        private readonly DataHelper.ActivityAttributeHelper _instance = DataHelper.ActivityAttributeHelper.Instance();

        [Route("")]
        [HttpGet]
        public ActivityAttributeModel[] GetActivityAttributes()
        {
            return _instance.Models.ToArray();
        }

        [Route("")]
        [HttpPost]
        public ActivityAttributeModel CreateActivityAttribute([FromBody] ActivityAttributeModel model)
        {
            _instance.Models.Add(model);
            return model;
        }

        [Route("{id:guid}")]
        [HttpGet]
        public ActivityAttributeModel GetActivityAttribute(string id)
        {
            return _instance.Models.Single(x => x.id.Equals(id));
        }

        [Route("{id:guid}")]
        [HttpDelete]
        public HttpStatusCode DeleteActivityAttribute(Guid id)
        {
            var model = _instance.Models.Single(x => x.id.Equals(id));
            var success = _instance.Models.Remove(model);

            if (success)
                return HttpStatusCode.NotFound;
            return HttpStatusCode.OK;
        }
    }
}