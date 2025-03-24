namespace TabuSearchProductionScheduling.Classes
{
    public class WorkCenter
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public float Capacity { get; set; }
        public double OEETarget { get; set; }
        public double TimeEfficient { get; set;}
        public List<WorkOrder> WorkOrders { get; set; }

        public WorkCenter(string code, string name, float capacity, double oEETarget, double timeEfficient)
        {
            Code = code;
            Name = name;
            Capacity = capacity;
            OEETarget = oEETarget;
            TimeEfficient = timeEfficient;
            this.WorkOrders = new List<WorkOrder>();
        }
    }
}
