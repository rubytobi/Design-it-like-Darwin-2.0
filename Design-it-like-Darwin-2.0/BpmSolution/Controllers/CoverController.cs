using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using Bpm.Helpers;
using BpmApi.Models;

namespace WebApi.Controllers
{
    [RoutePrefix("api/covers")]
    public class CoverController : ApiController
    {
        private readonly DataHelper.CoverHelper _instance = DataHelper.CoverHelper.Instance();

        [Route("")]
        [HttpPost]
        public void CreateCovers([FromBody] CoverModel[] models)
        {
            _instance.Models.AddRange(models);
        }

        [Route("")]
        [HttpGet]
        public CoverModel[] GetCovers()
        {
            return _instance.Models.ToArray();
        }

        [Route("{id:guid}")]
        [HttpDelete]
        public HttpStatusCode DeleteCover([FromUri] Guid id)
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