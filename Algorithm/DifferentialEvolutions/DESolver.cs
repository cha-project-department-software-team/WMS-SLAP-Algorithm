using Newtonsoft.Json;
using SLAPScheduling.Domain.AggregateModels.InventoryReceiptAggregate;
using System.Diagnostics;

namespace SLAPScheduling.Algorithm.DifferentialEvolutions
{
    public class DESolver
    {
        public Parameters Parameters { get; set; }
        private Solution bestSolution { get; set; }
        private double bestObjectValue { get; set; }
        private List<double> bestObjectValues { get; set; }
        private List<(double Time, double ObjectValue)> timeChangeObjectValues { get; set; }
        private List<(int SolutionCount, double ObjectValue)> solutionChangeObjectValues { get; set; }
        private int numberOfEvaluatedSolutions { get; set; }
        private Stopwatch sw { get; set; }

        public DESolver()
        {
            this.Parameters = new Parameters();
            this.bestObjectValues = new List<double>();
            this.timeChangeObjectValues = new List<(double Time, double ObjectValue)>();
            this.solutionChangeObjectValues = new List<(int SolutionCount, double ObjectValue)>();
            this.bestSolution = new Solution();
            this.numberOfEvaluatedSolutions = 0;
            this.sw = new Stopwatch();
        }

        public List<Location> Implement(List<ReceiptSublot> receiptSubLots, List<Location> availableLocations)
        {
            sw.Start();

            this.Parameters = new Parameters
            {
                Domain = new Tuple<double, double>(0, availableLocations.Count),
                F = 0.5,
                CR = 0.95,
                AgentsCount = 300,
                Dimensions = availableLocations.Count,
                Iterations = 1000
            };

            Population population = CreatePopulation();
            DifferentialEvolution DE = new DifferentialEvolution(population, Parameters, receiptSubLots, availableLocations);

            DE.OnSolutionComplete += DE_OnSolutionComplete;
            DE.OnGenerationComplete += DE_OnGenerationComplete;
            DE.OnRunComplete += DE_OnRunComplete;

            DE.Run(Parameters.Iterations, Parameters.CR, Parameters.F);

            sw.Stop();

            //using (TextWriter writer = File.CreateText(@"C:\Users\AnhTu\Master Subjects\Luan van Thac si\Document\SchedulingResult.json"))
            //{
            //    var serializer = new JsonSerializer();
            //    serializer.Serialize(writer, timeChangeObjectValues);
            //}

            var locationDictionary = Enumerable.Range(0, availableLocations.Count).ToDictionary(index => index, index => availableLocations[index]);
            var optimalLocations = bestSolution.GetLocations(locationDictionary);
            return optimalLocations?.Count() > 0 ? optimalLocations.ToList() : new List<Location>();
        }

        public Population CreatePopulation()
        {
            List<Individual> individuals = new List<Individual>();
            for (int i = 1; i < Parameters.AgentsCount; i++)
            {
                Individual individual = new Individual();

                for (int j = 0; j < Parameters.Dimensions; j++)
                {
                    //double randomNumber = RandomGenerator.GetDoubleRangeRandomNumber(Parameters.Domain.Item1, Parameters.Domain.Item2);
                    individual.Elements.Add(j);
                }

                individual.Elements.ShuffleFast();
                individuals.Add(individual);
            }

            return new Population(individuals);
        }

        private void DE_OnRunComplete(object sender, EventArgs e)
        {
            Population population = (Population)sender;
            Individual bestIndividual = population.GetBest();

            this.bestSolution = bestIndividual.GetSolution();
            this.bestObjectValue = bestIndividual.Fitness;
            this.bestObjectValues.Add(this.bestObjectValue);

            this.timeChangeObjectValues.Add((this.sw.Elapsed.TotalSeconds, this.bestObjectValue));
            this.solutionChangeObjectValues.Add((this.numberOfEvaluatedSolutions, this.bestObjectValue));

            Console.WriteLine("Run complete");
        }

        private void DE_OnGenerationComplete(object sender, EventArgs e)
        {
            Population population = (Population)sender;
            Individual bestIndividual = population.GetBest();

            this.bestSolution = bestIndividual.GetSolution();
            this.bestObjectValue = bestIndividual.Fitness;
            this.bestObjectValues.Add(this.bestObjectValue);

            this.timeChangeObjectValues.Add((this.sw.Elapsed.TotalSeconds, this.bestObjectValue));
            this.solutionChangeObjectValues.Add((this.numberOfEvaluatedSolutions, this.bestObjectValue));

            Console.Write("Fitness: " + Math.Round(bestIndividual.Fitness, 4));
            Console.WriteLine(string.Format(" - coordinate: ({0})", bestIndividual.ToString()));
        }

        private void DE_OnSolutionComplete(object sender, EventArgs e)
        {
            int evaluatedSolutions = (int)sender;
            this.numberOfEvaluatedSolutions += evaluatedSolutions;
        }
    }
}
