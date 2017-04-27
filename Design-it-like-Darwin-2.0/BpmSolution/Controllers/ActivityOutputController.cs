using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using Bpm.Helpers;
using BpmApi.Models;

namespace WebApi.Controllers
{
    [RoutePrefix("api/activityoutputs")]
    public class ActivityOutputController : ApiController
    {
        private readonly DataHelper.ActivityOutputHelper _instance = DataHelper.ActivityOutputHelper.Instance();

        [Route("")]
        [HttpPost]
        public void CreateActivityOutputs([FromBody] ActivityOutputModel[] activityOutputs)
        {
            _instance.Models.AddRange(activityOutputs);
        }

        [Route("")]
        [HttpGet]
        public ActivityOutputModel[] GetActivityOutput()
        {
            return _instance.Models.ToArray();
        }

        [Route("{id:guid}")]
        [HttpDelete]
        public HttpStatusCode DeleteActivityOutput([FromUri] Guid id)
        {
            var single = _instance.Models.Single(x => x.id.Equals(id));

            if (single == null)
            {
                return HttpStatusCode.BadRequest;
            }

            _instance.Models.Remove(single);
            return HttpStatusCode.OK;
        }
    }
}