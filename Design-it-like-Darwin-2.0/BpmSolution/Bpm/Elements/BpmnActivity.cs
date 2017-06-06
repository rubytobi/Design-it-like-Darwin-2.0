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