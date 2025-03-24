using TabuSearchProductionScheduling.Classes;
using TabuSearchProductionScheduling.Extensions;

namespace TabuSearchProductionScheduling.Planning
{
    public class WorkOrderPlanning
    {
        private static double conversionTime => 3 * 60;
        private DateTime firstDateStart { get; set; }
        private List<WorkOrder> plannedWorkOrders;

        public WorkOrderPlanning(DateTime firstDateStart)
        {
            this.firstDateStart = firstDateStart;
            this.plannedWorkOrders = new List<WorkOrder>();
        }

        public WorkOrder Planning(WorkOrder workOrder)
        {
            if (plannedWorkOrders.Count == 0)
            {
                workOrder.StartDate = firstDateStart;
                if (workOrder.ReleaseDate > workOrder.StartDate)
                    workOrder.StartDate = workOrder.ReleaseDate;
            }
            else
            {
                WorkOrder lastWorkOrder = plannedWorkOrders.Last();
                workOrder.StartDate = lastWorkOrder.EndDate;
                if (workOrder.Mold != lastWorkOrder.Mold)
                    workOrder.StartDate = workOrder.StartDate.AddTime(conversionTime);

                if (workOrder.ReleaseDate > workOrder.StartDate)
                    workOrder.StartDate = workOrder.ReleaseDate;
            }

            plannedWorkOrders.Add(workOrder);
            return workOrder;
        }
    }
}
