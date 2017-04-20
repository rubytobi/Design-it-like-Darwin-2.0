using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BpmApi.Models
{
    public class ActivityOutputModel
    {
        public string activityName { get; set; }
        public string objectName { get; set; }
        public bool isOutput { get; set; }
    }
}