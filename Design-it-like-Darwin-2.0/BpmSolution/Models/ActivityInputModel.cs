using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BpmApi.Models
{
    public class ActivityInputModel
    {
        public string activityName { get; set; }
        public string objectName { get; set; }
        public bool isInput { get; set; }
    }
}