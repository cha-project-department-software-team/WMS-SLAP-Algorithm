using SLAP.AggregateModels.InputAggregate;
using SLAP.AggregateModels.JobInforAggregate;

namespace SLAP.AggregateModels.InputAggregate
{
    public interface IObjectInputRepository
    {
        List<JobInfor> Implement(ObjectInput input);
    }
}
