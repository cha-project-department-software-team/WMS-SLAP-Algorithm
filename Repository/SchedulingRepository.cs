using System.Data;
using SLAP.AggregateModels.DeviceAggregate;
using SLAP.AggregateModels.InputAggregate;
using SLAP.AggregateModels.InventoryReceiptAggregate;
using SLAP.AggregateModels.JobInforAggregate;
using SLAP.AggregateModels.StorageAggregate;
using SLAP.AggregateModels.TechnicianAggregate;
using SLAP.AggregateModels.WareHouseMaterialAggregate;
using SLAP.AggregateModels.WorkAggregate;
using Material = SLAP.AggregateModels.MaterialAggregate.Material;
using static SLAP.Constant;
using SLAPScheduling.Algorithms;

namespace SLAP.Repository
{
    public class SchedulingRepository : IObjectInputRepository
    {
        //public List<JobInfor> Implement(ObjectInput input)
        //{

        //    List<WorkObjectInput> listWorkObjects = input.works.JsonInput.ToList();
        //    List<DeviceObjectInput> listDeviceObjects = input.devices.JsonInput.ToList();
        //    List<TechnicianObjectInput> listTechnicianObjects = input.technicians.JsonInput.ToList();
        //    List<WareHouseMaterialObjectInput> listWareHouseMaterialObjects = input.wareHouseMaterials.JsonInput.ToList();

        //    Dictionary<string, List<List<DateTime>>> deviceBreakingTime = ConvertFromObjectToTable.getDeviceDictionary(listDeviceObjects);
        //    Dictionary<string, List<List<DateTime>>> technicianWorkingTime = ConvertFromObjectToTable.getTechnicianDictionary(listTechnicianObjects);

        //    var workTable = ConvertFromObjectToTable.ConvertObjectInputToWorksTable(listWorkObjects);
        //    List<WareHouseMaterial> listwareHouseMaterials = CheckMaterial.getWareHouseMaterial(listWareHouseMaterialObjects);
        //    List<Material> listMaterials = CheckMaterial.getListMaterial(listWorkObjects, listwareHouseMaterials);


        //    DataTable wareHouseMaterialTable = ConvertFromObjectToTable.ConvertToWareHouseMaterialTable(listwareHouseMaterials);
        //    DataTable materialTable = ConvertFromObjectToTable.ConvertToMaterialTable(listMaterials);

        //    List<Material> listsparePartAvailable = CheckMaterial.getListWorkAvailable(listwareHouseMaterials, listMaterials);
        //    DataTable workAvailableTable = TabuSearch.getWorkAvailableTable(workTable, listsparePartAvailable);

        //    List<JobInfor> listJobInfor = TabuSearch.tabuSearch(workAvailableTable, deviceBreakingTime, technicianWorkingTime,
        //                                                        wareHouseMaterialTable, materialTable,
        //                                                        listwareHouseMaterials, listMaterials);

        //    return listJobInfor;
        //}

        public List<ReceiptSubLot> Implement(InventoryReceipt inventoryReceipt, Warehouse warehouse, List<Material> materials)
        {
            if (inventoryReceipt == null || warehouse == null || materials == null)
                return null;

            var receiptSubLots = new List<ReceiptSubLot>();
            using (var receiptLotSplitter = new ReceiptLotSplitter(inventoryReceipt.Entries, materials, warehouse))
            {
                // Receipt Sublots do not include the Location information
                receiptSubLots = receiptLotSplitter.GetReceiptSubLots().ToList();
            }

            return new List<ReceiptSubLot>();
        }
    }
}
