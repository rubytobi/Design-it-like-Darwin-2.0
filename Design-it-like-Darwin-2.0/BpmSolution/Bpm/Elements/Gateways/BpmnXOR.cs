#region license
// MIT License
// 
// Copyright (c) [2017] [Tobias Ruby]
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
#endregion
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