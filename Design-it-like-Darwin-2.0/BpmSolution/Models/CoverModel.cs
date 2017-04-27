using System;

namespace BpmApi.Models
{
    public class CoverModel
    {
        public Guid id { get; set; } = Guid.NewGuid();
        public string decisionId { get; set; }
        public string decisionValue { get; set; }
        public string activityName { get; set; }
        public bool isCovered { get; set; }
    }
}