﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BpmApi.Models
{
    public class CoverModel
    {
        public string decisionId { get; set; }
        public string decisionValue { get; set; }
        public string activityName { get; set; }
        public bool isCovered { get; set; }
    }
}