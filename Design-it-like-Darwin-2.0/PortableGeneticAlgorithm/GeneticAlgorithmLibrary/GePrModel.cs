using System;
using PortableGeneticAlgorithm.Analytics;
using PortableGeneticAlgorithm.Interfaces;
using PortableGeneticAlgorithm.Predefined;

namespace PortableGeneticAlgorithm
{
    public class GePrModel
    {
        private IAnalytics _analytics;
        private double _crossoverProbability;
        private bool _enableAnalytics;
        internal Type _fitness;
        internal IGenerationEvolver _generationEvolver;
        private IGenome _initialGenome;
        private bool _isFinished;
        private int _maximumNumberOfGenerations;
        private double _mutationProbability;
        private int _populationSize;
        private int _seed;
        internal Type _solutionType;
        private TerminationCombiner _termination;
        private int _tournamentSize;
        private bool _useParalell;

        public Type GetSolutionType()
        {
            return _solutionType;
        }

        public IAnalytics GetAnalytics()
        {
            return _analytics;
        }

        public Type GetFitness()
        {
            return _fitness;
        }

        public ITermination GetTermination()
        {
            return _termination;
        }

        public int GetPopulationSize()
        {
            return _populationSize;
        }

        public IGenerationEvolver GetGenerationEvolver()
        {
            return _generationEvolver;
        }

        public IGenome GetInitialGenome()
        {
            return _initialGenome;
        }

        public string GetInitialGenomeString()
        {
            return _initialGenome == null ? "" : _initialGenome.ToString();
        }

        public int GetMaximumNumberOfGenerations()
        {
            return _maximumNumberOfGenerations;
        }

        public bool GetUseParalell()
        {
            return _useParalell;
        }

        public int GetSeed()
        {
            return _seed;
        }

        public int GetTournamentSize()
        {
            return _tournamentSize;
        }

        public double GetMutationProbability()
        {
            return _mutationProbability;
        }

        public double GetCrossoverProbability()
        {
            return _crossoverProbability;
        }

        public bool IsFinished()
        {
            return _isFinished;
        }

        public bool AnalyticsEnabled()
        {
            return _enableAnalytics;
        }

        public class Builder
        {
            private readonly GePrModel _model = new GePrModel();

            public Builder()
            {
                Clear();
            }

            public void Clear()
            {
                _model._fitness = null;
                _model._generationEvolver = null;
                _model._termination = null;
                _model._initialGenome = null;
                _model._analytics = null;
                _model._solutionType = null;

                _model._crossoverProbability = 0.0;
                _model._mutationProbability = 0.0;
                _model._maximumNumberOfGenerations = 1;
                _model._tournamentSize = 2;
                _model._populationSize = 10;

                _model._seed = new Random().Next();

                _model._useParalell = false;
                _model._isFinished = false;
                _model._enableAnalytics = false;
            }

            public Builder SetMaximumNumberOfGenerations(int i)
            {
                if (i < 1)
                    throw new Exception("maximum number of generations too low, at least 1");

                _model._maximumNumberOfGenerations = i;
                return this;
            }

            public Builder SetCrossoverProbability(double d)
            {
                if (d < 0)
                    throw new Exception("crossover probability too low, at least 0");

                if (d > 1)
                    throw new Exception("crossover probability too high, maximum is 1");

                _model._crossoverProbability = d;
                return this;
            }

            public Builder SetMutationProbability(double d)
            {
                if (d < 0)
                    throw new Exception("mutation probability too low, at least 0");

                if (d > 1)
                    throw new Exception("mutation probability too high, maximum is 1");

                _model._mutationProbability = d;
                return this;
            }

            public Builder SetUseParalell(bool b)
            {
                _model._useParalell = b;
                return this;
            }

            public Builder SetEnableAnalytics(bool b)
            {
                _model._enableAnalytics = b;
                return this;
            }

            public Builder SetFitness(Type t)
            {
                if (!(Activator.CreateInstance(t) is IFitness))
                    throw new Exception("fitness does not implement IFitness interface");

                try
                {
                    var dump = (IFitness) Activator.CreateInstance(t);
                }
                catch (Exception)
                {
                    throw new Exception("not possible to create a fitness object (test failed)");
                }

                _model._fitness = t;
                return this;
            }

            public Builder SetSeed(int i)
            {
                _model._seed = i;
                return this;
            }

            public Builder SetTournamentSize(int i)
            {
                if (i < 2)
                    throw new Exception("turnament size too small, at least 2");

                _model._tournamentSize = i;
                return this;
            }

            public Builder SetGenerationEvolver(IGenerationEvolver g)
            {
                if (g == null)
                    throw new Exception("generation evolver is null");

                _model._generationEvolver = g;
                return this;
            }

            public Builder SetAdditionalAnalytics(IAnalytics a)
            {
                _model._analytics = a;
                return this;
            }

            public Builder SetPopulationSize(int i)
            {
                if (i < 1)
                    throw new Exception("population size too small");

                _model._populationSize = i;
                return this;
            }

            public Builder SetTermination(params ITermination[] parameters)
            {
                if (parameters.Length == 0)
                    throw new Exception("no termination provided, please call with null");

                _model._termination = new TerminationCombiner(parameters);

                return this;
            }

            public Builder SetSolutionType(Type t)
            {
                if (t == null)
                    throw new Exception("solution type is null");

                if (!(Activator.CreateInstance(t) is Solution))
                    throw new Exception("solution type is not subclass of solution");

                _model._solutionType = t;
                return this;
            }

            public Builder SetInitialGenome(IGenome g)
            {
                _model._initialGenome = g;
                return this;
            }

            public GePrModel Build()
            {
                if (_model._generationEvolver == null
                    || _model._fitness == null
                    || _model._termination == null
                    || _model._solutionType == null)
                    throw new Exception("Not all input delivered...");

                _model._isFinished = true;
                return _model;
            }
        }
    }
}