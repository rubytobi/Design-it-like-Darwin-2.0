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
    [RoutePrefix("api/activityattribute")]
    public class ActivityAttributeController : ApiController
    {
        private DataHelper.ActivityHelper _instance = DataHelper.ActivityHelper.Instance();

        [Route("")]
        [HttpPost]
        public void CreateActivityAttribute([FromBody]List<ActivityModel> activityAttribute)
        {
            foreach (var x in activityAttribute)
            {
                _instance.Add(x);
            }
        }

        [Route("")]
        [HttpGet]
        public IEnumerable<ActivityModel> GetAllActivityAttributes()
        {
            return _instance.GetAllModels();
        }
    }
}
