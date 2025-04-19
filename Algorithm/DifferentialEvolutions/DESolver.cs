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
        private List<(double Time, double ObjectValue)> changeBestObjectValues { get; set; }
        private Stopwatch sw { get; set; }

        public DESolver()
        {
            this.Parameters = new Parameters();
            this.bestObjectValues = new List<double>();
            this.changeBestObjectValues = new List<(double Time, double ObjectValue)>();
            this.bestSolution = new Solution();
            this.sw = new Stopwatch();
        }

        public List<Location> Implement(List<ReceiptSublot> receiptSubLots, List<Location> availableLocations)
        {
            sw.Start();

            this.Parameters = new Parameters
            {
                Domain = new Tuple<double, double>(0, availableLocations.Count),
                F = 0.5,
                CR = 0.9,
                AgentsCount = 100,
                Dimensions = availableLocations.Count,
                Iterations = 1000
            };

            Population population = CreatePopulation();
            DifferentialEvolution DE = new DifferentialEvolution(population, Parameters, receiptSubLots, availableLocations);

            DE.OnGenerationComplete += DE_OnGenerationComplete;
            DE.OnRunComplete += DE_OnRunComplete;

            DE.Run(Parameters.Iterations, Parameters.CR, Parameters.F);

            sw.Stop();

            using (TextWriter writer = File.CreateText(@"C:\Users\AnhTu\Master Subjects\Luan van Thac si\Document\SchedulingResult.json"))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(writer, changeBestObjectValues);
            }

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
                    double randomNumber = RandomGenerator.GetDoubleRangeRandomNumber(Parameters.Domain.Item1, Parameters.Domain.Item2);
                    individual.Elements.Add(randomNumber);
                }

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

            this.changeBestObjectValues.Add((this.sw.Elapsed.TotalSeconds, this.bestObjectValue));

            Console.WriteLine("Run complete");
        }

        private void DE_OnGenerationComplete(object sender, EventArgs e)
        {
            Population population = (Population)sender;
            Individual bestIndividual = population.GetBest();

            this.bestSolution = bestIndividual.GetSolution();
            this.bestObjectValue = bestIndividual.Fitness;
            this.bestObjectValues.Add(this.bestObjectValue);

            this.changeBestObjectValues.Add((this.sw.Elapsed.TotalSeconds, this.bestObjectValue));

            Console.Write("Fitness: " + Math.Round(bestIndividual.Fitness, 4));
            Console.WriteLine(string.Format(" - coordinate: ({0})", bestIndividual.ToString()));
        }
    }
}
