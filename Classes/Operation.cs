using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabuSearchProductionScheduling.Classes
{
    public class Operation
    {
        public string Code { get; set; }
        public WorkCenter WorkCenter { get; set; }
        public List<WorkCenter> AlternativeWorkCenters { get; set; }
        public double ManualDuration { get; set; }

        public Operation(string code, WorkCenter workCenter, List<WorkCenter> alternativeWorkCenters, double manualDuration)
        {
            Code = code;
            WorkCenter = workCenter;
            AlternativeWorkCenters = alternativeWorkCenters;
            ManualDuration = manualDuration;
        }
    }
}
