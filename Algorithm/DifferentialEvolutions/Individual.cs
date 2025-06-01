using System.Text;

namespace SLAPScheduling.Algorithm.DifferentialEvolutions
{
    public class Individual
    {
        public double Fitness { get; set; }
        public List<int> Elements { get; set; }

        public Individual(List<int> Elements)
        {
            this.Elements = Elements;
        }

        public Individual()
        {
            Elements = new List<int>();
        }

        public double Evaluate(List<ReceiptSublot> receiptSubLots, Dictionary<int, Location> locationDictionary, int numberOfReceiptSubLots)
        {
            var solution = this.GetSolution();
            Fitness = solution.CalculateObjectValue(receiptSubLots, locationDictionary);
            return Fitness;
        }

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();
            foreach (var item in Elements)
            {
                s.Append(Math.Round((decimal)item, 4) + ", ");
            }

            s.Remove(s.Length - 2, 2);
            return s.ToString();
        }
    }
}
