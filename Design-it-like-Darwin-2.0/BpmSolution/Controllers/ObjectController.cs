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
        private DataHelper.ObjectHelper _instance = DataHelper.ObjectHelper.Instance();

        [Route("")]
        [HttpGet]
        public HashSet<ObjectModel> GetObjects()
        {
            return DataHelper.ObjectHelper.Instance().GetAllModels();
        }

        [Route("")]
        [HttpPost]
        public ObjectModel CreateObject([FromBody] ObjectModel model)
        {
            model.id = Guid.NewGuid();

            DataHelper.ObjectHelper.Instance().Add(model);
            return model;
        }

        [Route("{id:guid}")]
        [HttpGet]
        public ObjectModel GetObject(Guid id)
        {
            return DataHelper.ObjectHelper.Instance().GetAllModels().Where(x => x.id.Equals(id)).FirstOrDefault();
        }

        [Route("{id:guid}")]
        [HttpDelete]
        public HttpStatusCode DeleteObject(Guid id)
        {
            var before = DataHelper.ObjectHelper.Instance().GetAllModels().Count;

            DataHelper.ObjectHelper.Instance().GetAllModels().RemoveWhere(x => x.id.Equals(id));

            var after = DataHelper.ObjectHelper.Instance().GetAllModels().Count;

            if (before == after)
                return HttpStatusCode.NotFound;
            return HttpStatusCode.OK;
        }
    }
}