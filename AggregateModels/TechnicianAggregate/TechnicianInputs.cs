using SLAP.AggregateModels.TechnicianAggregate;

namespace SLAP.AggregateModels.TechnicianAggregate
{
    public class TechnicianInputs
    {
        public TechnicianObjectInput[]? JsonInput { get; set; }

        public TechnicianInputs() { }
        public TechnicianInputs(TechnicianObjectInput[]? jsonInput)
        {
            JsonInput = jsonInput;
        }
    }
}
