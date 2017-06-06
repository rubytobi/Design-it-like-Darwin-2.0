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
using Bpm.Fitnesses;
using PortableGeneticAlgorithm;
using PortableGeneticAlgorithm.Predefined;

namespace Bpm.Helpers
{
    public class ModelHelper
    {
        private static BpmModel _bpmModel;

        private static readonly BpmModel _defaultBpmModel = new BpmModel.Builder()
            .SetCostWeight(2)
            .SetTimeWeight(1)
            .SetAlpha(0.01)
            .SetI(0.025)
            .SetIfix(0.0)
            .SetIvar(0.0)
            .SetT(5)
            .SetN(100)
            .SetMaxDepthRandomGenome(5)
            .SetOnlyValidSolutions(false)
            .SetOnlyValidSolutionsAtStart(true)
            .SetPainFactor(10)
            .Build();

        private static GePrModel _gePrModel;

        private static readonly GePrModel _defaultGePrModel = new GePrModel.Builder()
            .SetAdditionalAnalytics(BpmAnalytics.Instance())
            .SetEnableAnalytics(true)
            .SetFitness(typeof(BpmnFitness))
            .SetGenerationEvolver(new BpmnGenerationEvolver())
            .SetSolutionType(typeof(BpmSolution))
            .SetUseParalell(true)
            .SetCrossoverProbability(1)
            .SetInitialGenome(null)
            .SetMaximumNumberOfGenerations(50)
            .SetMutationProbability(0)
            .SetPopulationSize(100)
            .SetSeed(42)
            .SetTermination(new IterationTermination(50))
            .SetTournamentSize(3)
            .Build();

        public static void SetBpmModel(BpmModel model)
        {
            _bpmModel = model;
        }

        public static void SetGePrModel(GePrModel model)
        {
            _gePrModel = model;
        }

        public static BpmModel GetDefaultBpmModel()
        {
            return _defaultBpmModel;
        }

        public static GePrModel GetDefaultGePrModel()
        {
            return _defaultGePrModel;
        }

        public static GePrModel GetGePrModel()
        {
            if (_gePrModel == null)
                return _defaultGePrModel;

            return _gePrModel;
        }

        public static BpmModel GetBpmModel()
        {
            if (_bpmModel == null)
                return _defaultBpmModel;

            return _bpmModel;
        }
    }
}