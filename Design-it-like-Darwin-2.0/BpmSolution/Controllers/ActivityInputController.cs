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
    [RoutePrefix("api/activityinputs")]
    public class ActivityInputController : ApiController
    {
        private readonly DataHelper.ActivityInputHelper _instance = DataHelper.ActivityInputHelper.Instance();

        [Route("")]
        [HttpPost]
        public void CreateActivityInputs([FromBody] InputModel[] inputs)
        {
            inputs.ForEach(x => _instance.Models.Add(x));
        }

        [Route("")]
        [HttpGet]
        public InputModel[] GetActivityInputs()
        {
            return _instance.Models.ToArray();
        }

        [Route("{id:guid}")]
        [HttpDelete]
        public HttpStatusCode DeleteActivityInputs([FromUri] Guid id)
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