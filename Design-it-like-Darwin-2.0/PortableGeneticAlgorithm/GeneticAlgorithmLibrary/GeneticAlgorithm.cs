using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PortableGeneticAlgorithm.Analytics;
using PortableGeneticAlgorithm.GeneticAlgorithmLibrary;
using PortableGeneticAlgorithm.Interfaces;

namespace PortableGeneticAlgorithm
{
    public class GeneticAlgorithm
    {
        #region Constructor

        private GeneticAlgorithm()
        {
            Status.Initialisation = true;
        }

        public void SetModel(GePrModel model)
        {
            _model = model;

            Population = new Population(_model.GetInitialGenome());
            CrossoverProbability = _model.GetCrossoverProbability();
            MutationProbability = _model.GetMutationProbability();
            TimeEvolving = TimeSpan.Zero;

            Status.Initialisation = false;
            Status.Ready = true;
        }

        #endregion //Constructor

        #region Fields

        private readonly object _lock = new object();

        public readonly Status Status = new Status
        {
            Ready = false,
            Running = false,
            StopRequested = false,
            Stopped = false
        };

        #endregion //Fields

        #region Constants

        /// <summary>
        ///     The default crossover probability.
        /// </summary>
        public const float DefaultCrossoverProbability = 0f;

        /// <summary>
        ///     The default mutation probability.
        /// </summary>
        public const float DefaultMutationProbability = 1f;

        #endregion

        #region Properties

        internal GePrModel _model;
        public IGenome BestGenome => Population.BestGenome;
        public IPopulation Population { get; set; }
        private double CrossoverProbability { get; set; }
        private double MutationProbability { get; set; }
        public TimeSpan TimeEvolving { get; set; }
        private static readonly GeneticAlgorithm _instance = new GeneticAlgorithm();

        public static GeneticAlgorithm Instance()
        {
            return _instance;
        }

        #endregion //Properties

        #region Events

        /// <summary>
        ///     Occurs when generation is finished.
        /// </summary>
        public event EventHandler GenerationRan;

        /// <summary>
        ///     Occurs when termination is reached.
        /// </summary>
        public event EventHandler TerminationReached;

        #endregion //Events

        #region Methods

        public static void StartInNewTask()
        {
            Instance().Status.Ready = false;
            Instance().Status.Running = true;

            Task.Run(new Action(_instance.Start));
        }

        private void Start()
        {
            lock (_instance._lock)
            {
                // State = GeneticAlgorithmState.Started;
                var startDateTime = DateTime.Now;
                _instance.Population.CreateInitialGeneration(_instance.Population.InitialGenome);
                _instance.TimeEvolving = DateTime.Now - startDateTime;
            }

            _instance.Resume();
        }

        public void RequestStop()
        {
            Status.Running = true;
            Status.StopRequested = true;
        }

        /// <summary>
        ///     Resumes the last evolution of the genetic algorithm.
        /// </summary>
        public void Resume()
        {
            lock (_lock)
            {
                Status.StopRequested = false;
            }

            if (Population.NumberOfGenerations == 0)
                throw new InvalidOperationException(
                    "You can't resume a GeneticAlgorithm algorithm wich is not started yet.");

            if (Population.NumberOfGenerations > 1)
                if (_model.GetTermination().HasReached(this))
                    throw new InvalidOperationException(
                        $"Termination ({_model.GetTermination()}) is already reached. Please, specify a new termination or extend the current one.");

            if (EndCurrentGeneration().Result)
                return;

            var terminationConditionReached = false;

            do
            {
                if (Status.StopRequested)
                {
                    Status.Running = false;
                    Status.Stopped = true;
                    break;
                }

                var startDateTime = DateTime.Now;
                // Adjust mutation and crossoverprobability
                MutationProbability -= 1 / _model.GetMaximumNumberOfGenerations();
                CrossoverProbability += 1 / _model.GetMaximumNumberOfGenerations();
                terminationConditionReached = EvolveOneGeneration();
                TimeEvolving += DateTime.Now - startDateTime;
            } while (!terminationConditionReached);

            Status.Running = false;
            Status.Stopped = true;

            if (_model.AnalyticsEnabled())
                _model.GetAnalytics().ConsumeSolution(new FinishedSolution());
        }

        /// <summary>
        ///     Evolve one generation.
        /// </summary>
        /// <returns>True if termination has been reached, otherwise false.</returns>
        private bool EvolveOneGeneration()
        {
            var oldGenerationGenomes = Population.CurrentGeneration.Genomes.ToList();
            var newGenerationGenomes = _model.GetGenerationEvolver().EvolveGeneration(oldGenerationGenomes);
            Population.CreateNewGeneration(newGenerationGenomes);
            return EndCurrentGeneration().Result;
        }

        /// <summary>
        ///     Ends the current generation.
        /// </summary>
        /// <returns><c>true</c>, if current generation was ended, <c>false</c> otherwise.</returns>
        private async Task<bool> EndCurrentGeneration()
        {
            await EvaluateFitness();
            Population.EndCurrentGeneration();

            if (GenerationRan != null)
                GenerationRan(this, EventArgs.Empty);

            if (_model.GetTermination().HasReached(this))
            {
                //State = GeneticAlgorithmState.TerminationReached;

                if (TerminationReached != null)
                    TerminationReached(this, EventArgs.Empty);

                return true;
            }

            return false;
        }

        /// <summary>
        ///     Evaluates the fitness of each genome in the current generation.
        /// </summary>
        private async Task EvaluateFitness()
        {
            try
            {
                var genomesWithoutFitness =
                    Population.CurrentGeneration.Genomes.Where(c => !c.Fitness.HasValue).ToList();

                if (_model.GetUseParalell())
                {
                    var tasks = new List<Task<Solution>>();

                    foreach (var genome in genomesWithoutFitness)
                    {
                        var t = new Task<Solution>(g =>
                        {
                            var fitness = (IFitness) Activator.CreateInstance(_model.GetFitness());
                            return fitness.Evaluate((IGenome) g);
                        }, genome);

                        tasks.Add(t);
                        t.Start();
                    }

                    await Task.WhenAll(tasks.ToArray());

                    var allSolutions = new List<Solution>();
                    for (var i = 0; i < genomesWithoutFitness.Count; i++)
                    {
                        var s = tasks[i].Result;
                        allSolutions.Add(s);
                        genomesWithoutFitness[i].Fitness = s.Fitness;
                    }

                    if (_model.AnalyticsEnabled())
                        _model.GetAnalytics().ConsumeSolutions(allSolutions);
                }
                else
                {
                    var allSolutions = new List<Solution>();
                    for (var i = 0; i < genomesWithoutFitness.Count; i++)
                    {
                        var currentGenome = genomesWithoutFitness[i];

                        var fitness = (IFitness) Activator.CreateInstance(_model.GetFitness());

                        var solution = fitness.Evaluate(currentGenome);
                        allSolutions.Add(solution);
                        currentGenome.Fitness = solution.Fitness;
                    }

                    if (_model.AnalyticsEnabled())
                        _model.GetAnalytics().ConsumeSolutions(allSolutions);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            Population.CurrentGeneration.Genomes =
                Population.CurrentGeneration.Genomes.OrderByDescending(c => c.Fitness.Value).ToList();
        }

        public GePrModel GetModel()
        {
            return _model;
        }

        #endregion //Methods
    }
}