using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabuSearchProductionScheduling.Classes;

namespace TabuSearchProductionScheduling.Helpers
{
    public class CSVHelper
    {
        public DataSource Read()
        {
            return null;
        }

        public List<Customer> ReadDataFromCustomers(string filePath)
        {
            var customers = new List<Customer>();
            using (StreamReader reader = new StreamReader(filePath))
            {
                try
                {
                    string line;
                    reader.ReadLine();
                    while ((line = reader.ReadLine()) != null)
                    {
                        var values = line.Split(',');
                        if (values.Length >= 2 && float.TryParse(values[0], out float weight))
                        {
                            var customerName = values[1];
                            var customer = new Customer(weight, customerName);
                            customers.Add(customer);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
    
            return customers;
        }

        public List<WorkCenter> ReadDataFromWorkCenters(string filePath)
        {
            var workCenters = new List<WorkCenter>();
            using (StreamReader reader = new StreamReader(filePath))
            {
                try
                {
                    string line;
                    reader.ReadLine();
                    while ((line = reader.ReadLine()) != null)
                    {
                        var values = line.Split(',');
                        if (values.Length >= 5)
                        {
                            var code = values[0]; var name = values[1]; 
                            if (float.TryParse(values[2], out float capacity) && double.TryParse(values[3], out double oeeTarget) && double.TryParse(values[4], out double timeEfficient))
                            {
                                var workCenter = new WorkCenter(code: code, 
                                                                name: name, 
                                                                capacity: capacity, 
                                                                oEETarget: oeeTarget, 
                                                                timeEfficient: timeEfficient);
                                workCenters.Add(workCenter);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return workCenters;
        }

        public List<Product> ReadDataFromProducts(string filePath)
        {
            var products = new List<Product>();
            using (StreamReader reader = new StreamReader(filePath))
            {
                try
                {
                    string line;
                    reader.ReadLine();
                    while ((line = reader.ReadLine()) != null)
                    {
                        var values = line.Split(",");
                        if (values.Length >= 4)
                        {
                            var name = values[0]; var reference = values[1]; var mold = values[3];
                            if (double.TryParse(values[2], out double salePrice))
                            {
                                var product = new Product(name: name, 
                                                          reference: reference,  
                                                          salePrice: salePrice,
                                                          mold: mold);
                                products.Add(product);
                            } 
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
 
            return products;
        }

        public List<BOM> ReadDataFromBOMs(string filePath, List<Product> products, List<WorkCenter> workCenters)
        {
            var BOMs = new List<BOM>();
            var productDictionary = products.ToDictionary(x => x.Reference, y => y);
            var workCenterDictionary = workCenters.ToDictionary(x => x.Code, y => y);
            using (StreamReader reader = new StreamReader(filePath))
            {
                try
                {
                    string line;
                    reader.ReadLine();
                    while ((line = reader.ReadLine()) != null)
                    {
                        var values = line.Split(',');
                        if (values.Length >= 8)
                        {
                            var reference = values[1]; var operationCode = values[8]; var workCenterCode = values[9]; 
                            if (productDictionary.ContainsKey(reference) && workCenterDictionary.ContainsKey(workCenterCode))
                            {
                                var product = productDictionary[reference];
                                var workCenter = workCenterDictionary[workCenterCode];

                                var alternativeWorkCenters = new List<WorkCenter>();
                                var alternativeWorkCenterCodes = values[10].Split(" ");
                                if (alternativeWorkCenterCodes?.Length > 0)
                                {
                                    foreach (var alternativeCode in alternativeWorkCenterCodes)
                                    {
                                        if (workCenterDictionary.ContainsKey(alternativeCode))
                                            alternativeWorkCenters.Add(workCenterDictionary[alternativeCode]);
                                    }
                                }

                                if (float.TryParse(values[12], out float expectedDuration))
                                {
                                    var operation = new Operation(code: operationCode,
                                                                  workCenter: workCenter,
                                                                  alternativeWorkCenters: alternativeWorkCenters,
                                                                  manualDuration: expectedDuration);

                                    var bom = new BOM(product, operation);
                                    BOMs.Add(bom);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return BOMs;
        }

        public List<Order> ReadDataFromOrders(string filePath, List<Customer> customers, List<Product> products)
        {
            var orders = new List<Order>();
            var customerDictionary = customers.ToDictionary(x => x.Name, y => y);
            var productDictionary = products.ToDictionary(x => x.Reference, y => y);
            using (StreamReader reader = new StreamReader(filePath))
            {
                try
                {
                    string line;
                    reader.ReadLine();
                    while ((line = reader.ReadLine()) != null)
                    {
                        var values = line.Split(",");
                        if (values.Length >= 5)
                        {
                            var dueDate = DateTime.ParseExact(values[0], "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                            var releaseDate = DateTime.ParseExact(values[5], "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                            Customer customer = null;
                            if (customerDictionary.ContainsKey(values[2]))
                                customer = customerDictionary[values[2]];

                            Product product = null;
                            if (productDictionary.ContainsKey(values[3]))
                                product = productDictionary[values[3]];

                            if (customer != null && product != null && double.TryParse(values[4], out double quantity))
                            {
                                var order = new Order(dueDate: dueDate, 
                                                      releaseDate: releaseDate, 
                                                      customer: customer,
                                                      product: product,
                                                      quantity: quantity);
                                orders.Add(order);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            
            return orders;
        }
    }
}
