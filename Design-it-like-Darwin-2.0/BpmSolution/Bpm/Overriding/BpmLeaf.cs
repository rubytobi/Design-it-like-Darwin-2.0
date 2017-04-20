using System.Collections.Generic;

namespace Bpm
{
    /// <summary>
    ///     BpmLeaf is a BpmGene with Children = null
    /// </summary>
    public class BpmLeaf : BpmGene
    {
        private readonly List<BpmGene> _children;

        #region Constructor

        public BpmLeaf(int index, BpmGene parent) : base(index, parent)
        {
            _children = null;
        }

        #endregion //Constructor

        public override List<BpmGene> Children => _children;
    }
}