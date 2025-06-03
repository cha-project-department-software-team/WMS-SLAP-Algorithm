using SLAPScheduling.Algorithm.DifferentialEvolutions;
using Population = SLAPScheduling.Algorithm.DifferentialEvolutions.Population;

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
        this.numberOfReceiptSubLots = receiptSubLots.Count;
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
                // Generate unique random indices
                List<int> randomIndices = RandomGenerator.Instance.GenerateRandom(3, 0, Population.Solutions.Count, Population.Solutions.IndexOf(original));
                Individual individual1 = Population.Solutions[randomIndices[0]];
                Individual individual2 = Population.Solutions[randomIndices[1]];
                Individual individual3 = Population.Solutions[randomIndices[2]];

                Individual candidate = new Individual();
                int R = RandomGenerator.Instance.Random.Next(Parameters.Dimensions);
                for (int i = 0; i < Parameters.Dimensions; i++)
                {
                    double ri = RandomGenerator.Instance.Random.NextDouble();
                    if (ri < CR || i == R)
                    {
                        // Improved mutation: Ensure value stays within domain
                        double mutation = individual1.Elements[i] + F * (individual2.Elements[i] - individual3.Elements[i]);
                        int newElement = (int)Math.Max(Parameters.Domain.Item1, Math.Min(Parameters.Domain.Item2 - 1, mutation));
                        if (!candidate.Elements.Contains(newElement))
                        {
                            candidate.Elements.Add(newElement);
                        }
                        else
                        {
                            candidate.Elements.Add(original.Elements[i]); // Keep original if duplicate
                        }
                    }
                    else
                    {
                        candidate.Elements.Add(original.Elements[i]);
                    }
                }

                // Ensure candidate has exact number of elements
                while (candidate.Elements.Count < Parameters.Dimensions)
                {
                    int newElement = RandomGenerator.Instance.Random.Next((int)Parameters.Domain.Item1, (int)Parameters.Domain.Item2);
                    if (!candidate.Elements.Contains(newElement))
                    {
                        candidate.Elements.Add(newElement);
                    }
                }

                var candidateFitness = candidate.Evaluate(receiptSubLots, locationDictionary, numberOfReceiptSubLots);
                var originalFitness = original.Evaluate(receiptSubLots, locationDictionary, numberOfReceiptSubLots);

                newGeneration.Add(candidateFitness < originalFitness ? candidate : original);
                OnSolutionComplete?.Invoke(1, new EventArgs());
            }

            // Maintain diversity
            Population.Solutions = newGeneration;
            if (newGeneration.Distinct().Count() < Population.Solutions.Count * 0.1) // If diversity drops below 10%
            {
                Population.InitializeRandom(Parameters.Dimensions, (int)Parameters.Domain.Item1, (int)Parameters.Domain.Item2);
                Population.Evaluate(receiptSubLots, locationDictionary, numberOfReceiptSubLots);
            }

            maxEvaluations--;
            OnGenerationComplete?.Invoke(Population, new EventArgs());
        }

        OnRunComplete?.Invoke(Population, new EventArgs());
    }
}