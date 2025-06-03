using System.Text;

namespace SLAPScheduling.Algorithm.DifferentialEvolutions
{
    public class Individual
    {
        public List<int> Elements { get; set; } = new List<int>();
        public double Fitness { get; set; }

        public Individual()
        {
            Elements = new List<int>();
        }

        public double Evaluate(List<ReceiptSublot> receiptSubLots, Dictionary<int, Location> locationDictionary, int numberOfReceiptSubLots)
        {
            // Placeholder for fitness evaluation (to be implemented based on SLAP objectives)
            // Example: Sum of distances from locations to I/O point
            var solution = this.GetSolution();
            Fitness = solution.CalculateObjectValue(receiptSubLots, locationDictionary);
            return Fitness;
        }

        public override string ToString()
        {
            return string.Join(", ", Elements);
        }
    }
}
