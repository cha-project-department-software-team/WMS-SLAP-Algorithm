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
        /// <param name="issueLotStatus"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<IssueSublot>> Execute(string warehouseId, string issueLotStatus)
        {
            var warehouse = await GetSchedulingWarehouse(warehouseId);
            if (warehouse is null)
            {
                throw new Exception("No result for Warehouse");
            }

            var issueLots = await GetIssueLotsByStatus(issueLotStatus);
            if (issueLots is null || issueLots.Count == 0)
            {
                throw new Exception("No result for Issue Lots");
            }

            var materials = await GetAllMaterials();
            if (materials is null || materials.Count == 0)
            {
                throw new Exception("No result for Materials"); 
            }

            UpdateMaterialForIssueLots(materials, ref issueLots);

            var unavailableIssueLots = GetUnavailableIssueLots(issueLots);
            if (unavailableIssueLots?.Count() > 0)
            {
                throw new Exception($"The requested quantity is over the existing quantity in issue lots: {string.Join(',', issueLots.Select(x => x.issueLotId).ToList())}");
            }

            IssueLotSplitter issueLotSplitter = new IssueLotSplitter(issueLots);
            var issueSublots = issueLotSplitter.GetIssueSubLots().ToList();

            return issueSublots;
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


        /// <summary>
        /// Assign the Material to each issueLot from materialId in IssueEntry
        /// </summary>
        /// <param name="materials"></param>
        /// <param name="issueLots"></param>
        private void UpdateMaterialForIssueLots(List<Material> materials, ref List<IssueLot> issueLots)
        {
            var materialDictionary = materials.ToDictionary(x => x.materialId, y => y);
            foreach (var issueLot in issueLots)
            {
                var issueEntry = issueLot.inventoryIssueEntry;
                var materialId = issueEntry is not null ? issueEntry.materialId : string.Empty;
                if (materialDictionary.TryGetValue(materialId, out Material? material))
                {
                    issueLot.material = material;
                }
            }
        }

        public async Task<Warehouse> GetSchedulingWarehouse(string warehouseId)
        {
            var warehouse = await _context.Warehouses
                                .Include(s => s.properties)
                                .Include(s => s.locations)
                                    .ThenInclude(s => s.properties)
                                .FirstOrDefaultAsync(x => x.warehouseId == warehouseId);

            if (warehouse is not null && warehouse.locations?.Count > 0)
            {
                foreach (var location in warehouse.locations)
                {
                    var locationId = location.locationId;
                    var materialSublots = await GetMaterialSubLotsByLocationId(locationId);
                    if (materialSublots?.Count > 0)
                    {
                        location.materialSubLots = materialSublots;
                    }
                }

                return warehouse;
            }

            throw new Exception($"There is no existing warehouse with warehouseId = {warehouseId}");
        }

        public async Task<List<IssueLot>> GetIssueLotsByStatus(string lotStatus)
        {
            if (!Enum.TryParse<LotStatus>(lotStatus, out var status))
            {
                throw new ArgumentException("Invalid status value", nameof(lotStatus));
            }

            return await _context.IssueLots
                        .Where(x => x.issueLotStatus == status)
                        .Include(x => x.materialLot)
                        .Include(x => x.inventoryIssueEntry)
                        .Include(x => x.issueSublots)
                        .ToListAsync();
        }

        public async Task<List<Material>> GetAllMaterials()
        {
            var materials = await _context.Materials
                                .Include(x => x.properties)
                                .AsNoTracking()
                                .ToListAsync();

            return materials;
        }

        public async Task<List<MaterialSubLot>> GetMaterialSubLotsByLocationId(string locationId)
        {
            return await _context.MaterialSubLots
                                 .Include(x => x.materialLot)
                                 .Where(x => x.locationId == locationId).ToListAsync();
        }
    }
}
