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
    [RoutePrefix("activityinputs")]
    public class ActivityInputController : ApiController
    {
        private DataHelper.ActivityInputHelper _instance = DataHelper.ActivityInputHelper.Instance();

        [Route("")]
        [HttpPost]
        public void CreateActivityInput([FromBody]ActivityInputModel activityInput)
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
