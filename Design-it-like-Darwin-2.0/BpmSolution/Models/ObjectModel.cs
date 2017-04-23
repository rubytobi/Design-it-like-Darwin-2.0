using System;
using Bpm.NotationElements;

namespace BpmApi.Models
{
    public class ObjectModel
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string type { get; set; }
        public bool processInput { get; set; }
        public bool processOutput { get; set; }
        public double price { get; set; }

        public BpmnObject ToBpmnObject()
        {
            return new BpmnObject(name, type, processInput, processOutput, price);
        }
    }
}