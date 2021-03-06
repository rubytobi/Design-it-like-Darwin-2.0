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
namespace Bpm
{
    public class BpmModel
    {
        private double _alpha;
        private int _costWeight;
        private double _i;
        private double _ifix;

        private bool _isFinished;
        private double _ivar;
        private int _maxDepthRandomGenome = 1;
        private int _n;
        private bool _onlyValidSolutions = true;
        private bool _onlyValidSolutionsAtStart;
        private double _painFactor;
        private int _t;
        private int _timeWeight;

        private BpmModel()
        {
            // dummy
        }

        public int GetCostWeight()
        {
            return _costWeight;
        }

        public int GetTimeWeight()
        {
            return _timeWeight;
        }

        public bool GetOnlyValidSolutions()
        {
            return _onlyValidSolutions;
        }

        public int GetMaxDepthRandomGenome()
        {
            return _maxDepthRandomGenome;
        }

        public bool GetOnlyValidSolutionsAtStart()
        {
            return _onlyValidSolutionsAtStart;
        }

        public int GetN()
        {
            return _n;
        }

        public int GetT()
        {
            return _t;
        }

        public double GetI()
        {
            return _i;
        }

        public double GetIvar()
        {
            return _ivar;
        }

        public double GetIfix()
        {
            return _ifix;
        }

        public double GetAlpha()
        {
            return _alpha;
        }

        public double GetPainFactor()
        {
            return _painFactor;
        }

        public bool isFinished()
        {
            return _isFinished;
        }

        public class Builder
        {
            private readonly BpmModel _model = new BpmModel();

            public Builder()
            {
            }

            public Builder(BpmModel model)
            {
                SetCostWeight(model.GetCostWeight());
                SetTimeWeight(model.GetTimeWeight());
                SetAlpha(model.GetAlpha());
                SetI(model.GetI());
                SetIfix(model.GetIfix());
                SetIvar(model.GetIvar());
                SetMaxDepthRandomGenome(model.GetMaxDepthRandomGenome());
                SetN(model.GetN());
                SetOnlyValidSolutions(model.GetOnlyValidSolutions());
                SetOnlyValidSolutionsAtStart(model.GetOnlyValidSolutionsAtStart());
                SetPainFactor(model.GetPainFactor());
                SetT(model.GetT());
            }

            public Builder SetCostWeight(int c)
            {
                _model._costWeight = c;
                return this;
            }

            public Builder SetTimeWeight(int t)
            {
                _model._timeWeight = t;
                return this;
            }

            public Builder SetOnlyValidSolutions(bool b)
            {
                _model._onlyValidSolutions = b;
                return this;
            }

            public Builder SetMaxDepthRandomGenome(int i)
            {
                _model._maxDepthRandomGenome = i;
                return this;
            }

            public Builder SetOnlyValidSolutionsAtStart(bool b)
            {
                _model._onlyValidSolutionsAtStart = b;
                return this;
            }

            public BpmModel Build()
            {
                _model._isFinished = true;
                return _model;
            }

            public Builder SetN(int i)
            {
                _model._n = i;
                return this;
            }

            public Builder SetT(int i)
            {
                _model._t = i;
                return this;
            }

            public Builder SetI(double d)
            {
                _model._i = d;
                return this;
            }

            public Builder SetIfix(double d)
            {
                _model._ifix = d;
                return this;
            }

            public Builder SetIvar(double d)
            {
                _model._ivar = d;
                return this;
            }

            public Builder SetAlpha(double d)
            {
                _model._alpha = d;
                return this;
            }

            public Builder SetPainFactor(double d)
            {
                _model._painFactor = d;
                return this;
            }
        }
    }
}