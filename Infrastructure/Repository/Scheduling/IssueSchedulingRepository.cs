namespace SLAPScheduling.Infrastructure.Repository.Scheduling
{
    public class IssueSchedulingRepository : BaseRepository, IIssueSchedulingRepository
    {
        public IssueSchedulingRepository(SLAPDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Retrieve API to implement the scheduling based on the input data.
        /// </summary>
        /// <param name="warehouseId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<(IssueSublot SubLot, double StoragePercentage)>> Execute(string warehouseId)
        {
            var warehouse = await GetSchedulingWarehouse(warehouseId);
            if (warehouse is null)
            {
                throw new Exception("No result for Warehouse");
            }

            var issueLots = await GetIssueLotsByStatus(warehouseId, LotStatus.Pending);
            if (issueLots is null || issueLots.Count == 0)
            {
                throw new Exception("No result for Issue Lots");
            }

            var unavailableIssueLots = GetUnavailableIssueLots(issueLots);
            var availableIssueLots = issueLots.Where(x => !unavailableIssueLots.Any(y => y.Equals(x))).ToList();

            IssueLotSplitter issueLotSplitter = new IssueLotSplitter(availableIssueLots);
            var issueSublots = issueLotSplitter.GetIssueSubLots();

            return issueSublots.Select(x => (x, x.GetStoragePercentage())).ToList();
        }

        /// <summary>
        /// Check the availability in the requested quantity in Issue Lots
        /// </summary>
        /// <param name="issueLots"></param>
        /// <returns></returns>
        private IEnumerable<IssueLot> GetUnavailableIssueLots(List<IssueLot> issueLots)
        {
            foreach (var issueLot in issueLots)
            {
                var materialLot = issueLot.materialLot;
                if (materialLot is not null && issueLot.requestedQuantity > materialLot.exisitingQuantity)
                {
                    yield return issueLot;
                }
            }
        }

        public async Task<Warehouse> GetSchedulingWarehouse(string warehouseId)
        {
            var warehouse = await _context.Warehouses
                                .Include(s => s.properties)
                                .Include(s => s.locations)
                                    .ThenInclude(s => s.materialSubLots)
                                        .ThenInclude(s => s.materialLot)
                                .Include(s => s.locations)
                                    .ThenInclude(s => s.properties)
                                .FirstOrDefaultAsync(x => x.warehouseId == warehouseId);

            if (warehouse is not null && warehouse.locations?.Count > 0)
            {
                return warehouse;
            }

            throw new Exception($"There is no existing warehouse with warehouseId = {warehouseId}");
        }

        public async Task<List<IssueLot>> GetIssueLotsByStatus(string warehouseId, LotStatus lotStatus)
        {
            return await _context.IssueLots
                                 .Include(x => x.materialLot)
                                    .ThenInclude(x => x.subLots)
                                        .ThenInclude(x => x.location)
                                            .ThenInclude(x => x.warehouse)
                                 .Include(x => x.materialLot)
                                    .ThenInclude(x => x.material)
                                        .ThenInclude(x => x.properties)
                                 .Include(x => x.inventoryIssueEntry)
                                     .ThenInclude(x => x.inventoryIssue)
                                 .Where(x => x.issueLotStatus == lotStatus && x.inventoryIssueEntry.inventoryIssue.warehouseId == warehouseId)
                                 .ToListAsync();
        }
    }
}
