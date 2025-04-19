namespace SLAPScheduling.Algorithm.DifferentialEvolutions
{
    public class Population
    {
        public List<Individual> Solutions { get; set; }
        public double AverageFitness { get { return Solutions.Average(x => x.Fitness); } }
        public double MaximumFitness { get { return Solutions.Max(x => x.Fitness); } }
        public double MinimumFitness { get { return Solutions.Min(x => x.Fitness); } }
        public double TotalSumFitness { get { return Solutions.Sum(x => x.Fitness); } }

        public Population(List<Individual> individuals)
        {
            Solutions = individuals;
        }

        public Individual GetBest()
        {
            return Solutions.OrderBy(x => x.Fitness).FirstOrDefault();
        }

        public void Evaluate(List<ReceiptSublot> receiptSubLots, Dictionary<int, Location> locationDictionary, int numberOfReceiptSubLots)
        {
            foreach (var item in Solutions)
            {
                item.Evaluate(receiptSubLots, locationDictionary, numberOfReceiptSubLots);
            }
        }
    }
}
