using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using Bpm.Helpers;
using BpmApi.Models;
using MoreLinq;

namespace WebApi.Controllers
{
    [RoutePrefix("api/activityoutputs")]
    public class ActivityOutputController : ApiController
    {
        private readonly DataHelper.ActivityOutputHelper _instance = DataHelper.ActivityOutputHelper.Instance();

        [Route("")]
        [HttpPost]
        public void CreateActivityOutputs([FromBody] OutputModel[] outputs)
        {
            outputs.ForEach(x => _instance.Models.Add(x));
        }

        [Route("")]
        [HttpGet]
        public OutputModel[] GetActivityOutput()
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