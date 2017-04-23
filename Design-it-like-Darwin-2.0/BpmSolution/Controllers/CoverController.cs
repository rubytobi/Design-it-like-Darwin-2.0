﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Http;
using Bpm.Helpers;
using BpmApi.Models;

namespace WebApi.Controllers
{
    public class CoverController : ApiController
    {
        private readonly DataHelper.CoverHelper _instance = DataHelper.CoverHelper.Instance();

        // TODO POST api/activityInput
        public void Post([FromBody] List<CoverModel> activityOutput)
        {
            Debug.WriteLine("POST api/activityInput");

            foreach (var a in activityOutput)
                _instance.Add(a);
        }
    }
}