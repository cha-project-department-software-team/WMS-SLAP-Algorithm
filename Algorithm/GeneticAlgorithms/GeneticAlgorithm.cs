using Newtonsoft.Json;
using System.Diagnostics;

namespace SLAPScheduling.Algorithm.GeneticAlgorithms
{
    public class GeneticAlgorithms
    {
        private static int terminate => 1000;
        private static int maxGeneration => 300;
        private static int elitism => 15;
        private static float crossoverProbability => 0.95f;
        private static float mutationProbability => 0.05f;
        private List<ReceiptSublot> receiptSubLots { get; set; }
        private Dictionary<int, Location> locationDictionary { get; set; }
        private Solution bestSolution { get; set; }
        private double bestObjectValue { get; set; }
        private List<double> bestObjectValues { get; set; }
        private List<(double Time, double ObjectValue)> timeChangeObjectValues { get; set; }
        private List<(int SolutionCount, double ObjectValue)> solutionChangeObjectValues { get; set; }
        private int numberOfReceiptSubLots { get; set; }
        private int numberOfEvaluatedSolutions { get; set; }
        private Stopwatch sw { get; set; }

        #region Constructor
        public GeneticAlgorithms(List<ReceiptSublot> receiptSubLots, List<Location> availableLocations)
        {
            this.bestObjectValues = new List<double>();
            this.timeChangeObjectValues = new List<(double Time, double ObjectValue)>();
            this.solutionChangeObjectValues = new List<(int SolutionCount, double ObjectValue)>();
            this.bestSolution = new Solution();
            this.numberOfReceiptSubLots = receiptSubLots.Count;
            this.numberOfEvaluatedSolutions = 0;
            this.sw = new Stopwatch();

            this.receiptSubLots = new List<ReceiptSublot>();
            this.locationDictionary = new Dictionary<int, Location>();
            if (availableLocations?.Count > 0)
            {
                this.locationDictionary = Enumerable.Range(0, availableLocations.Count).ToDictionary(index => index, index => availableLocations[index]);
                this.receiptSubLots = Enumerable.Range(0, availableLocations.Count).Select(index =>
                {
                    return index < receiptSubLots.Count ? receiptSubLots[index] : new ReceiptSublot();
                }).ToList();
            }
        }

        #endregion

        #region Implementation
        public List<Location> Implement()
        {
            this.sw.Start();

            int populationSize = this.locationDictionary.Count;
            Population population = CreateChromosomes(maxGeneration, populationSize);

            var elite = new Elite(elitism);
            var crossover = new Crossover(crossoverProbability)
            {
                CrossoverType = CrossoverType.DoublePointOrdered
            };

            var mutate = new SwapMutate(mutationProbability);
            var ga = new GeneticAlgorithm(population, CalculateFitness);

            ga.OnGenerationComplete += GA_OnGenerationComplete;
            ga.OnRunComplete += GA_OnRunComplete;
            ga.Operators.Add(elite);
            ga.Operators.Add(crossover);
            ga.Operators.Add(mutate);

            ga.Run(Terminate);

            this.sw.Stop();

            using (TextWriter writer = File.CreateText(@"C:\Users\AnhTu\Master Subjects\Luan van Thac si\Document\SchedulingResult.json"))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(writer, timeChangeObjectValues);
            }

            var optimalLocations = bestSolution.GetLocations(locationDictionary);
            return optimalLocations?.Count() > 0 ? optimalLocations.ToList() : new List<Location>();
        }

        public Population CreateChromosomes(int maxGeneration, int populationSize)
        {
            var population = new Population();
            for (var p = 0; p < maxGeneration; p++)
            {
                var chromosome = new Chromosome();
                for (var g = 1; g <= populationSize; g++)
                {
                    chromosome.Genes.Add(new Gene(g));
                }

                chromosome.Genes.ShuffleFast();
                population.Solutions.Add(chromosome);
            }

            return population;
        }

        public bool Terminate(Population population, int currentGeneration, long currentEvaluation)
        {
            return currentGeneration > terminate;
        }

        private void GA_OnRunComplete(object sender, GaEventArgs e)
        {
            var bestChromosome = e.Population.GetTop(1)[0];
            this.bestSolution = bestChromosome.GetSolution();
            this.bestObjectValue = this.bestSolution.CalculateObjectValue(receiptSubLots, locationDictionary);
            this.bestObjectValues.Add(this.bestObjectValue);

            this.timeChangeObjectValues.Add((this.sw.Elapsed.TotalSeconds, this.bestObjectValue));
            this.solutionChangeObjectValues.Add((this.numberOfEvaluatedSolutions, this.bestObjectValue));
        }

        private void GA_OnGenerationComplete(object sender, GaEventArgs e)
        {
            var bestChromosome = e.Population.GetTop(1)[0];
            this.bestSolution = bestChromosome.GetSolution();
            this.bestObjectValue = this.bestSolution.CalculateObjectValue(receiptSubLots, locationDictionary);
            this.bestObjectValues.Add(this.bestObjectValue);

            this.timeChangeObjectValues.Add((this.sw.Elapsed.TotalSeconds, this.bestObjectValue));
            this.solutionChangeObjectValues.Add((this.numberOfEvaluatedSolutions, this.bestObjectValue));
        }

        public double CalculateFitness(Chromosome chromosome)
        {
            this.numberOfEvaluatedSolutions += 1;

            var solution = chromosome.GetSolution();
            double objectValue = solution.CalculateObjectValue(receiptSubLots, locationDictionary);
            double fitness = 1 - objectValue / (numberOfReceiptSubLots * 1000);
            return fitness;
        }

        #endregion
    }
}
