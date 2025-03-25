using SLAP.AggregateModels.WorkAggregate;

namespace SLAP.AggregateModels.TechnicianAggregate
{
    public interface ITechnicianObjectInputRepository
    {
        TechnicianObjectInput Add(TechnicianObjectInput technician);
    }
}
