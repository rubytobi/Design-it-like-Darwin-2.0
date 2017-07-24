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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Bpm;
using Bpm.Fitnesses;
using Bpm.Helpers;
using BpmApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class UnitTests
    {
        public static void LoadDummyData2()
        {
            #region ActivityHelper

            DataHelper.ActivityHelper.Instance()
                .Models
                .Add(
                    new ActivityModel
                    {
                        id = Guid.NewGuid(),
                        name = "a1",
                        description = "",
                        cashflow = -1,
                        variance = 0.3,
                        duration = 1
                    });
            DataHelper.ActivityHelper.Instance()
                .Models
                .Add(
                    new ActivityModel
                    {
                        id = Guid.NewGuid(),
                        name = "a2",
                        description = "",
                        cashflow = -7,
                        variance = 14.82,
                        duration = 1
                    });
            DataHelper.ActivityHelper.Instance()
                .Models
                .Add(
                    new ActivityModel
                    {
                        id = Guid.NewGuid(),
                        name = "a3",
                        description = "",
                        cashflow = -4,
                        variance = 4.84,
                        duration = 1
                    });
            DataHelper.ActivityHelper.Instance()
                .Models
                .Add(
                    new ActivityModel
                    {
                        id = Guid.NewGuid(),
                        name = "a4",
                        description = "",
                        cashflow = -23,
                        variance = 160.02,
                        duration = 1
                    });
            DataHelper.ActivityHelper.Instance()
                .Models
                .Add(
                    new ActivityModel
                    {
                        id = Guid.NewGuid(),
                        name = "a5",
                        description = "",
                        cashflow = -29,
                        variance = 254.4,
                        duration = 1
                    });
            DataHelper.ActivityHelper.Instance()
                .Models
                .Add(
                    new ActivityModel
                    {
                        id = Guid.NewGuid(),
                        name = "a6",
                        description = "",
                        cashflow = -20,
                        variance = 121,
                        duration = 1
                    });
            DataHelper.ActivityHelper.Instance()
                .Models
                .Add(
                    new ActivityModel
                    {
                        id = Guid.NewGuid(),
                        name = "a7",
                        description = "",
                        cashflow = -4,
                        variance = 4.84,
                        duration = 1
                    });
            DataHelper.ActivityHelper.Instance()
                .Models
                .Add(
                    new ActivityModel
                    {
                        id = Guid.NewGuid(),
                        name = "a8",
                        description = "",
                        cashflow = -25,
                        variance = 189.06,
                        duration = 1
                    });

            #endregion
        }

        public static void LoadDummyData()
        {
            #region ActivityHelper

            DataHelper.ActivityHelper.Instance()
                .Models
                .Add(
                    new ActivityModel
                    {
                        id = Guid.NewGuid(),
                        name = "a1",
                        description = "",
                        cashflow = -1,
                        variance = 0.3,
                        duration = 1
                    });
            DataHelper.ActivityHelper.Instance()
                .Models
                .Add(
                    new ActivityModel
                    {
                        id = Guid.NewGuid(),
                        name = "a2",
                        description = "",
                        cashflow = -7,
                        variance = 14.82,
                        duration = 1
                    });
            DataHelper.ActivityHelper.Instance()
                .Models
                .Add(
                    new ActivityModel
                    {
                        id = Guid.NewGuid(),
                        name = "a3",
                        description = "",
                        cashflow = -4,
                        variance = 4.84,
                        duration = 1
                    });
            DataHelper.ActivityHelper.Instance()
                .Models
                .Add(
                    new ActivityModel
                    {
                        id = Guid.NewGuid(),
                        name = "a4",
                        description = "",
                        cashflow = -23,
                        variance = 160.02,
                        duration = 1
                    });
            DataHelper.ActivityHelper.Instance()
                .Models
                .Add(
                    new ActivityModel
                    {
                        id = Guid.NewGuid(),
                        name = "a5",
                        description = "",
                        cashflow = -29,
                        variance = 254.4,
                        duration = 1
                    });
            DataHelper.ActivityHelper.Instance()
                .Models
                .Add(
                    new ActivityModel
                    {
                        id = Guid.NewGuid(),
                        name = "a6",
                        description = "",
                        cashflow = -20,
                        variance = 121,
                        duration = 1
                    });
            DataHelper.ActivityHelper.Instance()
                .Models
                .Add(
                    new ActivityModel
                    {
                        id = Guid.NewGuid(),
                        name = "a7",
                        description = "",
                        cashflow = -4,
                        variance = 4.84,
                        duration = 1
                    });
            DataHelper.ActivityHelper.Instance()
                .Models
                .Add(
                    new ActivityModel
                    {
                        id = Guid.NewGuid(),
                        name = "a8",
                        description = "",
                        cashflow = -25,
                        variance = 189.06,
                        duration = 1
                    });

            #endregion

            #region ObjectHelper

            DataHelper.ObjectHelper.Instance()
                .Models
                .Add(
                    new ObjectModel
                    {
                        id = Guid.NewGuid(),
                        name = "o1",
                        description = "",
                        type = "Information",
                        processInput = true,
                        processOutput = false,
                        price = 0.0
                    });
            DataHelper.ObjectHelper.Instance()
                .Models
                .Add(
                    new ObjectModel
                    {
                        id = Guid.NewGuid(),
                        name = "o2",
                        description = "",
                        type = "Information",
                        processInput = true,
                        processOutput = false,
                        price = 0.0
                    });
            DataHelper.ObjectHelper.Instance()
                .Models
                .Add(
                    new ObjectModel
                    {
                        id = Guid.NewGuid(),
                        name = "o3",
                        description = "",
                        type = "Information",
                        processInput = false,
                        processOutput = false,
                        price = 0.0
                    });
            DataHelper.ObjectHelper.Instance()
                .Models
                .Add(
                    new ObjectModel
                    {
                        id = Guid.NewGuid(),
                        name = "o4",
                        description = "",
                        type = "Information",
                        processInput = false,
                        processOutput = false,
                        price = 0.0
                    });
            DataHelper.ObjectHelper.Instance()
                .Models
                .Add(
                    new ObjectModel
                    {
                        id = Guid.NewGuid(),
                        name = "o5",
                        description = "",
                        type = "Information",
                        processInput = false,
                        processOutput = true,
                        price = 20.0
                    });
            DataHelper.ObjectHelper.Instance()
                .Models
                .Add(
                    new ObjectModel
                    {
                        id = Guid.NewGuid(),
                        name = "o6",
                        description = "",
                        type = "Information",
                        processInput = false,
                        processOutput = true,
                        price = 25.0
                    });
            DataHelper.ObjectHelper.Instance()
                .Models
                .Add(
                    new ObjectModel
                    {
                        id = Guid.NewGuid(),
                        name = "o7",
                        description = "",
                        type = "Information",
                        processInput = false,
                        processOutput = true,
                        price = 20.0
                    });
            DataHelper.ObjectHelper.Instance()
                .Models
                .Add(
                    new ObjectModel
                    {
                        id = Guid.NewGuid(),
                        name = "o8",
                        description = "",
                        type = "Information",
                        processInput = false,
                        processOutput = true,
                        price = 25.0
                    });

            #endregion

            #region ActivityInputHelper

            DataHelper.ActivityInputHelper.Instance()
                .Models
                .Add(new InputModel
                {
                    activityName = "a1",
                    objectName = "o1"
                });
            DataHelper.ActivityInputHelper.Instance()
                .Models
                .Add(new InputModel
                {
                    activityName = "a1",
                    objectName = "o2"
                });
            DataHelper.ActivityInputHelper.Instance()
                .Models
                .Add(new InputModel
                {
                    activityName = "a2",
                    objectName = "o1"
                });
            DataHelper.ActivityInputHelper.Instance()
                .Models
                .Add(new InputModel
                {
                    activityName = "a2",
                    objectName = "o2"
                });
            DataHelper.ActivityInputHelper.Instance()
                .Models
                .Add(new InputModel
                {
                    activityName = "a3",
                    objectName = "o1"
                });
            DataHelper.ActivityInputHelper.Instance()
                .Models
                .Add(new InputModel
                {
                    activityName = "a3",
                    objectName = "o2"
                });
            DataHelper.ActivityInputHelper.Instance()
                .Models
                .Add(new InputModel
                {
                    activityName = "a4",
                    objectName = "o1"
                });
            DataHelper.ActivityInputHelper.Instance()
                .Models
                .Add(new InputModel
                {
                    activityName = "a4",
                    objectName = "o2"
                });
            DataHelper.ActivityInputHelper.Instance()
                .Models
                .Add(new InputModel
                {
                    activityName = "a5",
                    objectName = "o3"
                });
            DataHelper.ActivityInputHelper.Instance()
                .Models
                .Add(new InputModel
                {
                    activityName = "a6",
                    objectName = "o3"
                });
            DataHelper.ActivityInputHelper.Instance()
                .Models
                .Add(new InputModel
                {
                    activityName = "a7",
                    objectName = "o4"
                });
            DataHelper.ActivityInputHelper.Instance()
                .Models
                .Add(new InputModel
                {
                    activityName = "a8",
                    objectName = "o4"
                });

            #endregion

            #region ActivityOutputHelper

            DataHelper.ActivityOutputHelper.Instance()
                .Models
                .Add(new OutputModel
                {
                    activityName = "a1",
                    objectName = "o3"
                });
            DataHelper.ActivityOutputHelper.Instance()
                .Models
                .Add(new OutputModel
                {
                    activityName = "a2",
                    objectName = "o3"
                });
            DataHelper.ActivityOutputHelper.Instance()
                .Models
                .Add(new OutputModel
                {
                    activityName = "a3",
                    objectName = "o4"
                });
            DataHelper.ActivityOutputHelper.Instance()
                .Models
                .Add(new OutputModel
                {
                    activityName = "a4",
                    objectName = "o4"
                });
            DataHelper.ActivityOutputHelper.Instance()
                .Models
                .Add(new OutputModel
                {
                    activityName = "a5",
                    objectName = "o5"
                });
            DataHelper.ActivityOutputHelper.Instance()
                .Models
                .Add(new OutputModel
                {
                    activityName = "a5",
                    objectName = "o6"
                });
            DataHelper.ActivityOutputHelper.Instance()
                .Models
                .Add(new OutputModel
                {
                    activityName = "a6",
                    objectName = "o5"
                });
            DataHelper.ActivityOutputHelper.Instance()
                .Models
                .Add(new OutputModel
                {
                    activityName = "a6",
                    objectName = "o6"
                });
            DataHelper.ActivityOutputHelper.Instance()
                .Models
                .Add(new OutputModel
                {
                    activityName = "a7",
                    objectName = "o7"
                });
            DataHelper.ActivityOutputHelper.Instance()
                .Models
                .Add(new OutputModel
                {
                    activityName = "a7",
                    objectName = "o8"
                });
            DataHelper.ActivityOutputHelper.Instance()
                .Models
                .Add(new OutputModel
                {
                    activityName = "a8",
                    objectName = "o7"
                });
            DataHelper.ActivityOutputHelper.Instance()
                .Models
                .Add(new OutputModel
                {
                    activityName = "a8",
                    objectName = "o8"
                });

            #endregion

            #region ActivityAttributeHelper

            DataHelper.ActivityAttributeHelper.Instance()
                .Models
                .Add(new ActivityAttributeModel
                {
                    decisionId = "1",
                    decisionValue = "1",
                    decisionProbability = 0.025
                });
            DataHelper.ActivityAttributeHelper.Instance()
                .Models
                .Add(new ActivityAttributeModel
                {
                    decisionId = "1",
                    decisionValue = "2",
                    decisionProbability = 0.175
                });
            DataHelper.ActivityAttributeHelper.Instance()
                .Models
                .Add(new ActivityAttributeModel
                {
                    decisionId = "1",
                    decisionValue = "3",
                    decisionProbability = 0.5
                });
            DataHelper.ActivityAttributeHelper.Instance()
                .Models
                .Add(new ActivityAttributeModel
                {
                    decisionId = "1",
                    decisionValue = "4",
                    decisionProbability = 0.25
                });
            DataHelper.ActivityAttributeHelper.Instance()
                .Models
                .Add(new ActivityAttributeModel
                {
                    decisionId = "1",
                    decisionValue = "5",
                    decisionProbability = 0.05
                });

            #endregion

            #region CoverHelper

            DataHelper.CoverHelper.Instance()
                .Models
                .Add(new CoverModel
                {
                    activityName = "a7",
                    decisionId = "1",
                    decisionValue = "5",
                    isCovered = false
                });
            DataHelper.CoverHelper.Instance()
                .Models
                .Add(new CoverModel
                {
                    activityName = "a8",
                    decisionId = "1",
                    decisionValue = "1",
                    isCovered = false
                });
            DataHelper.CoverHelper.Instance()
                .Models
                .Add(new CoverModel
                {
                    activityName = "a8",
                    decisionId = "1",
                    decisionValue = "2",
                    isCovered = false
                });
            DataHelper.CoverHelper.Instance()
                .Models
                .Add(new CoverModel
                {
                    activityName = "a8",
                    decisionId = "1",
                    decisionValue = "3",
                    isCovered = false
                });
            DataHelper.CoverHelper.Instance()
                .Models
                .Add(new CoverModel
                {
                    activityName = "a8",
                    decisionId = "1",
                    decisionValue = "4",
                    isCovered = false
                });

            #endregion
        }

        [ClassInitialize]
        public static void Load(TestContext context)
        {
            LoadDummyData2();
        }

        [TestMethod]
        public void GenerateGenomes()
        {
            var count = 1000000;

            var maxDepthRandomGenome = ModelHelper.GetBpmModel().GetMaxDepthRandomGenome();

            var genomes = new List<string>();

            for (var i = 0; i < count; i++)
            {
                var genome = new BpmGenome
                {
                    RootGene =
                        ProcessHelper.ProcessGenerator.GenerateRandomValidBpmGenome2(
                            maxDepthRandomGenome, null)
                };

                genomes.Add(genome.ToString());
            }

            var distinctCount = genomes.Distinct().Count();
            Assert.IsTrue(distinctCount >= count * 0.5);
        }

        [TestMethod]
        public void Fitness()
        {
            ModelHelper.SetBpmModel(new BpmModel.Builder(ModelHelper.GetBpmModel()).SetAlpha(0.0).Build());
            var fitness = new BpmnFitness(false);

            var genome0 = "<PI;SEQ(a3;SEQ(SEQ(a1;a6;XOR(v[1,5];SEQ(a8;a6;a3);a7))));PO>".ParseBpmGenome();
            var f0 = fitness.Evaluate(genome0) as BpmSolution;
            Debug.WriteLine(f0.Fitness);

            var genome1 = "<PI;SEQ(a3;SEQ(SEQ(a1;a6;XOR(v[1,5];a8;a7))));PO>".ParseBpmGenome();
            var f1 = fitness.Evaluate(genome1) as BpmSolution;
            Debug.WriteLine(f1.Fitness);

            Assert.IsTrue(f0.Fitness < f1.Fitness);
        }

        [TestMethod]
        public void Fitness3()
        {
            ModelHelper.SetBpmModel(new BpmModel.Builder(ModelHelper.GetBpmModel()).SetAlpha(0.01).Build());
            var fitness = new BpmnFitness(false);

            var genome0 = "<PI;AND(SEQ(a1;a6);SEQ(a3;XOR(v[1,5];a8;a7)));PO>".ParseBpmGenome();
            var f0 = fitness.Evaluate(genome0) as BpmSolution;
            Debug.WriteLine(f0.Fitness);

            var genome1 = "<PI;SEQ(a3;SEQ(SEQ(a1;a6;XOR(v[1,5];a8;a7))));PO>".ParseBpmGenome();
            var f1 = fitness.Evaluate(genome1) as BpmSolution;
            Debug.WriteLine(f1.Fitness);

            Assert.IsTrue(f0.Fitness > f1.Fitness);
        }

        [TestMethod]
        public void FitnessSimilar2()
        {
            ModelHelper.SetBpmModel(new BpmModel.Builder(ModelHelper.GetBpmModel()).SetAlpha(0.0).Build());
            var fitness = new BpmnFitness(false);

            var genome0 = "<PI;SEQ(a1;a3;a6;XOR(v[1,5];a8;a7));PO>".ParseBpmGenome();
            var f0 = fitness.Evaluate(genome0) as BpmSolution;
            Debug.WriteLine(f0.Fitness);

            var genome1 = "<PI;SEQ(a1;a3;a6;XOR(v[1,5];a8;XOR(v[1,5];a8;a7)));PO>".ParseBpmGenome();
            var f1 = fitness.Evaluate(genome1) as BpmSolution;
            Debug.WriteLine(f1.Fitness);

            Assert.AreEqual(f0.Fitness, f1.Fitness);
        }

        [TestMethod]
        public void FitnessSimilarPaperGenome()
        {
            ModelHelper.SetBpmModel(new BpmModel.Builder(ModelHelper.GetBpmModel()).SetAlpha(0.05).Build());
            var fitness = new BpmnFitness(false);

            var genome0 = "<PI;AND(SEQ(a2;a6);SEQ(a3;XOR(v[1,5];a8;a7)));PO>".ParseBpmGenome();
            var f0 = fitness.Evaluate(genome0) as BpmSolution;
            Debug.WriteLine(f0.Fitness);

            var genome1 = "<PI;AND(SEQ(a1;a6);SEQ(a4;XOR(v[1,5];a8;a7)));PO>".ParseBpmGenome();
            var f1 = fitness.Evaluate(genome1) as BpmSolution;
            Debug.WriteLine(f1.Fitness);

            var genome2 = "<PI;AND(SEQ(a1;a5);SEQ(XOR(v[1,3];a4;a3);XOR(v[1,5];a8;a7)));PO>".ParseBpmGenome();
            var f2 = fitness.Evaluate(genome2) as BpmSolution;
            Debug.WriteLine(f2.Fitness);

            var genome3 = "<PI;AND(SEQ(a1;a5);SEQ(a4;XOR(v[1,5];a8;a7)));PO>".ParseBpmGenome();
            var f3 = fitness.Evaluate(genome3) as BpmSolution;
            Debug.WriteLine(f3.Fitness);

            var genome4 = "<PI;AND(SEQ(XOR(v[1,2];a1;a2);a5);SEQ(XOR(v[1,4];a3;a4);XOR(v[1,5];a8;a7)));PO>"
                .ParseBpmGenome();
            var f4 = fitness.Evaluate(genome4) as BpmSolution;
            Debug.WriteLine(f4.Fitness);

            Assert.IsTrue(f0.Fitness > f1.Fitness);
            Assert.IsTrue(f1.Fitness > f2.Fitness);
            Assert.IsTrue(f2.Fitness > f3.Fitness);
            Assert.IsTrue(f3.Fitness > f4.Fitness);
        }

        [TestMethod]
        public void FitnessSimilar1()
        {
            ModelHelper.SetBpmModel(new BpmModel.Builder(ModelHelper.GetBpmModel()).SetAlpha(0.0).Build());
            var fitness = new BpmnFitness(false);

            var genome0 = "<PI;SEQ(SEQ(a2);a6;SEQ(a3;XOR(v[1,5];a8;a7)));PO>".ParseBpmGenome();
            var f0 = fitness.Evaluate(genome0) as BpmSolution;
            Debug.WriteLine(f0.Fitness);

            var genome1 = "<PI;SEQ(a2;a6;a3;XOR(v[1,5];a8;a7));PO>".ParseBpmGenome();
            var f1 = fitness.Evaluate(genome1) as BpmSolution;
            Debug.WriteLine(f1.Fitness);

            var genome2 = "<PI;AND(SEQ(AND(a2);a6);SEQ(AND(a3);XOR(v[1,5];AND(a8);a7)));PO>".ParseBpmGenome();
            var f2 = fitness.Evaluate(genome2) as BpmSolution;
            Debug.WriteLine(f2.Fitness);

            Assert.IsTrue(f0.Fitness == f1.Fitness);
            Assert.IsTrue(f1.Fitness == f2.Fitness);
        }

        [TestMethod]
        public void FitnessBestSolutionTest()
        {
            ModelHelper.SetBpmModel(new BpmModel.Builder(ModelHelper.GetBpmModel()).SetAlpha(0.0).Build());
            var fitness = new BpmnFitness(false);

            var genome0 = "<PI;AND(SEQ(a2;a6);SEQ(a3;XOR(v[1,5];a8;a7)));PO>".ParseBpmGenome();
            var f0 = fitness.Evaluate(genome0) as BpmSolution;
            Debug.WriteLine(f0.Fitness);

            Assert.IsTrue(f0.Fitness == 5333.41);
        }

        [TestMethod]
        public void FitnessTestCrazy1()
        {
            ModelHelper.SetBpmModel(new BpmModel.Builder(ModelHelper.GetBpmModel()).SetAlpha(0.0).Build());
            var fitness = new BpmnFitness(false);

            var genome0 = "<PI;AND(SEQ(a2;a6);SEQ(a3;XOR(v[1,5];a8;a7)));PO>".ParseBpmGenome();
            var f0 = fitness.Evaluate(genome0) as BpmSolution;
            Debug.WriteLine(f0.Fitness);

            var genome1 = "<PI;SEQ(a1;SEQ(a1;a6;a3;XOR(v[1,5];a6;a7));a3;XOR(v[1,5];a8;a7));PO>".ParseBpmGenome();
            var f1 = fitness.Evaluate(genome1) as BpmSolution;
            Debug.WriteLine(f1.Fitness);

            Assert.IsTrue(f0.Fitness > f1.Fitness);
        }

        [TestMethod]
        public void FitnessTestCrazy2()
        {
            ModelHelper.SetBpmModel(new BpmModel.Builder(ModelHelper.GetBpmModel()).SetAlpha(0.0).Build());
            var fitness = new BpmnFitness(false);

            var genome0 = "<PI;AND(SEQ(a2;a6);SEQ(a3;XOR(v[1,5];a8;a7)));PO>".ParseBpmGenome();
            var f0 = fitness.Evaluate(genome0) as BpmSolution;
            Debug.WriteLine(f0.Fitness);

            var genome1 = "<PI;SEQ(a1;SEQ(a1;a6;a3;XOR(v[1,5];a6;a7));a3;XOR(v[1,5];a8;a7);XOR(v[1,3];));PO>"
                .ParseBpmGenome();
            var f1 = fitness.Evaluate(genome1) as BpmSolution;
            Debug.WriteLine(f1.Fitness);

            Assert.IsTrue(f0.Fitness > f1.Fitness);
        }

        [TestMethod]
        public void FitnessTestMasse()
        {
            string[] array =
            {
                "<PI;SEQ(a1;SEQ(XOR(v[1,5];a5;a6);a4;AND(a6;a6;a1;XOR(v[1,4];a8;a7));a6);XOR(v[1,2];SEQ(AND(a7;AND(a7;a7;a7);a7));AND(AND(a1);a5;a1;a1)));PO>",
                "<PI;SEQ(a1;a3;a6;SEQ(XOR(v[1,5];a8;a7);a4));PO>",
                "<PI;SEQ(a4;SEQ(a2;a5;XOR(v[1,5];a8;SEQ(a7;a4)));a5;a5);PO>",
                "<PI;AND(SEQ(a3;a1;a6);SEQ(SEQ(a2;SEQ(a3;a6;a2;XOR(v[1,5];a8;a7));a2;a4);a3;a2;XOR(v[1,4];a1)));PO>",
                "<PI;SEQ(a3;SEQ(a1;AND(XOR(v[1,5];a8;a7));a5;a3));PO>",
                "<PI;SEQ(a2;SEQ(SEQ(SEQ(a6;a4;a5;a2);a4;a1;a6);a2;a2);a2;AND(SEQ(AND(SEQ(a3;a2;a5);a5;XOR(v[1,5];a8;a7));a3);a6));PO>",
                "<PI;SEQ(a3;SEQ(a1;a6;a4;SEQ(SEQ(AND(a1;a1;a6;a2));a6;a1));XOR(v[1,5];a8;a7));PO>",
                "<PI;SEQ(SEQ(a1;AND(a6;a3;a3;XOR(v[1,5];a4;SEQ(a3;a5;a7)));SEQ(a4;a5;a1;a4);a3);SEQ(AND(SEQ(a4;XOR(v[1,3];a7;a2);a4;XOR(v[1,1];a7)));SEQ(a5)));PO>",
                "<PI;SEQ(a3;a2;a6;AND(a6;XOR(v[1,1];SEQ(AND(a7;a7;a7;a7);a7);XOR(v[1,5];a8;AND(a7;a7;a7;a7)));a6));PO>",
                "<PI;SEQ(SEQ(a1);a5;a3;XOR(v[1,4];a7;XOR(v[1,5];SEQ(XOR(v[1,5];a8);SEQ(a4;a8;a3));a7)));PO>",
                "<PI;SEQ(a4;a2;a6;AND(XOR(v[1,5];a8;a7);a6;AND(SEQ(a1;a5))));PO>",
                "<PI;SEQ(a1;a3;XOR(v[1,3];a5;a5);AND(XOR(v[1,5];a8;a7);AND(a5;SEQ(a3;a4;a5;AND(a6;a1;a2));a1);a2;AND(a1;SEQ(a5;a5;a6);XOR(v[1,3];a7;a3))));PO>",
                "<PI;AND(a1;a3;SEQ(a1;a5;a3;XOR(v[1,5];a8;a7));XOR(v[1,4];XOR(v[1,4];a2;SEQ(a1;AND(a3;a5;a4);AND(a3;a6);a4))));PO>",
                "<PI;SEQ(SEQ(XOR(v[1,4];a4;a3);XOR(v[1,5];SEQ(a1;a5;a8);AND(a1;SEQ(a1);SEQ(a7;a1;a6;a6);a2));AND(a2;a4;a4));a2);PO>",
                "<PI;AND(a3;SEQ(a2;a6;XOR(v[1,5];SEQ(XOR(v[1,5];a4;a4));a4);XOR(v[1,5];a8;SEQ(a7;a6;XOR(v[1,3];a6);a6)));a2);PO>",
                "<PI;SEQ(XOR(v[1,2];a3;a3);a1;SEQ(a6;AND(a3);XOR(v[1,5];SEQ(SEQ(a8);XOR(v[1,5];a1;a5));AND(a7));a4));PO>",
                "<PI;SEQ(a1;SEQ(XOR(v[1,5];a5;a6);a4;AND(a6;a6;a1;XOR(v[1,5];a8;a7));a6);XOR(v[1,2];SEQ(AND(a7;AND(a7;a7;a7);a7));AND(AND(a1);a5;a1;a1)));PO>",
                "<PI;AND(a4;SEQ(SEQ(a4);a1;SEQ(AND(XOR(v[1,5];a8;a7);a6);a6;a4);a5));PO>",
                "<PI;SEQ(SEQ(a1;SEQ(AND(XOR(v[1,4];a4);a5;a4;XOR(v[1,1];a4;a6));SEQ(a6;a4;AND(a1;a1);a1);a3);a3;SEQ(a2;a3));SEQ(SEQ(a4;XOR(v[1,5];a8;a7);AND(a1;a5;a5;a4);SEQ(a4)));a6;SEQ(AND(SEQ(a5;SEQ(a2));a2;a6;AND(a6;a4;a1;a6));a6));PO>",
                "<PI;SEQ(a4;SEQ(a2;AND(SEQ(AND(a5;a6);a6);a6;SEQ(XOR(v[1,5];a5;a7);a4));a6;XOR(v[1,3];a7));a1);PO>",
                "<PI;AND(SEQ(SEQ(a3;XOR(v[1,5];a1;SEQ(a2;a7;a5)));AND(a2;a5);SEQ(AND(SEQ(a4;a4;a2;a6);AND(a4))));a3;SEQ(SEQ(a4;a1;AND(SEQ(a6);SEQ(a5;a6);a5;a6);a6);SEQ(a2;a1;XOR(v[1,2];a7;a4)));a2);PO>",
                "<PI;SEQ(a3;AND(SEQ(a1;SEQ(a5;XOR(v[1,5];a8;a7);a4));AND(a1;a2;a1;a2));a1);PO>",
                "<PI;SEQ(SEQ(a2;AND(a6;a4));a3;XOR(v[1,3];a5;a6);XOR(v[1,5];a8;a7));PO>",
                "<PI;SEQ(a1;SEQ(a5;a4;a5);AND(a5;a1;a5;XOR(v[1,5];a8;SEQ(a7;a1;a5;SEQ(a6;a3;a6)))));PO>",
                "<PI;SEQ(a4;SEQ(a1;XOR(v[1,3];a5;a6);SEQ(a2;a4;XOR(v[1,5];XOR(v[1,5];a8;a8);a7);SEQ(a4;a1;a3;a5))));PO>",
                "<PI;SEQ(a3;SEQ(a1;a5);SEQ(SEQ(a5;AND(a4;a5));SEQ(XOR(v[1,5];a8;a7);AND(a2;a5;a1;a6));XOR(v[1,5];XOR(v[1,5];SEQ(a5;a2;a6))));a4);PO>",
                "<PI;SEQ(a3;AND(a2;SEQ(a1;SEQ(AND(a5;a5);XOR(v[1,3];a5));SEQ(a6;a5;a4;XOR(v[1,5];a8;a7)));a2));PO>",
                "<PI;SEQ(a3;AND(XOR(v[1,3];a1;SEQ(AND(a2;a1;a2;a2);a2));a1);a1;XOR(v[1,5];SEQ(AND(a5;a8);a6;a8;a5);SEQ(a5;SEQ(XOR(v[1,3];a7));SEQ(a7;a7;a4))));PO>",
                "<PI;SEQ(a2;SEQ(XOR(v[1,3];a6;a6);a4;a2;XOR(v[1,5];a8;a7)));PO>",
                "<PI;AND(a1;AND(SEQ(a1;AND(AND(a3);SEQ(a3;a6;a4);a6;a3);a6;a3));a3;AND(SEQ(XOR(v[1,1];a4;XOR(v[1,2];a3;a3));SEQ(a1;a6;a3);a5;XOR(v[1,5];a8;a7))));PO>",
                "<PI;SEQ(a2;a5;AND(a3;a3);SEQ(a3;a6;a5;SEQ(XOR(v[1,5];XOR(v[1,5];a8;a7);a7))));PO>",
                "<PI;SEQ(a3;AND(a1;XOR(v[1,4];XOR(v[1,4];a7);a1);XOR(v[1,5];AND(XOR(v[1,5];a8);a2;a2);XOR(v[1,4];a7));a2);SEQ(a2;a5;a6);XOR(v[1,5];a8;a7));PO>",
                "<PI;SEQ(a3;SEQ(XOR(v[1,3];a1;a1));a5;AND(a4;AND(SEQ(SEQ(a2;a2;a5;a5);XOR(v[1,5];a8;a7);a4);a5;a5;SEQ(a3;XOR(v[1,2];a7;a3);SEQ(a6;a2;a4;a6)));a1;a5));PO>",
                "<PI;AND(AND(a4;a2;a4);a3;SEQ(a4;XOR(v[1,1];a1;a2);XOR(v[1,4];a6);AND(AND(AND(a2;a4;a2;a3);XOR(v[1,4];a7;a6);XOR(v[1,4];a7));a5));SEQ(a3;a1;a5;AND(SEQ(a3;XOR(v[1,5];a8;a7));AND(a5;a2);XOR(v[1,5];SEQ(a8);a7))));PO>",
                "<PI;SEQ(a2;SEQ(a3;AND(a5;XOR(v[1,5];a8;SEQ(a6;a7;a3)));a5;XOR(v[1,1];a7));AND(a4);a1);PO>",
                "<PI;SEQ(a1;SEQ(XOR(v[1,3];a6;a6);a4);a4;XOR(v[1,5];AND(a8;a8;SEQ(a8);a8);a7));PO>",
                "<PI;SEQ(a4;a1;a6;SEQ(a1;AND(SEQ(a4;a3;AND(a3;a3;a6;a5);XOR(v[1,5];a8;a7));a2;XOR(v[1,1];XOR(v[1,1];a7;a7)));a1));PO>",
                "<PI;AND(a3;SEQ(a2;a5;a4;AND(a2;SEQ(XOR(v[1,5];a8;a7);a5;a6;a3);a6));a1);PO>",
                "<PI;SEQ(a3;a2;a6;SEQ(XOR(v[1,5];a8;SEQ(a7;SEQ(a6;a4;a5;a1);a3;a2));a6;a6));PO>",
                "<PI;SEQ(a3;a1;AND(AND(SEQ(a5;XOR(v[1,5];a8;a7));a5;XOR(v[1,4];a7);AND(a5))));PO>",
                "<PI;SEQ(XOR(v[1,2];a2;AND(AND(AND(a2);AND(a4;a1);SEQ(a3;a1;a6));a3));a3;a5;AND(AND(SEQ(a2;a1;a1;a4);a1;a4;a1);a1;SEQ(a1;XOR(v[1,5];a8;a7);a3;a5);AND(SEQ(a1;SEQ(a6;a2));a6)));PO>",
                "<PI;AND(a2;AND(a4;SEQ(SEQ(a3);a2;a6);SEQ(a2));SEQ(AND(a3;XOR(v[1,5];SEQ(a2;a6;a3;a8);SEQ(a3;a7)));a4);a3);PO>",
                "<PI;SEQ(a2;SEQ(a5;a3;XOR(v[1,1];a7;a6));XOR(v[1,5];AND(a2;XOR(v[1,5];SEQ(a4;a2;a5;a1);a5);a2);a7));PO>",
                "<PI;AND(SEQ(a4;SEQ(a1;a5;a5);SEQ(XOR(v[1,1];a7;XOR(v[1,5];a8;a7));SEQ(a1;a6)));a1;a4;AND(a1;XOR(v[1,1];a2;a3);SEQ(a1;a6)));PO>",
                "<PI;SEQ(a4;a1;a6;SEQ(XOR(v[1,5];a8;a7);XOR(v[1,1];a5;a3);SEQ(a6;a4;a2;a1)));PO>",
                "<PI;SEQ(a1;SEQ(a5;SEQ(a3));XOR(v[1,5];a8;SEQ(a7;a1;a4)));PO>",
                "<PI;SEQ(a3;AND(AND(a2;XOR(v[1,5];a8;a1);a1;a2);a2;SEQ(a2;a5;XOR(v[1,5];a8;XOR(v[1,1];a7;a7))));a2;a5);PO>",
                "<PI;AND(SEQ(SEQ(SEQ(a1;a3;a6);a1;XOR(v[1,5];a8;AND(a7;a7;a7));AND(a5;XOR(v[1,4];a7;a4)));a5;SEQ(a2;SEQ(a5;a4;SEQ(a2;a6);XOR(v[1,1];a6));a2));SEQ(XOR(v[1,5];AND(a1);SEQ(a2;AND(a3))));a1);PO>",
                "<PI;AND(SEQ(SEQ(a2;a3);a5;XOR(v[1,5];XOR(v[1,5];SEQ(a8);SEQ(a8;a7;a5));a7));a3;a3;AND(a1));PO>",
                "<PI;SEQ(a4;XOR(v[1,5];SEQ(a1;SEQ(a8));SEQ(a1;SEQ(a6;a7;a3;SEQ(a7;a6));a2));a5);PO>",
                "<PI;AND(AND(a1;a1;AND(a4;a4;SEQ(a2;a3;SEQ(a5));a4));a4;a3;SEQ(SEQ(a1;SEQ(a3;a5);XOR(v[1,5];XOR(v[1,5];a8;a8);a7));a6));PO>",
                "<PI;SEQ(SEQ(a1;a3;a5);a4;AND(a1;AND(XOR(v[1,5];a8)));XOR(v[1,5];SEQ(a8;a8;a1);a7));PO>",
                "<PI;SEQ(a3;a2;AND(a5;a6;SEQ(XOR(v[1,3];a5);XOR(v[1,3];a7)));AND(AND(XOR(v[1,5];SEQ(a8;a6;a8);XOR(v[1,1];a7;a7));a5);SEQ(a5;a4);XOR(v[1,2];AND(SEQ(a7;a6);a7;a7;AND(a5;a6;a5;a5)))));PO>",
                "<PI;SEQ(a2;AND(a6;a4;a6);SEQ(a5;a4);AND(a1;AND(a4;a5;a6;a6);XOR(v[1,2];a7;XOR(v[1,5];a8;a7))));PO>",
                "<PI;AND(SEQ(SEQ(XOR(v[1,4];SEQ(a3;a1;a7;a6));a3;XOR(v[1,5];XOR(v[1,5];a5;a8);a7);a3);a2;a3);AND(AND(a1;a1;a2);XOR(v[1,1];a3;a2);a4;SEQ(a1));SEQ(a3;a1;a6;a6));PO>",
                "<PI;AND(SEQ(a1;a4;a6;SEQ(XOR(v[1,5];XOR(v[1,5];a8;a7);AND(a7;a7;a7))));a3;SEQ(a2;a5;a3);a3);PO>",
                "<PI;AND(XOR(v[1,1];a4);a2;AND(a3;SEQ(SEQ(AND(a2;a4);SEQ(a3;a2;a5;a2);XOR(v[1,5];a8;a7));AND(a4;XOR(v[1,2];a3);a2;AND(a5));a4);a4));PO>",
                "<PI;SEQ(a2;a3;SEQ(AND(a5;a5;a6;a6);a5;XOR(v[1,5];a8;a7);a1);XOR(v[1,4];a1));PO>",
                "<PI;SEQ(AND(a4);SEQ(SEQ(a3;a2);AND(SEQ(AND(a5);XOR(v[1,5];a8;a7);SEQ(a6;a4;a1);AND(a5;a3;a5)));SEQ(SEQ(XOR(v[1,5];a8;a7);a5);a1;a4);a3);a1);PO>",
                "<PI;AND(a2;AND(a2;AND(a2;a2;a4;a1));SEQ(a2;a6;a4;XOR(v[1,5];a8;a7));a3);PO>",
                "<PI;SEQ(a1;a6;a3;SEQ(a1;SEQ(a6);XOR(v[1,1];SEQ(a7;XOR(v[1,1];a7;a6));XOR(v[1,5];a8;a7));a2));PO>",
                "<PI;AND(SEQ(a1;a5;SEQ(a4;SEQ(AND(a4;a4);SEQ(a3;a1);AND(a6;a5));SEQ(a4;XOR(v[1,5];a8;a7)));a2);SEQ(a3;AND(a2;a2;AND(a2;a2;a1;XOR(v[1,5];a8)))));PO>",
                "<PI;SEQ(a1;AND(a6;a3;SEQ(a5;a3;XOR(v[1,5];a8;a7)));a4;XOR(v[1,1];XOR(v[1,1];a6)));PO>",
                "<PI;SEQ(a1;a3;a6;AND(a6;SEQ(SEQ(a5;a4;a4;a5);XOR(v[1,5];a8;AND(a7;a7;a7)));a2));PO>",
                "<PI;SEQ(a2;a6;a3;AND(SEQ(a6;a2;a5);AND(AND(AND(a1;a1);a1;a1;XOR(v[1,5];a8;a7));a5;a1;a2)));PO>",
                "<PI;SEQ(SEQ(AND(a2);a3;a1;a5);a5;XOR(v[1,2];a7;XOR(v[1,5];AND(a8;AND(a8;a8;a8;a8));a7)));PO>",
                "<PI;SEQ(AND(a1);SEQ(a4;AND(a2;XOR(v[1,2];a1;XOR(v[1,3];a2));XOR(v[1,5];SEQ(a8;a1;a6);a7));a1;a6);a1;AND(AND(a1)));PO>",
                "<PI;SEQ(a3;SEQ(a1;a6;SEQ(XOR(v[1,5];SEQ(a8);a7);a5;a6));a1);PO>",
                "<PI;SEQ(SEQ(a1;a6;a4;SEQ(a2;a5;a3));SEQ(AND(a2;XOR(v[1,4];XOR(v[1,4];a7;a7);a6);a1);XOR(v[1,5];a8;XOR(v[1,1];a7;a7));AND(a4;a5)));PO>",
                "<PI;SEQ(a1;a6;a4;XOR(v[1,5];a8;AND(AND(a7;a7;a7);a7)));PO>",
                "<PI;AND(AND(a1;AND(a3);a1);SEQ(AND(SEQ(a1);a1;AND(a3;SEQ(a1;a6;a3;a5);SEQ(a3);XOR(v[1,5];a4));a2);SEQ(a3;a2;SEQ(a6;a5;a1);a1);SEQ(a4;SEQ(AND(a4;a3);a3;a3;a3));AND(XOR(v[1,5];SEQ(a8;a4);a7)));SEQ(a3);a3);PO>",
                "<PI;SEQ(a3;SEQ(a2;a5;AND(a5;a1;SEQ(SEQ(a5;a4);XOR(v[1,5];a8;a7));SEQ(a2;a6;a3));a5);XOR(v[1,1];a7);a1);PO>",
                "<PI;SEQ(AND(AND(a2;SEQ(a3;a2);SEQ(a2;SEQ(a4;a5);a3;AND(a6;a5;a5;a5)));SEQ(a3;a1);a4);SEQ(SEQ(a4;a2);a5);a6;XOR(v[1,5];a8;a7));PO>",
                "<PI;AND(SEQ(a1;SEQ(a5);a4;SEQ(a5;XOR(v[1,5];a8;AND(a7;a7;a7));a1));a1);PO>",
                "<PI;SEQ(a1;a4;AND(a6;AND(a5;AND(a6);XOR(v[1,5];a8;a7));SEQ(a6;SEQ(XOR(v[1,1];a7);AND(a6;a1;a2;a4);a6;a2);AND(a6;XOR(v[1,2];a6;a4);AND(a5;a2;a2);XOR(v[1,4];a5))));AND(AND(a6;a5;a5;a6);SEQ(XOR(v[1,2];XOR(v[1,2];a7);a6);a6;a2;a4)));PO>",
                "<PI;SEQ(a3;a2;a5;XOR(v[1,5];a8;AND(a7)));PO>",
                "<PI;SEQ(SEQ(a3;a1;a6;a6);AND(a2;AND(a1;a5;a2));SEQ(XOR(v[1,5];AND(a8);a7);a4));PO>",
                "<PI;AND(AND(AND(AND(SEQ(a2;a4;a5));a4));SEQ(SEQ(a1;a3);SEQ(SEQ(a6;a1);SEQ(a5;a4;XOR(v[1,5];a8;a7));a4;a5);a6;a6);a3);PO>",
                "<PI;SEQ(SEQ(a1;a5;a4;a6);AND(a4;a1;AND(AND(a1;AND(a4;a2;a4);a3;XOR(v[1,1];a7;a1));AND(a1);a4));a5;AND(SEQ(a4;a6);SEQ(XOR(v[1,5];a8);a3);a1;AND(XOR(v[1,5];a8;a7);SEQ(a2;a5;AND(a5));a6;a6)));PO>",
                "<PI;SEQ(a2;a6;a3;AND(a4;SEQ(SEQ(XOR(v[1,5];a8;a7);SEQ(a5;a5;a2;a1);a6;a5);AND(XOR(v[1,1];a3;a5);AND(a1;a3);a6;a3);a3;SEQ(a6;AND(a6;a5;a4);a5;AND(a6;a6;a1;a2)));a1;a2));PO>",
                "<PI;SEQ(a3;SEQ(a2;AND(a6);SEQ(a6;AND(a6));XOR(v[1,5];a8;a7)));PO>",
                "<PI;SEQ(SEQ(a3;a2;a5;XOR(v[1,5];AND(a8);a7));a5;a6;SEQ(AND(a4)));PO>",
                "<PI;AND(a3;SEQ(SEQ(a1;AND(a3;AND(a4;a6;a4);a6;a6);a3);a5;a1;SEQ(XOR(v[1,5];a8;SEQ(a7;a3));a3;a1;a5));a1;a2);PO>",
                "<PI;SEQ(a2;a4;a5;XOR(v[1,5];a8;a7));PO>",
                "<PI;SEQ(a4;a1;a5;XOR(v[1,3];AND(AND(a7;a7;a7;a7));AND(a3;a6;a5;XOR(v[1,5];a8;a7))));PO>"
            };

            foreach (var s in array)
            {
                var g = s.ParseBpmGenome();
                var so = new BpmnFitness().Evaluate(g) as BpmSolution;
                Debug.WriteLine(so.EvaluationTime + "\t" + s);
            }
        }

        [TestMethod]
        public void FitnessTestCrazy3()
        {
            ModelHelper.SetBpmModel(new BpmModel.Builder(ModelHelper.GetBpmModel()).SetAlpha(0.0).Build());
            var fitness = new BpmnFitness(false);

            var genome0 = "<PI;AND(SEQ(a2;a6);SEQ(a3;XOR(v[1,5];a8;a7)));PO>".ParseBpmGenome();
            var f0 = fitness.Evaluate(genome0) as BpmSolution;
            Debug.WriteLine(f0.Fitness);

            var genome1 =
                "<PI;SEQ(a4;AND(a2;SEQ(a2;XOR(v[1,3];a5);SEQ(AND(a4);a4;a1;XOR(v[1,2];a7)));a3;AND(XOR(v[1,4];a1;SEQ(a2;a5;a3));a1;XOR(v[1,5];AND(a8;a8;a1;a8);a7);a2));AND(SEQ(a1);a1;XOR(v[1,1];a2));a2);PO>"
                    .ParseBpmGenome();
            var f1 = fitness.Evaluate(genome1) as BpmSolution;
            Debug.WriteLine(f1.Fitness);

            Assert.IsTrue(f0.Fitness > f1.Fitness);
        }

        [TestMethod]
        public void FitnessTestCrazy4()
        {
            ModelHelper.SetBpmModel(new BpmModel.Builder(ModelHelper.GetBpmModel()).SetAlpha(0.0).Build());
            var fitness = new BpmnFitness(false);

            var genome0 = "<PI;AND(SEQ(a2;a6);SEQ(a3;a7));PO>".ParseBpmGenome();
            var f0 = fitness.Evaluate(genome0) as BpmSolution;
            Debug.WriteLine(f0.Fitness);
        }

        [TestMethod]
        public void FitnessTest()
        {
            ModelHelper.SetBpmModel(new BpmModel.Builder(ModelHelper.GetBpmModel()).SetAlpha(0.0).Build());
            var fitness = new BpmnFitness(false);

            // start
            var genome1 = "<PI;SEQ(a1;SEQ(SEQ(a3;a1;a6);a1);XOR(v[1,5];a8;a7);a1);PO>".ParseBpmGenome();
            var f1 = fitness.Evaluate(genome1) as BpmSolution;
            Debug.WriteLine(f1.Fitness);

            // move a1 down
            var genome2 = "<PI;SEQ(SEQ(SEQ(a1;a3;a1;a6);a1);XOR(v[1,5];a8;a7);a1);PO>".ParseBpmGenome();
            var f2 = fitness.Evaluate(genome2) as BpmSolution;
            Debug.WriteLine(f2.Fitness);

            // remove SEQ
            var genome3 = "<PI;SEQ(SEQ(a1;a3;a1;a6;a1);XOR(v[1,5];a8;a7);a1);PO>".ParseBpmGenome();
            var f3 = fitness.Evaluate(genome3) as BpmSolution;
            Debug.WriteLine(f3.Fitness);

            // remove SEQ
            var genome4 = "<PI;SEQ(a1;a3;a1;a6;a1;XOR(v[1,5];a8;a7);a1);PO>".ParseBpmGenome();
            var f4 = fitness.Evaluate(genome4) as BpmSolution;
            Debug.WriteLine(f4.Fitness);

            // remove a1
            var genome5 = "<PI;SEQ(a1;a3;a1;a6;a1;XOR(v[1,5];a8;a7));PO>".ParseBpmGenome();
            var f5 = fitness.Evaluate(genome5) as BpmSolution;
            Debug.WriteLine(f5.Fitness);

            // remove a1
            var genome6 = "<PI;SEQ(a1;a3;a1;a6;XOR(v[1,5];a8;a7));PO>".ParseBpmGenome();
            var f6 = fitness.Evaluate(genome6) as BpmSolution;
            Debug.WriteLine(f6.Fitness);

            // remove a1
            var genome7 = "<PI;SEQ(a1;a3;a6;XOR(v[1,5];a8;a7));PO>".ParseBpmGenome();
            var f7 = fitness.Evaluate(genome7) as BpmSolution;
            Debug.WriteLine(f7.Fitness);

            Assert.IsTrue(f1.Fitness == f2.Fitness);
            Assert.IsTrue(f2.Fitness == f3.Fitness);
            Assert.IsTrue(f3.Fitness == f4.Fitness);
            Assert.IsTrue(f4.Fitness < f5.Fitness);
            Assert.IsTrue(f5.Fitness < f6.Fitness);
            Assert.IsTrue(f6.Fitness < f7.Fitness);
        }

        [TestMethod]
        public void XmlTestComplex()
        {
            var genome =
                "<PI;SEQ(a3;XOR(v[1,3];AND(a2;a2));SEQ(XOR(v[1,5];AND(AND(a8;a8;a7);SEQ(a8;a1;a6));a7);a2;a6;a3));PO>"
                    .ParseBpmGenome();
            var xml = XmlHelper.BpmnToXml(genome);
            Debug.WriteLine(xml.InnerXml);
        }

        [TestMethod]
        public void XmlTestFull()
        {
            var genome = "<PI;SEQ(a2;a3;a6;XOR(v[1,5];a8;a7));PO>".ParseBpmGenome();
            var xml = XmlHelper.BpmnToXml(genome);
            Debug.WriteLine(xml.InnerXml);
        }

        [TestMethod]
        public void XmlTestAnd()
        {
            var genome = "<PI;AND(a2;a3);PO>".ParseBpmGenome();
            var xml = XmlHelper.BpmnToXml(genome);
            Debug.WriteLine(xml.InnerXml);
        }

        [TestMethod]
        public void XmlTest()
        {
            var genome = "<PI;SEQ(a2;a3;a6;XOR(v[1,5];a8;a7));PO>".ParseBpmGenome();
            var xml = XmlHelper.BpmnToXml(genome);
            Debug.WriteLine(xml.InnerXml);
        }

        [TestMethod]
        public void XmlCrazyTest()
        {
            var genome1 =
                "<PI;SEQ(a4;AND(a2;SEQ(a2;XOR(v[1,3];a5);SEQ(AND(a4);a4;a1;XOR(v[1,2];a7)));a3;AND(XOR(v[1,4];a1;SEQ(a2;a5;a3));a1;XOR(v[1,5];AND(a8;a8;a1;a8);a7);a2));AND(SEQ(a1);a1;XOR(v[1,1];a2));a2);PO>"
                    .ParseBpmGenome();
            var xml = XmlHelper.BpmnToXml(genome1);
            Debug.WriteLine(xml.InnerXml);
        }
    }
}