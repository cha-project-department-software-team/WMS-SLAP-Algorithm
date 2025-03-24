using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabuSearchProductionScheduling.Classes
{
    public enum ProductCategory
    {
        NapBanCau
    }

    public enum ProductType
    {
        StorableProduct
    }

    public class Product
    {
        public string Name { get; set; }
        public string Reference { get; set; }
        public double SalePrice { get; set; }
        public string Mold { get; set; }
        public BOM BOM { get; set; }

        public Product(string name, string reference, double salePrice, string mold)
        {
            Name = name;
            Reference = reference;
            SalePrice = salePrice;
            Mold = mold;
        }
    }
}
