using MediatR;
using Microsoft.AspNetCore.Mvc;
using SLAPScheduling.Application.Commands.Scheduling;
using SLAPScheduling.Domain.AggregateModels.InventoryReceiptAggregate;
using SLAPScheduling.Domain.AggregateModels.MaterialAggregate.Materials;
using SLAPScheduling.Domain.AggregateModels.StorageAggregate.Warehouses;

namespace SLAPScheduling.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulingController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SchedulingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<List<ReceiptSublot>> Post(InventoryReceipt inventoryReceipt, Warehouse warehouse, List<Material> materials)
        {
            return await _mediator.Send(new AddSchedulingCommand(inventoryReceipt, warehouse, materials));
        }
    }
}
