using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabuSearchProductionScheduling.Classes
{
    public class Customer
    {
        public float Weight { get; set; }
        public string Name { get; set; }

        public Customer(float weight, string name)
        {
            Weight = weight;
            Name = name;
        }
    }
}
