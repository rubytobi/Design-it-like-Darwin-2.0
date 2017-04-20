namespace Bpm.NotationElements.Gateways
{
    public class BpmnSeq : BpmGene
    {
        #region Constructor

        public BpmnSeq(int index, BpmGene parent) : base(index, parent)
        {
        }

        public BpmnSeq(int index, BpmGene parent, double executionProbability) : base(index, parent)
        {
            ExecutionProbability = executionProbability;
        }

        public override string ToString()
        {
            return "SEQ(" + string.Join(";", Children) + ")";
        }

        #endregion //Constructor
    }
}