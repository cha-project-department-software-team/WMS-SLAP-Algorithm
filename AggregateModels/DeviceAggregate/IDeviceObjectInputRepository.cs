using SLAP.AggregateModels.DeviceAggregate;

namespace SLAP.AggregateModels.DeviceAggregate
{
    public interface IDeviceObjectInputRepository
    {
        DeviceObjectInput Add(DeviceObjectInput device);
    }
}
