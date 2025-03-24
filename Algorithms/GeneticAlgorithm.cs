using GAF;
using GAF.Extensions;
using GAF.Operators;
using System.Data;
using TabuSearchProductionScheduling.Classes;
using TabuSearchProductionScheduling.Planning;

namespace TabuSearchProductionScheduling.Algorithms
{
    public class GeneticAlgorithms
    {
        private static int terminate => 100;
        private static int maxGeneration => 100;
        private static int elitism => 15;
        private static  float crossoverProbability => 0.9f;
        private static  float mutationProbability => 0.05f;
        private List<WorkOrder> workOrders { get; set; }
        private DateTime firstDateStart { get; set; }
        private List<double> bestValues { get; set; }
        public double BestObjectValue { get; set; }
        public List<int> BestSolution { get; set; }


        public GeneticAlgorithms(List<WorkOrder> workOrders, DateTime firstDateStart)
        {
            this.workOrders = workOrders;
            this.firstDateStart = firstDateStart;
            this.BestSolution = Enumerable.Range(0, workOrders.Count).ToList();
            this.bestValues = new List<double>();
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
        public double CalculateObjectValue(Chromosome chromosome)
        {
            double objectValue = 0;
            List<int> solution = chromosome.Genes.Select(x => (int)x.RealValue).ToList();

            WorkOrderPlanning planning = new WorkOrderPlanning(firstDateStart);
            foreach (int job in solution)
            {
                WorkOrder workOrder = workOrders.FirstOrDefault(x => x.Id == job);
                if (workOrder != null)
                {
                    workOrder = planning.Planning(workOrder);
                    if (workOrder.EndDate > workOrder.DueDate)
                        objectValue += workOrder.Priority;
                }
            }

            return objectValue;
        }
        public double CalculateFitness(Chromosome chromosome)
        {
            double fitnessValue = CalculateObjectValue(chromosome);
            return 1 - fitnessValue / 396.8;
        }

        public bool Terminate(Population population, int currentGeneration, long currentEvaluation)
        {
            return currentGeneration > terminate;
        }

        public List<WorkOrder> GetScheduledWorkOrders()
        {
            int populationSize = this.workOrders.Count;
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

            var scheduledWorkOrders = new List<WorkOrder>();
            var bestSolution = this.BestSolution;
            var workOrderDictionary = this.workOrders.ToDictionary(x => x.Id, x => x);
            foreach (var index in bestSolution)
            {
                if (workOrderDictionary.ContainsKey(index))
                    scheduledWorkOrders.Add(workOrderDictionary[index]);
            }

            return scheduledWorkOrders;
        }

        private void GA_OnRunComplete(object sender, GaEventArgs e)
        {
            var bestChromosome = e.Population.GetTop(1)[0];
            BestSolution = bestChromosome.Genes.Select(x => (int)x.RealValue).ToList();
            BestObjectValue = CalculateObjectValue(bestChromosome);
        }

        private void GA_OnGenerationComplete(object sender, GaEventArgs e)
        {
            var fittest = e.Population.GetTop(1)[0];
            var objectValue = CalculateObjectValue(fittest);
            bestValues.Add(objectValue);
        }
    }
}
