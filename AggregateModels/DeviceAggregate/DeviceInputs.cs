using SLAP.AggregateModels.DeviceAggregate;

namespace SLAP.AggregateModels.DeviceAggregate
{
    public class DeviceInputs
    {
        public DeviceObjectInput[]? JsonInput { get; set; }

        public DeviceInputs() { }
        public DeviceInputs(DeviceObjectInput[]? jsonInput)
        {
            JsonInput = jsonInput;
        }
    }
}
