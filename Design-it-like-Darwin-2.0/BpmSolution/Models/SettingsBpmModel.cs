﻿#region license
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
namespace BpmApi.Models
{
    public class SettingsBpmModel
    {
        public int TimeWeight { get; set; }
        public int CostWeight { get; set; }
        public double Alpha { get; set; }
        public double I { get; set; }
        public double Ifix { get; set; }
        public double Ivar { get; set; }
        public int MaxDepthRandomGenome { get; set; }
        public int N { get; set; }
        public bool OnlyValidSolutions { get; set; }
        public bool OnlyValidSolutionsAtStart { get; set; }
        public double PainFactor { get; set; }
        public int PopulationMultiplicator { get; set; }
        public int T { get; set; }
    }
}