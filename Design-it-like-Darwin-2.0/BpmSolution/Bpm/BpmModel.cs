using System;

namespace Bpm
{
    public class BpmModel
    {
        private int _costWeight;
        private int _timeWeight;
        private int _n;
        private int _t;
        private double _i;
        private double _ifix;
        private double _ivar;
        private double _alpha;
        private double _painFactor;
        private bool _onlyValidSolutions = true;
        private int _maxDepthRandomGenome = 1;
        private int _populationMultiplicator;
        private bool _onlyValidSolutionsAtStart;

        private bool _isFinished;

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

        public int GetPopulationMultiplicator()
        {
            return _populationMultiplicator;
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

        private BpmModel()
        {
            // dummy
        }

        public class Builder
        {
            private BpmModel _model = new BpmModel();

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
                SetPopulationMultiplicator(model.GetPopulationMultiplicator());
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

            public Builder SetN(object n)
            {
                throw new NotImplementedException();
            }

            public Builder SetMaxDepthRandomGenome(int i)
            {
                _model._maxDepthRandomGenome = i;
                return this;
            }

            public Builder SetPopulationMultiplicator(int i)
            {
                _model._populationMultiplicator = i;
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