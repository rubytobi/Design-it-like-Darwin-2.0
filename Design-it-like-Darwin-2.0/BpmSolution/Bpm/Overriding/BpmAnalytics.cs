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
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using MoreLinq;
using PortableGeneticAlgorithm.Analytics;
using PortableGeneticAlgorithm.Interfaces;
using SQLite.CodeFirst;

namespace Bpm
{
    /// <summary>
    ///     Provides functionality to read the streaming log file
    /// </summary>
    public sealed class BpmAnalytics : IAnalytics
    {
        private static readonly BpmAnalytics _instance = new BpmAnalytics();

        private bool _isFinished;

        /// <summary>
        ///     Creates an Analytics object noticing on the analytics file
        /// </summary>
        public BpmAnalytics()
        {
            _isFinished = false;
        }

        public void ConsumeSolution(Solution solution)
        {
            if (solution is FinishedSolution)
                _isFinished = true;
            else
                ConsumeSolutions(new List<Solution> {solution});
        }

        public void ConsumeSolutions(List<Solution> allSolutions)
        {
            if (allSolutions.Any(x => x is FinishedSolution))
            {
                _isFinished = true;

                allSolutions.RemoveAll(x => x is FinishedSolution);
            }

            var list = new List<BpmSolution>();
            allSolutions.ForEach(x => list.Add(x as BpmSolution));

            InsertSolutions(list);
        }

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Solution Get(Guid id)
        {
            var ctx = new AnalyticsContext();
            return ctx.BpmSolution.FirstOrDefault(x => x.Id.Equals(id.ToString()));
        }


        public static BpmAnalytics Instance()
        {
            return _instance;
        }

        public void Clear()
        {
            var ctx = new AnalyticsContext();
            ctx.BpmSolution.RemoveRange(ctx.BpmSolution.ToArray());
            ctx.SaveChanges();
        }

        public BpmSolution BestSolution()
        {
            var ctx = new AnalyticsContext();
            var s = ctx.BpmSolution
                .OrderByDescending(x => x.Fitness)
                .ToList()
                .Where(x => x.ValidGenome)
                .DefaultIfEmpty(null)
                .First();
            return s;
        }

        private void InsertSolutions(IEnumerable<BpmSolution> s)
        {
            var ctx = new AnalyticsContext();
            ctx.BpmSolution.AddRange(s);
            ctx.SaveChangesAsync();
        }

        public List<BpmSolution> GetTopSolutions(int count)
        {
            if (count < 1)
                return new List<BpmSolution> {BestSolution()};

            var ctx = new AnalyticsContext();
            var list = ctx.BpmSolution
                .OrderByDescending(x => x.Fitness)
                .ToList()
                .Where(x => x.ValidGenome)
                .DistinctBy(x => x.Process)
                .Take(count)
                .DefaultIfEmpty(null)
                .ToList();
            return list;
        }

        public bool isFinished()
        {
            return _isFinished;
        }

        public IEnumerable<BpmSolution> GetAll()
        {
            var ctx = new AnalyticsContext();
            return ctx.BpmSolution.Select(x => x).ToList();
        }

        public List<double> MinFitnessValidOnly()
        {
            var ctx = new AnalyticsContext();
            return ctx.BpmSolution.Where(x => x.ValidGenome)
                .GroupBy(x => x.Generation)
                .Select(y => y.Min(z => z.Fitness))
                .ToList();
        }

        public List<double> MinFitness()
        {
            var ctx = new AnalyticsContext();
            return ctx.BpmSolution.GroupBy(x => x.Generation).Select(y => y.Min(z => z.Fitness)).ToList();
        }

        public List<double> MaxFitnessValidOnly()
        {
            var ctx = new AnalyticsContext();
            return ctx.BpmSolution.Where(x => x.ValidGenome)
                .GroupBy(x => x.Generation)
                .Select(y => y.Max(z => z.Fitness))
                .ToList();
        }

        public List<double> MaxFitness()
        {
            var ctx = new AnalyticsContext();
            return ctx.BpmSolution.GroupBy(x => x.Generation).Select(y => y.Max(z => z.Fitness)).ToList();
        }

        public List<double> AvgFitnessValidOnly()
        {
            var ctx = new AnalyticsContext();
            return ctx.BpmSolution.Where(x => x.ValidGenome)
                .GroupBy(x => x.Generation)
                .Select(y => y.Average(z => z.Fitness))
                .ToList();
        }

        public List<double> AvgFitness()
        {
            var ctx = new AnalyticsContext();
            return ctx.BpmSolution.GroupBy(x => x.Generation).Select(y => y.Average(z => z.Fitness)).ToList();
        }

        public List<double> AvgRuntime()
        {
            var ctx = new AnalyticsContext();
            return ctx.BpmSolution.GroupBy(x => x.Generation).Select(y => y.Average(z => z.EvaluationTime)).ToList();
        }

        public List<double> MinRuntime()
        {
            var ctx = new AnalyticsContext();
            return ctx.BpmSolution.GroupBy(x => x.Generation).Select(y => y.Min(z => z.EvaluationTime)).ToList();
        }

        public List<double> MaxRuntime()
        {
            var ctx = new AnalyticsContext();
            return ctx.BpmSolution.GroupBy(x => x.Generation).Select(y => y.Max(z => z.EvaluationTime)).ToList();
        }

        public List<double> ValidGenomes()
        {
            var ctx = new AnalyticsContext();

            var groups = ctx.BpmSolution.GroupBy(x => x.Generation);

            var valid = groups.Select(y => y.Count(z => z.ValidGenome)).ToList();
            var all = groups.Select(y => y.Count(z => true)).ToList();

            var percentage = new List<double>();

            for (var i = 0; i < Math.Min(valid.Count, all.Count); i++)
                percentage.Add((double) valid[i] / all[i]);

            return percentage;
        }

        public class BpmSolutionMap : EntityTypeConfiguration<BpmSolution>
        {
            public BpmSolutionMap()
            {
                // Primary Key
                HasKey(t => t.Id);

                // Properties
                Property(t => t.EvaluationTime)
                    .IsRequired();
                Property(t => t.ValidGenome)
                    .IsRequired();
                Property(t => t.Fitness)
                    .IsRequired();
                Property(t => t.ActivityList)
                    .IsRequired();
                Property(t => t.EvaluationTime)
                    .IsRequired();
                Property(t => t.Generation)
                    .IsRequired();
                Property(t => t.MueP)
                    .IsRequired();
                Property(t => t.MueNpv)
                    .IsRequired();
                Property(t => t.Sigma2P)
                    .IsRequired();
                Property(t => t.Sigma2Npv)
                    .IsRequired();
                Property(t => t.Process)
                    .IsRequired();

                // Table & Column Mappings
                ToTable("BpmSolutions");
                Property(t => t.Id).HasColumnName("Id");
                Property(t => t.ValidGenome).HasColumnName("ValidGenome");
                Property(t => t.Fitness).HasColumnName("Fitness");
                Property(t => t.ActivityList).HasColumnName("ActivityList");
                Property(t => t.EvaluationTime).HasColumnName("EvaluationTime");
                Property(t => t.Generation).HasColumnName("Generation");
                Property(t => t.MueP).HasColumnName("MueP");
                Property(t => t.MueNpv).HasColumnName("MueNpv");
                Property(t => t.Sigma2P).HasColumnName("Sigma2P");
                Property(t => t.Sigma2Npv).HasColumnName("Sigma2Npv");
                Property(t => t.Process).HasColumnName("Process");
            }
        }

        private class AnalyticsContext : DbContext
        {
            public AnalyticsContext()
                : base("BpmSolutionsDb")
            {
                Configuration.ProxyCreationEnabled = true;
                Configuration.LazyLoadingEnabled = true;
            }

            public DbSet<BpmSolution> BpmSolution { get; set; }

            protected override void OnModelCreating(DbModelBuilder modelBuilder)
            {
                var initializer = new SqliteDropCreateDatabaseAlways<AnalyticsContext>(modelBuilder);
                Database.SetInitializer(initializer);

                modelBuilder.Configurations.Add(new BpmSolutionMap());
            }
        }
    }
}