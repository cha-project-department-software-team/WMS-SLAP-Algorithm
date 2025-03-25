using MediatR;
using SLAP.AggregateModels.DeviceAggregate;

namespace SLAP.Commands.Devices
{
    public record AddDeviceCommand(DeviceInputs devices) : IRequest<DeviceInputs>;
}
