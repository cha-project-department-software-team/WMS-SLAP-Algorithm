using SLAP.AggregateModels.DeviceAggregate;

namespace SLAP.Repository
{
    public class DeviceObjectInputRepository : IDeviceObjectInputRepository
    {
        public static List<DeviceObjectInput> listDeviceObjects = new List<DeviceObjectInput>();
        public DeviceObjectInput Add(DeviceObjectInput device)
        {
            listDeviceObjects.Add(device);
            return device;
        }
    }
}
