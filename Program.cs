using System.Data;
using System.Diagnostics;
using TabuSearchProductionScheduling.Algorithms;
using TabuSearchProductionScheduling.Classes;
using TabuSearchProductionScheduling.Helpers;

public class Program
{
    public static void Main(string[] args)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        var csvPath = @"C:\Users\AnhTu\Master Subjects\Đồ án 1\TabuSearchProductionScheduling\CSV";
        var firstDateStart = DateTime.ParseExact("05/02/2024 07:00", "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);

        CSVHelper csvHelper = new CSVHelper();
        var customers = csvHelper.ReadDataFromCustomers(csvPath + @"\Customers.csv");
        var workcenters = csvHelper.ReadDataFromWorkCenters(csvPath +  @"\WorkCenters.csv");
        var products = csvHelper.ReadDataFromProducts(csvPath + @"\Products.csv");
        var boms = csvHelper.ReadDataFromBOMs(csvPath + @"\BOMs.csv", products, workcenters);
        var orders = csvHelper.ReadDataFromOrders(csvPath + @"\Orders.csv", customers, products);

        DataSource dataSource = new DataSource(bOMs: boms,
                                               customers: customers,
                                               products: products,
                                               workCenters: workcenters,
                                               orders: orders);

        double bestValue = 0; var bestSolution = new List<int>();
        List<WorkOrder> workOrders = dataSource.GetWorkOrders();
        var workOrderGroups = workOrders.GroupBy(x => x.WorkCenter.Code).ToDictionary(x => x.Key, y => y.ToList());
        foreach (var kvp in workOrderGroups)
        {
            var workCenter = kvp.Key; var workOrderGroup = kvp.Value;
            for (int i = 0; i < workOrderGroup.Count; i++)
            {
                workOrderGroup[i].Id = i;
            }

            if (workOrderGroup?.Count > 1)
            {
                var listWorkOrders = Enumerable.Range(0, workOrderGroup.Count).Select(x => workOrderGroup[x]).ToList();
                //TabuSearch tabuSearch = new TabuSearch(listWorkOrders, firstDateStart);
                //var scheduledWorkOrders = tabuSearch.GetScheduledWorkOrders();

                GeneticAlgorithms GA = new GeneticAlgorithms(listWorkOrders, firstDateStart);
                var scheduledWorkOrders = GA.GetScheduledWorkOrders();

                bestValue = GA.BestObjectValue; bestSolution = GA.BestSolution;
            }
        }

        sw.Stop();
        var seconds = sw.Elapsed.TotalSeconds;

        Console.ReadKey();
    }
}
