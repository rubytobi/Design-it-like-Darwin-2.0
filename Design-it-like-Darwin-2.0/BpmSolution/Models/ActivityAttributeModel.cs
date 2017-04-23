using Bpm.NotationElements;

namespace BpmApi.Models
{
    public class ActivityAttributeModel
    {
        public string decisionId { get; set; }
        public string decisionValue { get; set; }
        public double decisionProbability { get; set; }

        public BpmnProcessAttribute ToBpmnProcessAttribute()
        {
            return new BpmnProcessAttribute(decisionId, decisionValue, decisionProbability);
        }
    }
}