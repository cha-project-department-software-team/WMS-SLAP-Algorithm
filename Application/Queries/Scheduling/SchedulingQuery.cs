using MediatR;
using SLAPScheduling.Application.DTOs.InventoryReceiptDTOs;

namespace SLAPScheduling.Application.Queries.Scheduling
{
    public class SchedulingQuery : IRequest<IEnumerable<ReceiptSubLotDTO>>
    {
        public SchedulingQuery()
        {
        }
    }
}
