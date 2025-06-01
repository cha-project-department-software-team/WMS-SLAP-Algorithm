using System.Diagnostics;

namespace SLAPScheduling.Algorithm.DifferentialEvolutions
{
    public class DifferentialEvolution
    {
        #region Properties
        public Population Population { get; set; }
        public Parameters Parameters { get; set; }
        private List<ReceiptSublot> receiptSubLots { get; set; }
        private Dictionary<int, Location> locationDictionary { get; set; }
        private int numberOfReceiptSubLots { get; set; }


        #endregion

        #region Events
        public event EventHandler OnSolutionComplete;
        public event EventHandler OnGenerationComplete;
        public event EventHandler OnRunComplete;
        #endregion

        public DifferentialEvolution(Population population, Parameters parameters, List<ReceiptSublot> receiptSubLots, List<Location> availableLocations)
        {
            Population = population;
            Parameters = parameters;

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

        public void Run(int maxEvaluations, double CR, double F)
        {
            Population.Evaluate(receiptSubLots, locationDictionary, numberOfReceiptSubLots);

            while (maxEvaluations > 0)
            {
                List<Individual> newGeneration = new List<Individual>();

                foreach (var original in Population.Solutions)
                {
                    // generate unique random numbers
                    List<int> randomValues = RandomGenerator.GenerateRandom(3, 0, Population.Solutions.Count);
                    int a = randomValues[0];
                    int b = randomValues[1];
                    int c = randomValues[2];

                    // choose random individuals (agents) from population
                    Individual individual1 = Population.Solutions[a];
                    Individual individual2 = Population.Solutions[b];
                    Individual individual3 = Population.Solutions[c];

                    int i = 0;
                    int R = RandomGenerator.Instance.Random.Next(Population.Solutions.Count);
                    Individual candidate = new Individual();
                    foreach (var originalElement in original.Elements)
                    {
                        double ri = RandomGenerator.Instance.Random.NextDouble();
                        if (ri < CR || i == R)
                        {
                            // simple mutation
                            int newElement = (int)(individual1.Elements[i] + F * (individual2.Elements[i] - individual3.Elements[i]));

                            if (CheckIfWithinDomain(newElement, Parameters) && !candidate.Elements.Contains(newElement))
                            {
                                candidate.Elements.Add(newElement);
                            }
                            else if (!candidate.Elements.Contains(originalElement))
                            {
                                candidate.Elements.Add(originalElement);
                            }
                            else if (FindFallbackValue(candidate.Elements, Parameters, out int nonExistingNumber))
                            {
                                candidate.Elements.Add(nonExistingNumber);
                            }
                        }
                        else
                        {
                            if (!candidate.Elements.Contains(originalElement))
                            {
                                candidate.Elements.Add(originalElement);
                            }
                            else if (FindFallbackValue(candidate.Elements, Parameters, out int nonExistingNumber))
                            {
                                candidate.Elements.Add(nonExistingNumber);
                            }
                        }

                        i++;
                    }

                    var candidateFitness = candidate.Evaluate(receiptSubLots, locationDictionary, numberOfReceiptSubLots);
                    var originalFitness = original.Evaluate(receiptSubLots, locationDictionary, numberOfReceiptSubLots);

                    if (candidateFitness < originalFitness)
                    {
                        newGeneration.Add(candidate);
                    }
                    else
                    {
                        newGeneration.Add(original);
                    }

                    OnSolutionComplete.Invoke(1, new EventArgs());
                }

                // switch populations
                Population.Solutions = newGeneration;
                maxEvaluations--;

                OnGenerationComplete.Invoke(Population, new EventArgs());
            }

            OnRunComplete.Invoke(Population, new EventArgs());
        }

        private bool CheckIfWithinDomain(double newElement, Parameters parameters)
        {
            return newElement >= parameters.Domain.Item1 && newElement < parameters.Domain.Item2;
        }

        private bool FindFallbackValue(List<int> used, Parameters parameters, out int nonExistingNumber)
        {
            for (int x = (int)parameters.Domain.Item1; x <= parameters.Domain.Item2; x++)
            {
                if (!used.Contains(x))
                {
                    nonExistingNumber = x;
                    return true;
                }
            }

            nonExistingNumber = -1;
            return false;
        }
    }
}
