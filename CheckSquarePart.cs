using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FindPlannedDate;

namespace CheckSquarePart
{
    public class SquarePart
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int MinimumQuantity { get; set; }
        public string isRequestAdd { get; set; }
        public DateTime expectedAddDate { get; set; }

        public SquarePart (int id, string name, int quantity, int minimumQuantity, string isRequestAdd, DateTime expectedAddDate)
        {
            Id = id;
            Name = name;
            Quantity = quantity;
            MinimumQuantity = minimumQuantity;
            this.isRequestAdd = isRequestAdd;
            this.expectedAddDate = expectedAddDate;
        }
    }

    public class SquarePartOnWork
    {
        public int Id { get; set; }
        public int Priority { get; set; }
        public string Device { get; set; }
        public string Work { get; set; }
        public DateTime DueDate { get; set; }
        public int ExcutionTime { get; set; }
        public List<string> listNamePart { get; set; }
        public List<int> listSequencePart { get; set; }
        public List<int> listQuantityPart { get; set; }

        public SquarePartOnWork()
        {

        }

        public SquarePartOnWork(int id, int priority, string device, string work, DateTime dueDate, int excutionTime, List<string> listNamePart, List<int> listSequencePart, List<int> listQuantityPart)
        {
            Id = id;
            Priority = priority;
            Device = device;
            Work = work;
            DueDate = dueDate;
            ExcutionTime = excutionTime;
            this.listNamePart = listNamePart;
            this.listSequencePart = listSequencePart;
            this.listQuantityPart = listQuantityPart;
        }
    }

    public class CheckSquarePart
    {
        public static List<SquarePart> getListSquarePart (DataTable squarePartTable)
        {
            var listSquarePart = new List<SquarePart>();
            foreach (DataRow row in squarePartTable.Rows)
            {
                int id = Convert.ToInt32(row["No"]);
                string part = (string)row["Part"];
                int quantity = Convert.ToInt32(row["Quantity"]);
                int minimumQuantity = Convert.ToInt32(row["MinimumQuantity"]);
                string isAdd = (string)row["IsAddition"];
                DateTime expectedDate = DateTime.ParseExact((string)row["ExpectedPartDate"], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var squarePart = new SquarePart(id, part, quantity, minimumQuantity, isAdd, expectedDate);
                listSquarePart.Add(squarePart);
            }

            foreach (SquarePart part in listSquarePart)
            {
                Console.WriteLine(part.Id.ToString() + " " + part.Name + " " + part.Quantity.ToString() + " " + part.MinimumQuantity.ToString() + " " + part.isRequestAdd.ToString() + " " + part.expectedAddDate.ToString());
            }

            return listSquarePart;
        }

        public static List<SquarePartOnWork> getListSquarePartOnWork(DataTable squarePartOnWorkTable) 
        {
            var listSquarePartOnWork = new List<SquarePartOnWork>();
            var squarePartOnWork = new SquarePartOnWork();
            var listNamePart = new List<string>();
            var listSequencePart = new List<int>();
            var listQuantityPart = new List<int>();
            int i = 0;

            foreach (DataRow row in squarePartOnWorkTable.Rows)
            {
                if (row["No"] != "")
                {
                    if (i != 0)
                    {
                        squarePartOnWork.listNamePart = listNamePart;
                        squarePartOnWork.listSequencePart = listSequencePart;
                        squarePartOnWork.listQuantityPart = listQuantityPart;
                        listNamePart = new List<string>();
                        listSequencePart= new List<int>();
                        listQuantityPart = new List<int>();
                        listSquarePartOnWork.Add(squarePartOnWork);
                    }
                    
                    squarePartOnWork = new SquarePartOnWork();
                    squarePartOnWork.Id = Convert.ToInt32(row["No"]);
                    squarePartOnWork.Priority = Convert.ToInt32(row["Priority"]);
                    squarePartOnWork.Device = (string)row["Device"];
                    squarePartOnWork.Work = (string)row["Work"];
                    squarePartOnWork.DueDate = DateTime.ParseExact((string)row["DueDate"], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    squarePartOnWork.ExcutionTime = Convert.ToInt32(row["ExecutionTime"]);
                    listNamePart.Add((string)row["PartList"]);
                    listSequencePart.Add(Convert.ToInt32(row["SequencePartList"]));
                    listQuantityPart.Add(Convert.ToInt32(row["QuantityPart"]));
                }
                else
                {
                    listNamePart.Add((string)row["PartList"]);
                    listSequencePart.Add(Convert.ToInt32(row["SequencePartList"]));
                    listQuantityPart.Add(Convert.ToInt32(row["QuantityPart"]));
                }
                i++;
            }
            squarePartOnWork.listNamePart = listNamePart;
            squarePartOnWork.listSequencePart = listSequencePart;
            squarePartOnWork.listQuantityPart = listQuantityPart;
            listSquarePartOnWork.Add(squarePartOnWork);

            Console.WriteLine();
            foreach (var squarepart in listSquarePartOnWork)
            {
                Console.WriteLine("---------------------------");
                Console.WriteLine($"{squarepart.Id} {squarepart.Priority} {squarepart.Device} {squarepart.Work} {squarepart.DueDate} {squarepart.ExcutionTime}");
                for (int j = 0; j < squarepart.listSequencePart.Count; j++)
                {
                    Console.WriteLine(squarepart.listNamePart[j].ToString() + " - " + squarepart.listSequencePart[j].ToString() + " - " + squarepart.listQuantityPart[j].ToString());
                }
            }

            return listSquarePartOnWork;
        }

        public static Dictionary<SquarePartOnWork, List<int>> checkSquarePartAvailable(List<SquarePart> listSquarePart, List<SquarePartOnWork> listSquarePartOnWork)
        {
            
            Dictionary<SquarePartOnWork, List<int>> dictJobRemove = new Dictionary<SquarePartOnWork, List<int>>();

            Console.WriteLine();
            foreach (SquarePartOnWork squarePartOnWork in listSquarePartOnWork)
            {
                var listSequencePartTemp = squarePartOnWork.listSequencePart;
                var listSequencePartLack = new List<int>();
                bool check = true;
                
                if (squarePartOnWork.listSequencePart[0] != 0)
                {
                    int j = 0;
                    for (int i = 0; i < squarePartOnWork.listSequencePart.Count; i++)
                    {
                        var quantityPartInventory = listSquarePart[squarePartOnWork.listSequencePart[i] - 1].Quantity;
                        var quantityPartOnWork = squarePartOnWork.listQuantityPart[i];
                        var newQuantityPart = quantityPartInventory - quantityPartOnWork;
                        if (newQuantityPart >= 0)
                        {
                            Console.WriteLine($"The job {squarePartOnWork.Id} - the sequence part {squarePartOnWork.listSequencePart[i]} consumed: {quantityPartOnWork}. And the new quantity part inventory: {newQuantityPart}");
                        }

                        if (quantityPartInventory < quantityPartOnWork)
                        {
                            check = false;
                            j++;
                            listSequencePartLack.Add(squarePartOnWork.listSequencePart[i]);
                        }
                    }

                    Console.WriteLine(j);
                    if (check)
                    {
                        for (int i = 0; i < squarePartOnWork.listSequencePart.Count; i++)
                        {
                            var quantityPartInventory = listSquarePart[squarePartOnWork.listSequencePart[i] - 1].Quantity;
                            var quantityPartOnWork = squarePartOnWork.listQuantityPart[i];
                            var newQuantityPart = quantityPartInventory - quantityPartOnWork;
                            if (newQuantityPart >= 0)
                            {
                                listSquarePart[squarePartOnWork.listSequencePart[i] - 1].Quantity = newQuantityPart;
                                Console.WriteLine($"The job {squarePartOnWork.Id} - the sequence part {squarePartOnWork.listSequencePart[i]} consumed: {quantityPartOnWork}. And the new quantity part inventory: {newQuantityPart}");
                            }
                        }
                    }
                    else
                    {
                        dictJobRemove.Add(squarePartOnWork, listSequencePartLack);
                        Console.WriteLine($"The job {squarePartOnWork.Id} is removed");
                        foreach (int item in listSequencePartLack)
                        {
                            Console.WriteLine($"Because this job {squarePartOnWork.Id} is lacked: the sequence part {item}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"The job {squarePartOnWork.Id} doesn't need to use the square part");
                    Console.WriteLine("*************************************************************");
                    continue;
                }
                Console.WriteLine("*************************************************************");
            }
            return dictJobRemove;
        }

        public static List<SquarePartOnWork> getListWorkAvailable(DataTable squarePartTable, DataTable squarePartOnWorkTable)
        {
            List<SquarePart> listSquarePart = getListSquarePart(squarePartTable);
            List<SquarePartOnWork> listSquarePartOnWork = getListSquarePartOnWork(squarePartOnWorkTable);
            Dictionary<SquarePartOnWork, List<int>> dictJobRemove = checkSquarePartAvailable(listSquarePart, listSquarePartOnWork);
            var mondayOfThisWeek = DateTime.ParseExact("13/02/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var saturdayOfThisWeek = DateTime.ParseExact("18/02/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture);


            foreach (SquarePartOnWork squarePartOnWork in dictJobRemove.Keys)
            {
                foreach(int sequence in dictJobRemove[squarePartOnWork])
                {
                    DateTime expectedDate = listSquarePart[sequence - 1].expectedAddDate;
                    Console.WriteLine($"The expected additional square part {sequence} in job {squarePartOnWork.Id} is: {expectedDate}");
                    if (NewFindPlannedDate.isInRange(mondayOfThisWeek, saturdayOfThisWeek, expectedDate) == false)
                    {
                        listSquarePartOnWork.Remove(squarePartOnWork);
                        Console.WriteLine($"The job {squarePartOnWork.Id} is removed from a list");
                    }
                }
            }

            Console.WriteLine();
            foreach (var squarepart in listSquarePartOnWork)
            {
                Console.WriteLine($"{squarepart.Id} {squarepart.Priority} {squarepart.Device} {squarepart.Work} {squarepart.DueDate} {squarepart.ExcutionTime}");
            }

            return listSquarePartOnWork;
        }
    }
}
