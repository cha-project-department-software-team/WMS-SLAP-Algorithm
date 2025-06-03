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
            Parameters = new Parameters();
            bestObjectValues = new List<double>();
            timeChangeObjectValues = new List<(double Time, double ObjectValue)>();
            solutionChangeObjectValues = new List<(int SolutionCount, double ObjectValue)>();
            bestSolution = new Solution();
            numberOfEvaluatedSolutions = 0;
            sw = new Stopwatch();
        }

        public List<Location> Implement(List<ReceiptSublot> receiptSubLots, List<Location> availableLocations)
        {
            sw.Start();

            Parameters = new Parameters
            {
                Domain = new Tuple<double, double>(0, availableLocations.Count),
                F = 0.5,
                CR = 0.95, 
                AgentsCount = 300,
                Dimensions = availableLocations.Count,
                Iterations = 1000
            };

            Population population = CreatePopulation(availableLocations.Count);
            DifferentialEvolution DE = new DifferentialEvolution(population, Parameters, receiptSubLots, availableLocations);

            DE.OnSolutionComplete += DE_OnSolutionComplete;
            DE.OnGenerationComplete += DE_OnGenerationComplete;
            DE.OnRunComplete += DE_OnRunComplete;

            DE.Run(Parameters.Iterations, Parameters.CR, Parameters.F);

            sw.Stop();

            //using (TextWriter writer = File.CreateText(@"C:\Users\AnhTu\Master Subjects\Luan van Thac si\Document\Excel\DifferentialEvolutionSchedulingResult.json"))
            //{
            //    var serializer = new JsonSerializer();
            //    serializer.Serialize(writer, timeChangeObjectValues);
            //}

            var locationDictionary = Enumerable.Range(0, availableLocations.Count).ToDictionary(index => index, index => availableLocations[index]);
            var optimalLocations = bestSolution.GetLocations(locationDictionary);
            return optimalLocations?.Count() > 0 ? optimalLocations.ToList() : new List<Location>();
        }

        public Population CreatePopulation(int locationCount)
        {
            List<Individual> individuals = new List<Individual>();
            Random rand = RandomGenerator.Instance.Random;
            for (int i = 0; i < Parameters.AgentsCount; i++)
            {
                Individual individual = new Individual();
                for (int j = 0; j < Parameters.Dimensions; j++)
                {
                    int randomLocation = rand.Next(0, locationCount);
                    individual.Elements.Add(randomLocation);
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
            bestSolution = bestIndividual.GetSolution();
            bestObjectValue = bestIndividual.Fitness;
            bestObjectValues.Add(bestObjectValue);
            timeChangeObjectValues.Add((sw.Elapsed.TotalSeconds, bestObjectValue));
            solutionChangeObjectValues.Add((numberOfEvaluatedSolutions, bestObjectValue));
            Console.WriteLine("Run complete");
        }

        private void DE_OnGenerationComplete(object sender, EventArgs e)
        {
            Population population = (Population)sender;
            Individual bestIndividual = population.GetBest();
            bestSolution = bestIndividual.GetSolution();
            bestObjectValue = bestIndividual.Fitness;
            bestObjectValues.Add(bestObjectValue);
            timeChangeObjectValues.Add((sw.Elapsed.TotalSeconds, bestObjectValue));
            solutionChangeObjectValues.Add((numberOfEvaluatedSolutions, bestObjectValue));
            Console.WriteLine($"Fitness: {Math.Round(bestIndividual.Fitness, 4)} - Coordinate: {bestIndividual}");
        }

        private void DE_OnSolutionComplete(object sender, EventArgs e)
        {
            numberOfEvaluatedSolutions++;
        }
    }
}
