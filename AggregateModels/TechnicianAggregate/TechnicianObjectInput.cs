using static TabuSearchImplement.Constant;

namespace TabuSearchImplement.AggregateModels.TechnicianAggregate
{
    public class TechnicianObjectInput
    {
        public string? id { get; set; }
        public string? name { get; set; }
        public WorkingTime[][]? workingTime { get; set; }
        public TechnicianObjectInput() { }

        public TechnicianObjectInput(string? id, string? name, WorkingTime[][]? workingTime)
        {
            this.id = id;
            this.name = name;
            this.workingTime = workingTime;
        }
    }
}
