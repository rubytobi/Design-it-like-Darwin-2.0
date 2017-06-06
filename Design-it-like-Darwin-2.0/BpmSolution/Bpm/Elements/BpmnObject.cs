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
    /// <summary>
    /// </summary>
    public class BpmnObject
    {
        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="processInput"></param>
        /// <param name="processOutput"></param>
        /// <param name="price"></param>
        public BpmnObject(string name, string type, bool processInput, bool processOutput, double price)
        {
            Name = name;
            Type = type;
            ProcessInput = processInput;
            ProcessOutput = processOutput;
            Price = price;
        }

        public string Name { get; }
        public string Type { get; }
        public bool ProcessInput { get; }
        public bool ProcessOutput { get; }
        public double Price { get; }

        public override bool Equals(object o)
        {
            if (o is BpmnObject)
            {
                var other = o as BpmnObject;
                return Name.Equals(other.Name);
            }

            return false;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public virtual BpmnObject Clone()
        {
            return new BpmnObject(Name, Type, ProcessInput, ProcessOutput, Price);
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}