namespace Bpm.NotationElements
{
    public class BpmnProcessAttribute
    {
        public BpmnProcessAttribute(string id, string value, double probability)
        {
            DecisionId = id;
            DecisionValue = value;
            DecisionProbability = probability;
        }

        public string DecisionId { get; }
        public string DecisionValue { get; }
        public double DecisionProbability { get; }

        public override bool Equals(object obj)
        {
            if (obj is BpmnProcessAttribute)
                return ToString().Equals(obj.ToString());
            return false;
        }

        public override string ToString()
        {
            return DecisionId + "-" + DecisionValue;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}