using Bpm.NotationElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BpmApi.Models
{
    public class ActivityModel
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public double cashflow { get; set; }
        public double variance { get; set; }
        public double time { get; set; }

        public BpmnActivity ToBpmnActivity()
        {
            return new BpmnActivity(-1, null, name);
        }
    }
}