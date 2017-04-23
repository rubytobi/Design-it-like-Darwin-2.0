using System.Collections.Generic;
using System.Web.Http;
using Bpm.Helpers;
using BpmApi.Models;

namespace WebApi.Controllers
{
    [RoutePrefix("activityinputs")]
    public class ActivityInputController : ApiController
    {
        private readonly DataHelper.ActivityInputHelper _instance = DataHelper.ActivityInputHelper.Instance();

        [Route("")]
        [HttpPost]
        public void CreateActivityInput([FromBody] ActivityInputModel activityInput)
        {
            _instance.Add(activityInput);
        }

        [Route("")]
        [HttpGet]
        public IEnumerable<ActivityInputModel> GetActivityInputs()
        {
            return _instance.GetAll();
        }
    }
}