using SLAP.AggregateModels.WorkAggregate;

namespace SLAP.Repository
{
    public class WorkObjectInputRepository : IWorkObjectInputRepository
    {
        public static List<WorkObjectInput> listWorkObjects = new List<WorkObjectInput>();
        public WorkObjectInput Add(WorkObjectInput work)
        {
            listWorkObjects.Add(work);
            return work;
        }
    }
}
