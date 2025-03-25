using Microsoft.VisualBasic;
using SLAP.AggregateModels.WorkAggregate;
using static SLAP.Constant;

namespace SLAP.AggregateModels.JobInforAggregate
{
    public class JobInfor
    {
        public int Id { get; set; }
        public int Priority { get; set; }
        public string Device { get; set; }
        public string Work { get; set; }
        public int Technician { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime StartPlannedDate { get; set; }
        public DateTime EndPlannedDate { get; set; }
        public int EstProcessTime { get; set; }
        public MaterialOnWork[]? Materials { get; set; }
        public int[] ArrayFail { get; set; } = new int[5];
        public JobInfor() { }

        public JobInfor(int id, string device, string work, int technician, DateTime startPlannedDate, DateTime endPlannedDate)
        {
            Id = id;
            Device = device;
            Work = work;
            Technician = technician;
            StartPlannedDate = startPlannedDate;
            EndPlannedDate = endPlannedDate;
        }

        public JobInfor(int id, int priority, string device, string work, int technician, DateTime dueDate, DateTime startPlannedDate, DateTime endPlannedDate, int estProcessTime, MaterialOnWork[]? materials, int[] arrayFail)
        {
            Id = id;
            Priority = priority;
            Device = device;
            Work = work;
            Technician = technician;
            DueDate = dueDate;
            StartPlannedDate = startPlannedDate;
            EndPlannedDate = endPlannedDate;
            EstProcessTime = estProcessTime;
            Materials = materials;
            ArrayFail = arrayFail;
        }
    }
}
