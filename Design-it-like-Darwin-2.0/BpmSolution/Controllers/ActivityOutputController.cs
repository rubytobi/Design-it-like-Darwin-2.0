using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Http;
using Bpm.Helpers;
using BpmApi.Models;

namespace WebApi.Controllers
{
    public class ActivityOutputController : ApiController
    {
        private readonly DataHelper.ActivityOutputHelper _instance = DataHelper.ActivityOutputHelper.Instance();

        // TODO POST api/activityInput
        public void Post([FromBody] List<ActivityOutputModel> activityOutput)
        {
            Debug.WriteLine("POST api/activityInput");

            foreach (var a in activityOutput)
                _instance.Add(a);
        }
    }
}