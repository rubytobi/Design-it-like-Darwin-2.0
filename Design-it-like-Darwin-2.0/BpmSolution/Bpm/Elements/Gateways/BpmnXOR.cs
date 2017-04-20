namespace Bpm.NotationElements.Gateways
{
    /// <summary>
    ///     BPMN-Gateway: XOR
    /// </summary>
    public class BpmnXor : BpmGene
    {
        /// <summary>
        ///     Returns the decision info, a processattribute
        /// </summary>
        /// <returns></returns>
        public BpmnProcessAttribute ToProcessAttribute()
        {
            return new BpmnProcessAttribute(DecisionId, DecisionValue, ExecutionProbability);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is BpmnXor && Id.Equals(((BpmnXor) obj).Id))
                return true;
            return false;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        #region Constructor

        /// <summary>
        ///     Creates an BpmnXor
        /// </summary>
        public BpmnXor(int index, BpmGene parent) : base(index, parent)
        {
        }

        /// <summary>
        ///     Creates a new BPMN-XOR-Gateway
        /// </summary>
        /// <param name="index">i dof node</param>
        /// <param name="parent">parent node</param>
        /// <param name="decisionId">id of decision</param>
        /// <param name="decisionValue">value of decision</param>
        /// <param name="executionProbability">probability of execution</param>
        public BpmnXor(int index, BpmGene parent, string decisionId, string decisionValue,
            double executionProbability = 1.0) : base(index, parent)
        {
            DecisionId = decisionId;
            DecisionValue = decisionValue;
            ExecutionProbability = executionProbability;
        }

        /// <summary>
        ///     id of decision
        /// </summary>
        public string DecisionId { get; set; }

        /// <summary>
        ///     value of decision
        /// </summary>
        public string DecisionValue { get; set; }

        /// <summary>
        ///     Creates a string representation of this node
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            return "XOR(v[" + DecisionId + "," + DecisionValue + "];" + string.Join(";", Children) + ")";
        }

        #endregion //Constructor
    }
}