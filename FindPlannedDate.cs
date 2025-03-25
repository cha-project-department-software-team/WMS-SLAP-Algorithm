using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SLAP.AggregateModels.JobInforAggregate;
using SLAP.AggregateModels.MaterialAggregate;
using SLAP.AggregateModels.JobInforAggregate;
using static SLAP.Constant;
using static SLAP.Constant;
using Material = SLAP.Constant.Material;

namespace SLAP
{
    public class FindPlannedDate
    {
        /// <summary>
        /// Check: Is the considered date exist in range of (startDate, endDate)?
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="checkDate"></param>
        /// <returns></returns>
        public static bool isInRange(DateTime? startDate, DateTime? endDate, DateTime? checkDate)
        {
            return (startDate <= checkDate) && (checkDate <= endDate);
        }

        /// <summary>
        /// Create an empty dictionary is similar to deviceDictionary. 
        /// </summary>
        /// <param name="deviceDictionary"></param>
        /// <returns></returns>
        public static Dictionary<string, List<List<DateTime>>> deviceStructure(Dictionary<string, List<List<DateTime>>> deviceDictionary)
        {
            Dictionary<string, List<List<DateTime>>> maintenanceDeviceBreakTime = new Dictionary<string, List<List<DateTime>>>();
            foreach (string key in deviceDictionary.Keys)
            {
                List<List<DateTime>> listDateTimeEmpty = new List<List<DateTime>>();
                maintenanceDeviceBreakTime.Add(key, listDateTimeEmpty);
            }

            return maintenanceDeviceBreakTime;
        }

        /// <summary>
        /// Create an empty dictionary is similar to technicianDictionary. 
        /// </summary>
        /// <param name="technicianDictionary"></param>
        /// <returns></returns>
        public static Dictionary<string, List<List<DateTime>>> technicianStructure(Dictionary<string, List<List<DateTime>>> technicianDictionary)
        {
            Dictionary<string, List<List<DateTime>>> maintenanceTechnicianWorkTime = new Dictionary<string, List<List<DateTime>>>();
            foreach (string no in technicianDictionary.Keys)
            {
                List<List<DateTime>> listDateTimeEmpty = new List<List<DateTime>>();
                maintenanceTechnicianWorkTime.Add(no, listDateTimeEmpty);
            }

            return maintenanceTechnicianWorkTime;
        }

        public static List<string> shuffleList(List<string> list)
        {
            var random = new Random();
            var newShuffledList = new List<string>();
            var listcCount = list.Count;
            for (int i = 0; i < listcCount; i++)
            {
                var randomElementInList = random.Next(0, list.Count);
                newShuffledList.Add(list[randomElementInList]);
                list.Remove(list[randomElementInList]);
            }
            return newShuffledList;
        }

        /// <summary>
        /// When we have already calculated the planned date, this method to check the available of this date and the seuquence of technician assigned
        /// If everything is ok, return 0
        /// If the break time of device is not available, return 1
        /// If the work time of technician is not available, return 2
        /// If there are 2 jobs performing on the same device simultaneously, return 3
        /// If a technician is assigned 2 jobs at the same time, return 4
        /// </summary>
        /// <param name="nameOfDevice"></param>
        /// <param name="listReturn"></param>
        /// <param name="listStartEndWorking"></param>
        /// <param name="deviceDictionary"></param>
        /// <param name="technicianDictionary"></param>
        /// <param name="maintenanceDeviceBreakTime"></param>
        /// <param name="maintenanceTechnicianWorkTime"></param>
        /// <returns></returns>
        public static JobInfor checkTimeAvailable(JobInfor workInfor,
                                                  Dictionary<string, List<List<DateTime>>> deviceDictionary,
                                                  Dictionary<string, List<List<DateTime>>> technicianDictionary,
                                                  Dictionary<string, List<List<DateTime>>> maintenanceDeviceBreakTime,
                                                  Dictionary<string, List<List<DateTime>>> maintenanceTechnicianWorkTime)
        {
            bool checkDeviceAvailable = false;
            foreach (List<DateTime> listBreakTime in deviceDictionary[workInfor.Device])
            {
                if (isInRange(listBreakTime[0], listBreakTime[1], workInfor.StartPlannedDate) && isInRange(listBreakTime[0], listBreakTime[1], workInfor.EndPlannedDate))
                {
                    //  Check the available of device's break time
                    checkDeviceAvailable = true;
                    break;
                }
            }

            bool checkTechnicianAvailable = false;
            List<string> listNoTechnician = technicianDictionary.Keys.ToList();

            List<string> newListNoTechnician = shuffleList(listNoTechnician);

            foreach (string no in newListNoTechnician)
            {
                foreach (List<DateTime> listWorkTime in technicianDictionary[no])
                {
                    if (isInRange(listWorkTime[0], listWorkTime[1], workInfor.StartPlannedDate) && isInRange(listWorkTime[0], listWorkTime[1], workInfor.EndPlannedDate))
                    {
                        checkTechnicianAvailable = true;
                        break;
                    }
                }

                if (checkTechnicianAvailable)
                {
                    workInfor.Technician = int.Parse(no);
                    break;
                }
            }

            if (checkDeviceAvailable && checkTechnicianAvailable)
            {
                bool checkTechnicianDuplicate = false;
                foreach (List<DateTime> listTechnicianWorkTime in maintenanceTechnicianWorkTime[workInfor.Technician.ToString()])
                {
                    if (isInRange(listTechnicianWorkTime[0], listTechnicianWorkTime[1], workInfor.StartPlannedDate) || isInRange(listTechnicianWorkTime[0], listTechnicianWorkTime[1], workInfor.EndPlannedDate))
                    {
                        checkTechnicianDuplicate = true;
                        break;
                    }
                }

                if (checkTechnicianDuplicate == true)
                {
                    workInfor.ArrayFail[4] = 1;
                }

                bool checkDeviceDuplicate = false;
                foreach (List<DateTime> listDeviceBreakTime in maintenanceDeviceBreakTime[workInfor.Device])
                {
                    if (isInRange(listDeviceBreakTime[0], listDeviceBreakTime[1], workInfor.StartPlannedDate) || isInRange(listDeviceBreakTime[0], listDeviceBreakTime[1], workInfor.EndPlannedDate))
                    {
                        checkDeviceDuplicate = true;
                        break;
                    }
                }

                if (checkDeviceDuplicate == true)
                {
                    workInfor.ArrayFail[3] = 1;
                }

                if (checkDeviceDuplicate == false && checkTechnicianDuplicate == false)
                {
                    // If everything is ok
                    workInfor.ArrayFail[0] = 1;
                }
            }
            else if (checkDeviceAvailable == false)
            {
                // If the break time of device is not available, return 1
                workInfor.ArrayFail[1] = 1;
            }
            else if (checkTechnicianAvailable == false)
            {
                // If the work time of technician is not available, return 2
                workInfor.ArrayFail[2] = 1;
            }

            return workInfor;
        }

        /// <summary>
        /// If the planned date have an error, this method will modify the planned date and double-check until this date is suitable for implement
        /// </summary>
        /// <param name="workAvailableTable"></param>
        /// <param name="job"></param>
        /// <param name="nameOfDevice"></param>
        /// <param name="listStartEndWorking"></param>
        /// <param name="listReturn"></param>
        /// <param name="deviceDictionary"></param>
        /// <param name="technicianDictionary"></param>
        /// <param name="maintenanceDeviceBreakTime"></param>
        /// <param name="maintenanceTechnicianWorkTime"></param>
        /// <returns></returns>
        public static JobInfor changePlannedDate(DataTable workAvailableTable, JobInfor workInfor,
                                                 Dictionary<string, List<List<DateTime>>> deviceDictionary,
                                                 Dictionary<string, List<List<DateTime>>> technicianDictionary,
                                                 Dictionary<string, List<List<DateTime>>> maintenanceDeviceBreakTime,
                                                 Dictionary<string, List<List<DateTime>>> maintenanceTechnicianWorkTime,
                                                 Material workLackPart)
        {
            if (workInfor.ArrayFail[1] == 1)
            {
                if (workLackPart.Id == null)
                {
                    for (int i = 0; i < (deviceDictionary[workInfor.Device].Count - 1); i++)
                    {
                        if (deviceDictionary[workInfor.Device][i][0] <= workInfor.StartPlannedDate && workInfor.StartPlannedDate <= deviceDictionary[workInfor.Device][i + 1][0])
                        {
                            workInfor.StartPlannedDate = deviceDictionary[workInfor.Device][i + 1][0].Add(TimeSpan.FromMinutes(1));
                            break;
                        }
                    }
                }
                else if (workLackPart.Id != null)
                {
                    //Console.WriteLine($"The job {workInfor.Id} is the first job on the {workInfor.Device} and not exist in the break time of device");
                    for (int i = 0; i < (deviceDictionary[workInfor.Device].Count - 1); i++)
                    {
                        if (deviceDictionary[workInfor.Device][i][0] <= workInfor.StartPlannedDate && workInfor.StartPlannedDate <= deviceDictionary[workInfor.Device][i + 1][0])
                        {
                            workInfor.StartPlannedDate = deviceDictionary[workInfor.Device][i + 1][0].Add(TimeSpan.FromMinutes(1));
                            break;
                        }
                    }
                }

                //Console.WriteLine($"The job {workInfor.Id} with fail 1 has new planned start is {workInfor.StartPlannedDate} on the device {workInfor.Device}");

                double minutes = double.Parse((string)workAvailableTable.Rows[workInfor.Id - 1]["ExecutionTime"]);
                TimeSpan executionTime = TimeSpan.FromMinutes(minutes);
                workInfor.EndPlannedDate = workInfor.StartPlannedDate.Add(executionTime);
            }

            if (workInfor.ArrayFail[2] == 1)
            {
                if (workLackPart.Id == null)
                {
                    //Console.WriteLine($"The technician {workInfor.Technician} has the fail 2");
                    if (workInfor.StartPlannedDate < technicianDictionary[workInfor.Technician.ToString()][0][0])
                    {
                        workInfor.StartPlannedDate = technicianDictionary[workInfor.Technician.ToString()][0][0].AddMinutes(1);
                    }
                    else
                    {
                        for (int i = 0; i < (technicianDictionary[workInfor.Technician.ToString()].Count - 1); i++)
                        {
                            if (technicianDictionary[workInfor.Technician.ToString()][i][0] <= workInfor.StartPlannedDate && workInfor.StartPlannedDate <= technicianDictionary[workInfor.Technician.ToString()][i + 1][0])
                            {
                                workInfor.StartPlannedDate = technicianDictionary[workInfor.Technician.ToString()][i + 1][0].Add(TimeSpan.FromMinutes(1));
                                break;
                            }
                        }
                    }
                }
                else if (workLackPart.Id != null)
                {
                    //Console.WriteLine($"The job {workInfor.Id} is the first job on the {workInfor.Device} and not exist in the break time of device");
                    if (workInfor.StartPlannedDate < technicianDictionary[workInfor.Technician.ToString()][0][0])
                    {
                        workInfor.StartPlannedDate = technicianDictionary[workInfor.Technician.ToString()][0][0].AddMinutes(1);
                    }
                    else 
                    {
                        for (int i = 0; i < (technicianDictionary[workInfor.Technician.ToString()].Count - 1); i++)
                        {
                            if (technicianDictionary[workInfor.Technician.ToString()][i][0] <= workInfor.StartPlannedDate && workInfor.StartPlannedDate <= technicianDictionary[workInfor.Technician.ToString()][i + 1][0])
                            {
                                workInfor.StartPlannedDate = technicianDictionary[workInfor.Technician.ToString()][i + 1][0].Add(TimeSpan.FromMinutes(1));
                                break;
                            }
                        }
                    }
                }

                //Console.WriteLine($"The job {workInfor.Id} with fail 2 has new planned start is {workInfor.StartPlannedDate} and sequence of technician {workInfor.Technician}");

                double minutes = double.Parse((string)workAvailableTable.Rows[workInfor.Id - 1]["ExecutionTime"]);
                TimeSpan executionTime = TimeSpan.FromMinutes(minutes);
                workInfor.EndPlannedDate = workInfor.StartPlannedDate.Add(executionTime);
            }

            if (workInfor.ArrayFail[3] == 1)
            {
                int numberOfWorkOnDevice = maintenanceDeviceBreakTime[workInfor.Device].Count;
                workInfor.StartPlannedDate = maintenanceDeviceBreakTime[workInfor.Device][numberOfWorkOnDevice - 1][1].Add(TimeSpan.FromMinutes(1));
                double minutes = double.Parse((string)workAvailableTable.Rows[workInfor.Id - 1]["ExecutionTime"]);
                TimeSpan executionTime = TimeSpan.FromMinutes(minutes);
                workInfor.EndPlannedDate = workInfor.StartPlannedDate.Add(executionTime);
                //Console.WriteLine($"The job {workInfor.Id} with fail 3 has new planned start is {workInfor.StartPlannedDate}");

            }

            if (workInfor.ArrayFail[4] == 1)
            {
                if (maintenanceTechnicianWorkTime[workInfor.Technician.ToString()].Count > 8)
                {
                    Random rnd = new Random();
                    workInfor.Technician = rnd.Next(1, maintenanceTechnicianWorkTime.Count);
                }

                workInfor.StartPlannedDate = workInfor.StartPlannedDate.AddMinutes(1);
                workInfor.EndPlannedDate = workInfor.EndPlannedDate.AddMinutes(1);

                for (int i = 0; i < maintenanceTechnicianWorkTime[workInfor.Technician.ToString()].Count - 1; i++)
                {
                    //Console.WriteLine($"The job {workInfor.Id} with fail 4 in technician {workInfor.Technician} with {workInfor.StartPlannedDate} - {workInfor.EndPlannedDate}");

                    if (isInRange(maintenanceTechnicianWorkTime[workInfor.Technician.ToString()][i][0], maintenanceTechnicianWorkTime[workInfor.Technician.ToString()][i][1], workInfor.StartPlannedDate) || isInRange(maintenanceTechnicianWorkTime[workInfor.Technician.ToString()][i][0], maintenanceTechnicianWorkTime[workInfor.Technician.ToString()][i][1], workInfor.EndPlannedDate))
                    {
                        //Console.WriteLine($"The job {workInfor.Id} is duplicated in technician {workInfor.Technician} with duplicated job: {maintenanceTechnicianWorkTime[workInfor.Technician.ToString()][i][0]} - {maintenanceTechnicianWorkTime[workInfor.Technician.ToString()][i][1]}");
                        workInfor.StartPlannedDate = maintenanceTechnicianWorkTime[workInfor.Technician.ToString()][i][1].Add(TimeSpan.FromMinutes(1));
                    }
                }

                //Console.WriteLine($"The job {workInfor.Id} with fail 4 has new planned start is {workInfor.StartPlannedDate}");

                double minutes = double.Parse((string)workAvailableTable.Rows[workInfor.Id - 1]["ExecutionTime"]);
                TimeSpan executionTime = TimeSpan.FromMinutes(minutes);
                workInfor.EndPlannedDate = workInfor.StartPlannedDate.Add(executionTime);
            }

            workInfor = checkTimeAvailable(workInfor, deviceDictionary, technicianDictionary,
                                           maintenanceDeviceBreakTime, maintenanceTechnicianWorkTime);
            if (workInfor.ArrayFail[0] == 1)
            {
                return workInfor;
            }
            else if (workInfor.ArrayFail[0] != 1)
            {
                workInfor = changePlannedDate(workAvailableTable, workInfor,
                                              deviceDictionary, technicianDictionary,
                                              maintenanceDeviceBreakTime, maintenanceTechnicianWorkTime,
                                              workLackPart);
                return workInfor;
            }

            return workInfor;
        }



        public static JobInfor findPlannedDate(DataTable workAvailableTable, List<int> soluton, int job, JobInfor workInfor,
                                               Dictionary<string, List<List<DateTime>>> deviceDictionary,
                                               Dictionary<string, List<List<DateTime>>> technicianDictionary,
                                               Dictionary<string, List<List<DateTime>>> maintenanceDeviceBreakTime,
                                               Dictionary<string, List<List<DateTime>>> maintenanceTechnicianWorkTime,
                                               List<WareHouseMaterial> listWareHouseMaterials,
                                               List<Material> listMaterials,
                                               List<Material> listWorkAvailableChangedId,
                                               Material workLackPart)
        {
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;

            string nameOfDevice = workAvailableTable.Rows[job - 1]["Device"].ToString();
            //Console.WriteLine("--------------------------------------------------------------");
            //Console.WriteLine($"Name of device: {nameOfDevice}. And this is job {job}");
            int numberWorkOnDevice = maintenanceDeviceBreakTime[nameOfDevice].Count;
            string problem = workAvailableTable.Rows[job - 1]["Work"].ToString();
            //Console.WriteLine($"Number of Work on device: {numberWorkOnDevice}");

            var random = new Random();
            workInfor = new JobInfor(job, nameOfDevice, problem, random.Next(1, technicianDictionary.Count), startDate, endDate);

            //Console.WriteLine($"The job {workInfor.Id} with spare part on work id {listWorkAvailableChangedId[workInfor.Id - 1].Id} and number of part {listWorkAvailableChangedId[workInfor.Id - 1].listSequencePart.Count}");

            DateTime lastestStartDate = new DateTime();
            var mondayOfThisWeek = DateTime.ParseExact("24/04/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var saturdayOfThisWeek = DateTime.ParseExact("30/04/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture);
            if (workLackPart.Id != null)
            {
                //Console.WriteLine($"The job {workInfor.Id} is lack spare part");
                var listSequencePartLack = SLAP.CheckMaterial.returnListSequencePartLackOnWork(listWareHouseMaterials, listWorkAvailableChangedId, workLackPart);
                //Console.WriteLine($"The length of listSequencePartLack: {listSequencePartLack.Count}");
                lastestStartDate = listWareHouseMaterials[listSequencePartLack[0] - 1].ExpectedAddDate;
                //Console.WriteLine($"The sequence part is {listSequencePartLack[0]} with expected add date {listWareHouseMaterials[listSequencePartLack[0] - 1].ExpectedAddDate}");

                if (listSequencePartLack.Count >= 2)
                {
                    foreach (var sequence in listSequencePartLack)
                    {
                        //Console.WriteLine($"The sequence part is {sequence} with expected add date {listWareHouseMaterials[sequence - 1].ExpectedAddDate}");
                        if (listWareHouseMaterials[sequence].ExpectedAddDate > lastestStartDate)
                        {
                            lastestStartDate = listWareHouseMaterials[sequence].ExpectedAddDate;
                        }
                    }
                }


                workInfor.StartPlannedDate = lastestStartDate;
                //Console.WriteLine($"The start date in job {workInfor.Id} is {workInfor.StartPlannedDate}");
            }
            else
            {
                if (numberWorkOnDevice == 0)
                {
                    workInfor.StartPlannedDate = deviceDictionary[workInfor.Device][0][0].Add(TimeSpan.FromMinutes(1));
                    //Console.WriteLine($"The start date in job {workInfor.Id} on device {workInfor.Device} is {workInfor.StartPlannedDate}");

                }
                else
                {
                    workInfor.StartPlannedDate = maintenanceDeviceBreakTime[workInfor.Device][numberWorkOnDevice - 1][1].Add(TimeSpan.FromMinutes(1));
                    //Console.WriteLine($"The start date in job {workInfor.Id} on device {workInfor.Device} is {workInfor.StartPlannedDate}");
                }
            }

            double minutes = double.Parse((string)workAvailableTable.Rows[job - 1]["ExecutionTime"]);
            TimeSpan executionTime = TimeSpan.FromMinutes(minutes);
            workInfor.EndPlannedDate = workInfor.StartPlannedDate.Add(executionTime);
            //Console.WriteLine($"The end date in job {workInfor.Id} on device {workInfor.Device} is {workInfor.EndPlannedDate}");
            // Check: Is the planned date available?
            workInfor = checkTimeAvailable(workInfor, deviceDictionary,
                                           technicianDictionary, maintenanceDeviceBreakTime,
                                           maintenanceTechnicianWorkTime);
            //Console.WriteLine($"The Fail's number: {workInfor.ArrayFail[0]} - {workInfor.ArrayFail[1]} - {workInfor.ArrayFail[2]} - {workInfor.ArrayFail[3]} - {workInfor.ArrayFail[4]}");

            if (workInfor.ArrayFail[0] == 1)
            {
                List<DateTime> listStartEndWorking = new List<DateTime> { workInfor.StartPlannedDate, workInfor.EndPlannedDate };
                // Add the record into maintenanceDeviceBreakTime
                List<List<DateTime>> listListDeviceBreakingTime = maintenanceDeviceBreakTime[workInfor.Device];
                List<List<DateTime>> listListTempDevice = new List<List<DateTime>>();
                foreach (List<DateTime> listTemp in listListDeviceBreakingTime)
                {
                    listListTempDevice.Add(listTemp);
                }
                listListTempDevice.Add(listStartEndWorking);
                maintenanceDeviceBreakTime[workInfor.Device] = new List<List<DateTime>>();
                maintenanceDeviceBreakTime[workInfor.Device] = listListTempDevice;

                // Add the record into maintenanceTechnicianWorkTime
                List<List<DateTime>> listListTechnicianWorkingTime = maintenanceTechnicianWorkTime[workInfor.Technician.ToString()];
                List<List<DateTime>> listListTempTechnician = new List<List<DateTime>>();
                foreach (List<DateTime> listTemp in listListTechnicianWorkingTime)
                {
                    listListTempTechnician.Add(listTemp);
                }
                listListTempTechnician.Add(listStartEndWorking);

                maintenanceTechnicianWorkTime[workInfor.Technician.ToString()] = new List<List<DateTime>>();
                maintenanceTechnicianWorkTime[workInfor.Technician.ToString()] = listListTempTechnician;
            }
            else
            {
                // Get the modified date by changePlannedDate
                workInfor = changePlannedDate(workAvailableTable, workInfor,
                                              deviceDictionary, technicianDictionary,
                                              maintenanceDeviceBreakTime, maintenanceTechnicianWorkTime,
                                              workLackPart);
                // Get the sequence of Technician perform this job
                workInfor = checkTimeAvailable(workInfor, deviceDictionary,
                                               technicianDictionary, maintenanceDeviceBreakTime,
                                               maintenanceTechnicianWorkTime);

                List<DateTime> listStartEndWorking = new List<DateTime> { workInfor.StartPlannedDate, workInfor.EndPlannedDate };
                // Add the record into maintenanceDeviceBreakTime
                List<List<DateTime>> listListDeviceBreakingTime = maintenanceDeviceBreakTime[workInfor.Device];
                List<List<DateTime>> listListTempDevice = new List<List<DateTime>>();
                foreach (List<DateTime> listTemp in listListDeviceBreakingTime)
                {
                    listListTempDevice.Add(listTemp);
                }
                listListTempDevice.Add(listStartEndWorking);
                maintenanceDeviceBreakTime[workInfor.Device] = new List<List<DateTime>>();
                maintenanceDeviceBreakTime[workInfor.Device] = listListTempDevice;

                // Add the record into maintenanceTechnicianWorkTime
                List<List<DateTime>> listListTechnicianWorkingTime = maintenanceTechnicianWorkTime[workInfor.Technician.ToString()];
                List<List<DateTime>> listListTempTechnician = new List<List<DateTime>>();
                foreach (List<DateTime> listTemp in listListTechnicianWorkingTime)
                {
                    listListTempTechnician.Add(listTemp);
                }
                listListTempTechnician.Add(listStartEndWorking);
                maintenanceTechnicianWorkTime[workInfor.Technician.ToString()] = new List<List<DateTime>>();
                maintenanceTechnicianWorkTime[workInfor.Technician.ToString()] = listListTempTechnician;
            }

            //Console.WriteLine();
            //Console.WriteLine($"The start and end planned date are: {workInfor.StartPlannedDate} - {workInfor.EndPlannedDate}");
            //Console.WriteLine();
            ////// Print all of maintenance device's record
            //foreach (string key in maintenanceDeviceBreakTime.Keys)
            //{
            //    Console.WriteLine($"The name of device is checked: {key}");
            //    foreach (List<DateTime> listTime in maintenanceDeviceBreakTime)
            //    {
            //        Console.WriteLine(listTime[0].ToString() + " " + listTime[1].ToString());
            //    }
            //}

            //Console.WriteLine();
            ////// Print all of maintenance technician's record
            //foreach (string key in maintenanceTechnicianWorkTime.Keys)
            //{
            //    Console.WriteLine($"The sequence of technician is checked: {key}");
            //    foreach (List<DateTime> listTime in maintenanceTechnicianWorkTime)
            //    {
            //        Console.WriteLine(listTime[0].ToString() + " " + listTime[1].ToString());
            //    }
            //}
            //Console.WriteLine();

            return workInfor;
        }
    }
}
