using PortableGeneticAlgorithm.Analytics;
using PortableGeneticAlgorithm.Interfaces;
using SQLite.CodeFirst;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using MoreLinq;

namespace Bpm
{
    /// <summary>
    ///     Provides functionality to read the streaming log file
    /// </summary>
    public sealed class BpmAnalytics : IAnalytics
    {
        public class BpmSolutionMap : EntityTypeConfiguration<BpmSolution>
        {
            public BpmSolutionMap()
            {
                // Primary Key
                this.HasKey(t => t.Id);

                // Properties
                this.Property(t => t.EvaluationTime)
                    .IsRequired();
                this.Property(t => t.ValidGenome)
                    .IsRequired();
                this.Property(t => t.Fitness)
                    .IsRequired();
                this.Property(t => t.ActivityList)
                    .IsRequired();
                this.Property(t => t.EvaluationTime)
                    .IsRequired();
                this.Property(t => t.Generation)
                    .IsRequired();
                this.Property(t => t.MueP)
                    .IsRequired();
                this.Property(t => t.MueNpv)
                    .IsRequired();
                this.Property(t => t.Sigma2P)
                    .IsRequired();
                this.Property(t => t.Sigma2Npv)
                    .IsRequired();
                this.Property(t => t.Process)
                    .IsRequired();

                // Table & Column Mappings
                this.ToTable("BpmSolutions");
                this.Property(t => t.Id).HasColumnName("Id");
                this.Property(t => t.ValidGenome).HasColumnName("ValidGenome");
                this.Property(t => t.Fitness).HasColumnName("Fitness");
                this.Property(t => t.ActivityList).HasColumnName("ActivityList");
                this.Property(t => t.EvaluationTime).HasColumnName("EvaluationTime");
                this.Property(t => t.Generation).HasColumnName("Generation");
                this.Property(t => t.MueP).HasColumnName("MueP");
                this.Property(t => t.MueNpv).HasColumnName("MueNpv");
                this.Property(t => t.Sigma2P).HasColumnName("Sigma2P");
                this.Property(t => t.Sigma2Npv).HasColumnName("Sigma2Npv");
                this.Property(t => t.Process).HasColumnName("Process");
            }
        }

        private class AnalyticsContext : DbContext
        {
            public DbSet<BpmSolution> BpmSolution { get; set; }

            public AnalyticsContext()
            : base("BpmSolutionsDb")
            {
                Configuration.ProxyCreationEnabled = true;
                Configuration.LazyLoadingEnabled = true;
            }

            protected override void OnModelCreating(DbModelBuilder modelBuilder)
            {
                var initializer = new SqliteDropCreateDatabaseAlways<AnalyticsContext>(modelBuilder);
                Database.SetInitializer(initializer);

                modelBuilder.Configurations.Add(new BpmSolutionMap());
            }
        }

        private bool _isFinished;
        private static BpmAnalytics _instance = new BpmAnalytics();


        public static BpmAnalytics Instance()
        {
            return _instance;
        }

        public void Clear()
        {
            var ctx = new AnalyticsContext();
            ctx.BpmSolution.RemoveRange(ctx.BpmSolution.ToArray());
        }

        /// <summary>
        ///     Creates an Analytics object noticing on the analytics file
        /// </summary>
        public BpmAnalytics()
        {
            _isFinished = false;
        }

        public BpmSolution BestSolution()
        {
            var ctx = new AnalyticsContext();
            BpmSolution s = ctx.BpmSolution
                .OrderByDescending(x => x.Fitness)
                .ToList()
                .Where(x => x.ValidGenome == true)
                .DefaultIfEmpty(null)
                .First();
            return s;
        }

        private void InsertSolution(BpmSolution s)
        {
            var ctx = new AnalyticsContext();
            ctx.BpmSolution.Add(s);
            ctx.SaveChanges();
        }

        public void ConsumeSolution(PortableGeneticAlgorithm.Analytics.Solution solution)
        {
            if (solution is FinishedSolution)
            {
                _isFinished = true;
                return;
            }
            else
            {
                ConsumeSolutions(new List<PortableGeneticAlgorithm.Analytics.Solution>() { solution });
            }
        }


        public List<BpmSolution> GetTopSolutions(int count)
        {
            if (count < 1)
            {
                return new List<BpmSolution>() { BestSolution() };
            }

            var ctx = new AnalyticsContext();
            List<BpmSolution> list = ctx.BpmSolution
                .OrderByDescending(x => x.Fitness)
                .ToList()
                .Where(x => x.ValidGenome == true)
                .DistinctBy(x => x.Process)
                .Take(count)
                .DefaultIfEmpty(null)
                .ToList();
            return list;
        }

        public void ConsumeSolutions(List<PortableGeneticAlgorithm.Analytics.Solution> allSolutions)
        {
            if (allSolutions.Any(x => x is FinishedSolution))
            {
                _isFinished = true;

                allSolutions.RemoveAll(x => x is FinishedSolution);
            }

            foreach (var s in allSolutions)
            {
                if (s is FinishedSolution)
                {
                    _isFinished = true;
                    return;
                }

                InsertSolution(s as BpmSolution);
            }
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

        public List<double> MinFitness()
        {
            var ctx = new AnalyticsContext();
            return ctx.BpmSolution.GroupBy(x => x.Generation).Select(y => y.Min(z => z.Fitness)).ToList();
        }

        public List<double> MaxFitness()
        {
            var ctx = new AnalyticsContext();
            // möglicherweise wir dhier eie generation verloren gehen ohne gültiges genom
            return ctx.BpmSolution.GroupBy(x => x.Generation).Select(y => y.Max(z => z.Fitness)).ToList();
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

            var valid = groups.Select(y => y.Count(z => z.ValidGenome == true)).ToList();
            var all = groups.Select(y => y.Count(z => true)).ToList();

            var percentage = new List<double>();

            for (int i = 0; i < Math.Min(valid.Count, all.Count); i++)
            {
                percentage.Add((double)(valid[i]) / all[i]);
            }

            return percentage;
        }
    }
}