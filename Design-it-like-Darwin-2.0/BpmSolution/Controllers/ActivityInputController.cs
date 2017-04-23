using System.Collections.Generic;
using System.Web.Http;
using Bpm.Helpers;
using BpmApi.Models;

namespace WebApi.Controllers
{
    [RoutePrefix("api/activityinputs")]
    public class ActivityInputController : ApiController
    {
        private readonly DataHelper.ActivityInputHelper _instance = DataHelper.ActivityInputHelper.Instance();

        [Route("")]
        [HttpPost]
        public void CreateActivityInputs([FromBody] ActivityInputModel[] activityInputs)
        {
            _instance.Models.AddRange(activityInputs);
        }

        [Route("")]
        [HttpGet]
        public IEnumerable<ActivityInputModel> GetActivityInputs()
        {
            return _instance.GetAll();
        }
    }
}