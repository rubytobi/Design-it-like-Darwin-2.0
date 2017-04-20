namespace Bpm.NotationElements.Gateways
{
    public class BpmnAnd : BpmGene
    {
        #region Constructor

        public BpmnAnd(int index, BpmGene parent) : base(index, parent)
        {
        }

        public BpmnAnd(int index, BpmGene parent, double executionProbability) : base(index, parent)
        {
            ExecutionProbability = executionProbability;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return "AND(" + string.Join(";", Children) + ")";
        }

        #endregion //Constructor
    }
}