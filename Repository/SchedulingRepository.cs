using System.Data;
using SLAP.AggregateModels.DeviceAggregate;
using SLAP.AggregateModels.InputAggregate;
using SLAP.AggregateModels.JobInforAggregate;
using SLAP.AggregateModels.TechnicianAggregate;
using SLAP.AggregateModels.WareHouseMaterialAggregate;
using SLAP.AggregateModels.WorkAggregate;
using static SLAP.Constant;

namespace SLAP.Repository
{
    public class SchedulingRepository : IObjectInputRepository
    {
        public List<JobInfor> Implement(ObjectInput input)
        {

            List<WorkObjectInput> listWorkObjects = input.works.JsonInput.ToList();
            List<DeviceObjectInput> listDeviceObjects = input.devices.JsonInput.ToList();
            List<TechnicianObjectInput> listTechnicianObjects = input.technicians.JsonInput.ToList();
            List<WareHouseMaterialObjectInput> listWareHouseMaterialObjects = input.wareHouseMaterials.JsonInput.ToList();

            Dictionary<string, List<List<DateTime>>> deviceBreakingTime = ConvertFromObjectToTable.getDeviceDictionary(listDeviceObjects);
            Dictionary<string, List<List<DateTime>>> technicianWorkingTime = ConvertFromObjectToTable.getTechnicianDictionary(listTechnicianObjects);

            var workTable = ConvertFromObjectToTable.ConvertObjectInputToWorksTable(listWorkObjects);
            List<WareHouseMaterial> listwareHouseMaterials = CheckMaterial.getWareHouseMaterial(listWareHouseMaterialObjects);
            List<Material> listMaterials = CheckMaterial.getListMaterial(listWorkObjects, listwareHouseMaterials);


            DataTable wareHouseMaterialTable = ConvertFromObjectToTable.ConvertToWareHouseMaterialTable(listwareHouseMaterials);
            DataTable materialTable = ConvertFromObjectToTable.ConvertToMaterialTable(listMaterials);

            List<Material> listsparePartAvailable = CheckMaterial.getListWorkAvailable(listwareHouseMaterials, listMaterials);
            DataTable workAvailableTable = TabuSearch.getWorkAvailableTable(workTable, listsparePartAvailable);

            List<JobInfor> listJobInfor = TabuSearch.tabuSearch(workAvailableTable, deviceBreakingTime, technicianWorkingTime,
                                                                wareHouseMaterialTable, materialTable,
                                                                listwareHouseMaterials, listMaterials);

            //List<JobInfor> bestListWorkInfor = new List<JobInfor>();
            //foreach(WorkObjectInput workObject in listWorkObjects)
            //{
            //    JobInfor jobInfor = new JobInfor();
            //    jobInfor.Id = int.Parse(workObject.id);
            //    jobInfor.Priority = int.Parse(workObject.priority);
            //    jobInfor.Device = workObject.deviceCode;
            //    jobInfor.Technician = 0;
            //    jobInfor.DueDate = workObject.dueDate;
            //    jobInfor.StartPlannedDate = DateTime.Now;
            //    jobInfor.EndPlannedDate = DateTime.Now;
            //    jobInfor.EstProcessTime = int.Parse(workObject.estProcessTime);
            //    jobInfor.Materials = workObject.materials;
            //    bestListWorkInfor.Add(jobInfor);
            //}

            //System.Threading.Thread.Sleep(600000);
            return listJobInfor;
        }
    }
}
