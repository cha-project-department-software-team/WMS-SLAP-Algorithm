using TabuSearchProductionScheduling.Extensions;

namespace TabuSearchProductionScheduling.Classes
{
    public class WorkOrder
    {
        public int Id { get; set; }
        public double Priority { get; set; }
        public string ProductCode { get; set; }
        public double ProcessingTime { get; set; }
        public string Mold { get; set; }
        public BOM BOM { get; set; }
        public WorkCenter WorkCenter { get; set; }
        public List<WorkCenter> AlternativeWorkCenters { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate => StartDate.AddTime(ProcessingTime);
        public DateTime DueDate { get; set; }

        public WorkOrder(int id, double priority, string productCode, string mold, double processingTime, WorkCenter workCenter, List<WorkCenter> alternativeWorkCenters, BOM bom, DateTime releaseDate, DateTime startDate, DateTime dueDate)
        {
            this.Id = id;
            this.Priority = priority;
            this.ProductCode = productCode;
            this.ProcessingTime = processingTime;
            this.Mold = mold;
            this.WorkCenter = workCenter;
            this.AlternativeWorkCenters = alternativeWorkCenters;
            this.BOM = bom;
            this.ReleaseDate = releaseDate;
            this.StartDate = startDate;
            this.DueDate = dueDate;
        }
    }
}
