using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FindPlannedDate;

namespace ChecksparePart
{
    public class SparePart
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int MinimumQuantity { get; set; }
        public string isRequestAdd { get; set; }
        public DateTime expectedAddDate { get; set; }

        public SparePart (int id, string name, int quantity, int minimumQuantity, string isRequestAdd, DateTime expectedAddDate)
        {
            Id = id;
            Name = name;
            Quantity = quantity;
            MinimumQuantity = minimumQuantity;
            this.isRequestAdd = isRequestAdd;
            this.expectedAddDate = expectedAddDate;
        }
    }

    public class SparePartOnWork
    {
        public int? Id { get; set; }
        public int Priority { get; set; }
        public string Device { get; set; }
        public string Work { get; set; }
        public DateTime DueDate { get; set; }
        public int ExcutionTime { get; set; }
        public List<string> listNamePart { get; set; }
        public List<int> listSequencePart { get; set; }
        public List<int> listQuantityPart { get; set; }

        public SparePartOnWork()
        {

        }

        public SparePartOnWork(int id, int priority, string device, string work, DateTime dueDate, int excutionTime, List<string> listNamePart, List<int> listSequencePart, List<int> listQuantityPart)
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

    public class ChecksparePart
    {
        public static List<SparePart> getListsparePart (DataTable sparePartTable)
        {
            var listsparePart = new List<SparePart>();
            foreach (DataRow row in sparePartTable.Rows)
            {
                int id = Convert.ToInt32(row["No"]);
                string part = (string)row["Part"];
                int quantity = Convert.ToInt32(row["Quantity"]);
                int minimumQuantity = Convert.ToInt32(row["MinimumQuantity"]);
                string isAdd = (string)row["IsAddition"];
                DateTime expectedDate = DateTime.ParseExact((string)row["ExpectedPartDate"], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var sparePart = new SparePart(id, part, quantity, minimumQuantity, isAdd, expectedDate);
                listsparePart.Add(sparePart);
            }

            //foreach (sparePart part in listsparePart)
            //{
            //    Console.WriteLine(part.Id.ToString() + " " + part.Name + " " + part.Quantity.ToString() + " " + part.MinimumQuantity.ToString() + " " + part.isRequestAdd.ToString() + " " + part.expectedAddDate.ToString());
            //}

            return listsparePart;
        }

        public static List<SparePartOnWork> getListsparePartOnWork(DataTable sparePartOnWorkTable) 
        {
            var listsparePartOnWork = new List<SparePartOnWork>();
            var sparePartOnWork = new SparePartOnWork();
            var listNamePart = new List<string>();
            var listSequencePart = new List<int>();
            var listQuantityPart = new List<int>();
            int i = 0;

            foreach (DataRow row in sparePartOnWorkTable.Rows)
            {
                if (row["No"] != "")
                {
                    if (i != 0)
                    {
                        sparePartOnWork.listNamePart = listNamePart;
                        sparePartOnWork.listSequencePart = listSequencePart;
                        sparePartOnWork.listQuantityPart = listQuantityPart;
                        listNamePart = new List<string>();
                        listSequencePart= new List<int>();
                        listQuantityPart = new List<int>();
                        listsparePartOnWork.Add(sparePartOnWork);
                    }
                    
                    sparePartOnWork = new SparePartOnWork();
                    sparePartOnWork.Id = Convert.ToInt32(row["No"]);
                    sparePartOnWork.Priority = Convert.ToInt32(row["Priority"]);
                    sparePartOnWork.Device = (string)row["Device"];
                    sparePartOnWork.Work = (string)row["Work"];
                    sparePartOnWork.DueDate = DateTime.ParseExact((string)row["DueDate"], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    sparePartOnWork.ExcutionTime = Convert.ToInt32(row["ExecutionTime"]);
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
            sparePartOnWork.listNamePart = listNamePart;
            sparePartOnWork.listSequencePart = listSequencePart;
            sparePartOnWork.listQuantityPart = listQuantityPart;
            listsparePartOnWork.Add(sparePartOnWork);

            //Console.WriteLine();
            //foreach (var sparepart in listsparePartOnWork)
            //{
            //    Console.WriteLine("---------------------------");
            //    Console.WriteLine($"{sparepart.Id} {sparepart.Priority} {sparepart.Device} {sparepart.Work} {sparepart.DueDate} {sparepart.ExcutionTime}");
            //    for (int j = 0; j < sparepart.listSequencePart.Count; j++)
            //    {
            //        Console.WriteLine(sparepart.listNamePart[j].ToString() + " - " + sparepart.listSequencePart[j].ToString() + " - " + sparepart.listQuantityPart[j].ToString());
            //    }
            //}

            return listsparePartOnWork;
        }

        public static List<int> checksparePartAvailable(List<SparePart> listsparePart, List<SparePartOnWork> listsparePartOnWork, SparePartOnWork sparePartOnWork)
        {
            var listSequencePartLack = new List<int>();
            //Console.WriteLine($"The job {sparePartOnWork.Id} need {sparePartOnWork.listSequencePart.Count} parts");
            for (int i = 0; i < sparePartOnWork.listSequencePart.Count; i++)
            {
                if (sparePartOnWork.listSequencePart[i] != 0)
                {
                    //Console.WriteLine($"The part {sparePartOnWork.listSequencePart[i]} is considered");
                    var quantityPartInventory = listsparePart[sparePartOnWork.listSequencePart[i] - 1].Quantity;
                    var quantityPartOnWork = sparePartOnWork.listQuantityPart[i];
                    if (quantityPartInventory < quantityPartOnWork)
                    {
                        //Console.WriteLine($"The job {sparePartOnWork.Id} lack the sequence spare part {sparePartOnWork.listSequencePart[i]}");
                        listSequencePartLack.Add(sparePartOnWork.listSequencePart[i]);
                    }

                    //var newQuantityPart = quantityPartInventory - quantityPartOnWork;
                    //if (newQuantityPart >= 0)
                    //{
                    //    Console.WriteLine($"The job {sparePartOnWork.Id} - the sequence part {sparePartOnWork.listSequencePart[i]} consumed: {quantityPartOnWork}. And the new quantity part inventory: {newQuantityPart}");
                    //}
                }
            }

            return listSequencePartLack;
        }

        public static Dictionary<SparePartOnWork, List<int>> findJobRemove(List<SparePart> listsparePart, List<SparePartOnWork> listsparePartOnWork)
        {
            
            Dictionary<SparePartOnWork, List<int>> dictJobRemove = new Dictionary<SparePartOnWork, List<int>>();

            //Console.WriteLine();
            foreach (SparePartOnWork sparePartOnWork in listsparePartOnWork)
            {
                if (sparePartOnWork.listSequencePart[0] != 0)
                {
                    var listSequencePartLack = checksparePartAvailable(listsparePart, listsparePartOnWork, sparePartOnWork);

                    //Console.WriteLine();
                    if (listSequencePartLack.Count == 0)
                    {
                        for (int i = 0; i < sparePartOnWork.listSequencePart.Count; i++)
                        {
                            var quantityPartInventory = listsparePart[sparePartOnWork.listSequencePart[i] - 1].Quantity;
                            var quantityPartOnWork = sparePartOnWork.listQuantityPart[i];
                            var newQuantityPart = quantityPartInventory - quantityPartOnWork;
                            if (newQuantityPart >= 0)
                            {
                                listsparePart[sparePartOnWork.listSequencePart[i] - 1].Quantity = newQuantityPart;
                                //Console.WriteLine($"The job {sparePartOnWork.Id} - the sequence part {sparePartOnWork.listSequencePart[i]} consumed: {quantityPartOnWork}. And the new quantity part inventory: {newQuantityPart}");
                            }
                        }
                    }
                    else
                    {
                        dictJobRemove.Add(sparePartOnWork, listSequencePartLack);
                        //Console.WriteLine($"The job {sparePartOnWork.Id} is removed");
                        //foreach (int item in listSequencePartLack)
                        //{
                        //    Console.WriteLine($"Because this job {sparePartOnWork.Id} is lacked: the sequence part {item}");
                        //}
                    }
                }
                else
                {
                    //Console.WriteLine($"The job {sparePartOnWork.Id} doesn't need to use the spare part");
                    //Console.WriteLine("*************************************************************");
                    continue;
                }
                //Console.WriteLine("*************************************************************");
            }
            return dictJobRemove;
        }

        public static List<SparePartOnWork> getListWorkAvailable(DataTable sparePartTable, DataTable sparePartOnWorkTable)
        {
            List<SparePart> listsparePart = getListsparePart(sparePartTable);
            List<SparePartOnWork> listsparePartOnWork = getListsparePartOnWork(sparePartOnWorkTable);
            Dictionary<SparePartOnWork, List<int>> dictJobRemove = findJobRemove(listsparePart, listsparePartOnWork);
            var mondayOfThisWeek = DateTime.ParseExact("13/02/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var saturdayOfThisWeek = DateTime.ParseExact("18/02/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture);

            foreach (SparePartOnWork sparePartOnWork in dictJobRemove.Keys)
            {
                foreach (int sequence in dictJobRemove[sparePartOnWork])
                {
                    DateTime expectedDate = listsparePart[sequence - 1].expectedAddDate;
                    if (NewFindPlannedDate.isInRange(mondayOfThisWeek, saturdayOfThisWeek, expectedDate) == false)
                    {
                        listsparePartOnWork.Remove(sparePartOnWork);
                    }
                }
            }

            //Console.WriteLine();
            //foreach (var sparepart in listsparePartOnWork)
            //{
            //    Console.WriteLine($"{sparepart.Id} {sparepart.Priority} {sparepart.Device} {sparepart.Work} {sparepart.DueDate} {sparepart.ExcutionTime}");
            //}

            return listsparePartOnWork;
        }

        public static List<SparePartOnWork> getListWorkLackPartId(DataTable sparePartTable, DataTable sparePartOnWorkTable)
        {
            List<SparePart> listsparePart = getListsparePart(sparePartTable);
            List<SparePartOnWork> listsparePartOnWork = getListWorkAvailableChangedId(sparePartTable, sparePartOnWorkTable);
            Dictionary<SparePartOnWork, List<int>> dictJobRemove = findJobRemove(listsparePart, listsparePartOnWork);
            var mondayOfThisWeek = DateTime.ParseExact("13/02/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var saturdayOfThisWeek = DateTime.ParseExact("18/02/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture);

            List<SparePartOnWork> listWorkLackPartAvailable = new List<SparePartOnWork>();

            foreach (SparePartOnWork sparePartOnWork in dictJobRemove.Keys)
            {
                foreach (int sequence in dictJobRemove[sparePartOnWork])
                {
                    DateTime expectedDate = listsparePart[sequence - 1].expectedAddDate;
                    if (NewFindPlannedDate.isInRange(mondayOfThisWeek, saturdayOfThisWeek, expectedDate) == true)
                    {
                        listWorkLackPartAvailable.Add(sparePartOnWork);
                    }
                }
            }

            //Console.WriteLine();
            //foreach (var sparepart in listWorkLackPartAvailable)
            //{
            //    Console.WriteLine($"{sparepart.Id} {sparepart.Priority} {sparepart.Device} {sparepart.Work} {sparepart.DueDate} {sparepart.ExcutionTime}");
            //}

            return listWorkLackPartAvailable;
        }

        public static List<SparePartOnWork> getListWorkEnoughPartId(DataTable sparePartTable, DataTable sparePartOnWorkTable)
        {
            List<SparePart> listsparePart = getListsparePart(sparePartTable);
            List<SparePartOnWork> listsparePartOnWork = getListsparePartOnWork(sparePartOnWorkTable);
            Dictionary<SparePartOnWork, List<int>> dictJobRemove = findJobRemove(listsparePart, listsparePartOnWork);

            foreach (SparePartOnWork sparePartOnWork in dictJobRemove.Keys)
            {
                listsparePartOnWork.Remove(sparePartOnWork);
            }

            int j = 1;
            foreach (SparePartOnWork sparepart in listsparePartOnWork)
            {
                sparepart.Id = j++;
            }

            //Console.WriteLine();
            //foreach (var sparepart in listsparePartOnWork)
            //{
            //    Console.WriteLine($"{sparepart.Id} {sparepart.Priority} {sparepart.Device} {sparepart.Work} {sparepart.DueDate} {sparepart.ExcutionTime}");
            //}

            return listsparePartOnWork;
        }

        public static List<SparePartOnWork> getListWorkAvailableChangedId(DataTable sparePartTable, DataTable sparePartOnWorkTable)
        {
            List<SparePartOnWork> listsparePartOnWork = getListWorkAvailable(sparePartTable, sparePartOnWorkTable);

            int j = 1;
            foreach (SparePartOnWork sparepart in listsparePartOnWork)
            {
                sparepart.Id = j++;
            }

            //foreach (var sparepart in listsparePartOnWork)
            //{
            //    Console.WriteLine($"{sparepart.Id} {sparepart.Priority} {sparepart.Device} {sparepart.Work} {sparepart.DueDate} {sparepart.ExcutionTime}");
            //}

            return listsparePartOnWork;
        }

        public static List<int> returnListSequencePartLackOnWork(List<SparePart> listsparePart, List<SparePartOnWork> listsparePartOnWork, SparePartOnWork workNeedCheck)
        {
            var listSequencePartLack = new List<int>();
            foreach (SparePartOnWork sparePartOnWork in listsparePartOnWork)
            {
                if (sparePartOnWork.listSequencePart[0] != 0)
                {
                    listSequencePartLack = checksparePartAvailable(listsparePart, listsparePartOnWork, sparePartOnWork);
                    
                    if (listSequencePartLack.Count == 0)
                    {
                        for (int i = 0; i < sparePartOnWork.listSequencePart.Count; i++)
                        {
                            var quantityPartInventory = listsparePart[sparePartOnWork.listSequencePart[i] - 1].Quantity;
                            var quantityPartOnWork = sparePartOnWork.listQuantityPart[i];
                            var newQuantityPart = quantityPartInventory - quantityPartOnWork;
                            if (newQuantityPart >= 0)
                            {
                                listsparePart[sparePartOnWork.listSequencePart[i] - 1].Quantity = newQuantityPart;
                            }
                        }
                    }
                    else
                    {
                        if (sparePartOnWork.Id == workNeedCheck.Id)
                        {
                            Console.WriteLine($"The job {workNeedCheck.Id} is removed");
                            foreach (int item in listSequencePartLack)
                            {
                                Console.WriteLine($"Because this job {sparePartOnWork.Id} is lacked: the sequence part {item}");
                            }

                            return listSequencePartLack; 
                        }
                    }
                }
                else
                {
                    continue;
                }
            }
            return listSequencePartLack;
        }
    }
}
