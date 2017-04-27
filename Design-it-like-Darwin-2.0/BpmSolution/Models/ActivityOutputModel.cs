using System;

namespace BpmApi.Models
{
    public class ActivityOutputModel
    {
        public Guid id { get; set; } = Guid.NewGuid();
        public string activityName { get; set; }
        public string objectName { get; set; }
        public bool isOutput { get; set; }
    }
}