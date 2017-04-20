namespace Bpm.NotationElements
{
    public class BpmnActivity : BpmLeaf
    {
        #region Properties

        public string Name { get; set; }

        #endregion //Properties

        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override bool Equals(object o)
        {
            if (o == null)
                return false;

            if (o is BpmnActivity)
                return Id.Equals(((BpmnActivity) o).Id);
            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        #region Constructor

        public BpmnActivity(int index, BpmGene parent, string name) : base(index, parent)
        {
            Name = name;
        }

        // TODO: Get rid of code duplication
        public BpmnActivity(int index, BpmGene parent, string name, double executionProbability) : base(index, parent)
        {
            Name = name;
            ExecutionProbability = executionProbability;
        }

        #endregion //Constructor
    }
}