using System;
using Bpm.NotationElements;

namespace BpmApi.Models
{
    public class ActivityModel
    {
        public Guid id { get; set; } = Guid.NewGuid();
        public string name { get; set; }
        public string description { get; set; }
        public double cashflow { get; set; }
        public double variance { get; set; }
        public double duration { get; set; }

        public BpmnActivity ToBpmnActivity()
        {
            return new BpmnActivity(-1, null, name);
        }
    }
}