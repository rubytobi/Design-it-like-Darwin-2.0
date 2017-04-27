using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using Bpm.Helpers;
using BpmApi.Models;

namespace WebApi.Controllers
{
    [RoutePrefix("api/objects")]
    public class ObjectAttributeController : ApiController
    {
        private readonly DataHelper.ObjectHelper _instance = DataHelper.ObjectHelper.Instance();

        [Route("")]
        [HttpGet]
        public ObjectModel[] GetObjects()
        {
            return _instance.Models.ToArray();
        }

        [Route("")]
        [HttpPost]
        public ObjectModel CreateObject([FromBody] ObjectModel model)
        {
            _instance.Models.Add(model);
            return model;
        }

        [Route("{id:guid}")]
        [HttpGet]
        public ObjectModel GetObject(Guid id)
        {
            return _instance.Models.Single(x => x.id.Equals(id));
        }

        [Route("{id:guid}")]
        [HttpDelete]
        public HttpStatusCode DeleteObject(Guid id)
        {
            var model = _instance.Models.Single(x => x.id.Equals(id));
            var success = _instance.Models.Remove(model);

            if (success)
                return HttpStatusCode.NotFound;
            return HttpStatusCode.OK;
        }
    }
}