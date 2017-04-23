using System.Collections.Generic;
using System.Web.Http;
using Bpm.Helpers;
using BpmApi.Models;

namespace WebApi.Controllers
{
    [RoutePrefix("api/activityattribute")]
    public class ActivityAttributeController : ApiController
    {
        private readonly DataHelper.ActivityHelper _instance = DataHelper.ActivityHelper.Instance();

        [Route("")]
        [HttpPost]
        public void CreateActivityAttribute([FromBody] List<ActivityModel> activityAttribute)
        {
            foreach (var x in activityAttribute)
                _instance.Add(x);
        }

        [Route("")]
        [HttpGet]
        public IEnumerable<ActivityModel> GetAllActivityAttributes()
        {
            return _instance.GetAllModels();
        }
    }
}