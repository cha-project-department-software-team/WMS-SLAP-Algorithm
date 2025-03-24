using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabuSearchProductionScheduling.Classes
{
    public class BOM
    {
        public Product Product { get; set; }
        public Operation Operation { get; set; }

        public BOM (Product product, Operation operation)
        {
            Operation = operation;
            Product = product;
            Product.BOM = this;
        }
    }
}
