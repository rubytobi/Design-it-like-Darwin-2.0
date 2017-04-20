using System;
using System.Collections.Generic;
using Bpm.NotationElements;

namespace Bpm
{
    /// <summary>
    ///     Represents an bpm gene
    /// </summary>
    public class BpmGene
    {
        #region Constructor

        /// <summary>
        ///     Creates an instance of the
        /// </summary>
        /// <param name="index"></param>
        /// <param name="parent"></param>
        public BpmGene(int index, BpmGene parent)
        {
            Index = index;
            Parent = parent;
        }

        #endregion //Constructor

        #region Properties

        /// <summary>
        ///     Index is been set by the system according to depth first search.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        ///     Parent of the gene in the BPMN-Prozess-Tree
        /// </summary>
        public BpmGene Parent { get; set; }

        /// <summary>
        ///     Children of the gene in the BPMN-Process-Tree
        /// </summary>
        public virtual List<BpmGene> Children { get; internal set; } = new List<BpmGene>();

        /// <summary>
        ///     BPMN-Activities that produce input documents needed for this.
        /// </summary>
        public virtual List<BpmnActivity> RequiredInputBpmActivities { get; internal set; }

        public string Id { get; } = Guid.NewGuid().ToString();

        public List<BpmGene> DepthOfDeepestLeaf
        {
            get
            {
                var path = new List<BpmGene>();
                if (Children != null)
                    foreach (var node in Children)
                    {
                        var tmp = node.DepthOfDeepestLeaf;
                        if (tmp.Count > path.Count)
                            path = tmp;
                    }

                path.Insert(0, this);
                return path;
            }
        }

        /// <summary>
        ///     Execution probability is needed to calculate the FitnessValue according
        ///     to μ-σ-Principle.
        /// </summary>
        public double ExecutionProbability { get; set; } = 1;

        #endregion //Properties
    }
}