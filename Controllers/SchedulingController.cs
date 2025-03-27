using MediatR;
using Microsoft.AspNetCore.Mvc;
using SLAP.AggregateModels.InventoryReceiptAggregate;
using SLAP.AggregateModels.MaterialAggregate;
using SLAP.AggregateModels.StorageAggregate;
using SLAP.Commands.Inputs;

namespace SLAP.Controllers
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
        public async Task<List<ReceiptSubLot>> Post(InventoryReceipt inventoryReceipt, Warehouse warehouse, List<Material> materials)
        {
            return await _mediator.Send(new AddSchedulingCommand(inventoryReceipt, warehouse, materials));
        }
    }
}
