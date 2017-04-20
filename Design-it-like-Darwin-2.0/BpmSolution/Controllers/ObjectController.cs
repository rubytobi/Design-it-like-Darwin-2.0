using Bpm.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
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
            int before = DataHelper.ObjectHelper.Instance().GetAllModels().Count;

            DataHelper.ObjectHelper.Instance().GetAllModels().RemoveWhere(x => x.id.Equals(id));

            int after = DataHelper.ObjectHelper.Instance().GetAllModels().Count;

            if (before == after)
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
