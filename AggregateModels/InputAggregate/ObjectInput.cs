using SLAP.AggregateModels.DeviceAggregate;
using SLAP.AggregateModels.TechnicianAggregate;
using SLAP.AggregateModels.WareHouseMaterialAggregate;
using SLAP.AggregateModels.WorkAggregate;

namespace SLAP.AggregateModels.InputAggregate
{
    public class ObjectInput
    {
        public WorkInputs works { get; set; }
        public DeviceInputs devices { get; set; }
        public TechnicianInputs technicians { get; set; }
        public WareHouseMaterialInputs wareHouseMaterials { get; set; }

        public ObjectInput()
        {

        }

        public ObjectInput(WorkInputs works, DeviceInputs devices, TechnicianInputs technicians, WareHouseMaterialInputs wareHouseMaterials)
        {
            this.works = works;
            this.devices = devices;
            this.technicians = technicians;
            this.wareHouseMaterials = wareHouseMaterials;
        }
    }
}
