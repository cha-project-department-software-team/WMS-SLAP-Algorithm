namespace SLAPScheduling.Infrastructure.Repository.Scheduling
{
    public class SchedulingRepository : BaseRepository, ISchedulingRepository
    {
        public SchedulingRepository(SLAPDbContext context) : base(context)
        {
        }

        /// <summary>
        /// POST API to implement the scheduling based on the input data.
        /// </summary>
        /// <param name="warehouseId"></param>
        /// <param name="receiptLotStatus"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<ReceiptSublot>> Execute(string warehouseId, string receiptLotStatus)
        {
            var warehouse = await GetSchedulingWarehouse(warehouseId);
            if (warehouse is null)
            {
                throw new Exception("No result for Warehouse");
            }

            var receiptLots = await GetReceiptLotsByStatus(receiptLotStatus);
            if (receiptLots == null || receiptLots.Count == 0)
            {
                throw new Exception("No result for Receipt Lots");
            }

            var materials = await GetAllMaterials();
            if (materials is null || materials.Count == 0)
            {
                throw new Exception("No result for Receipt Lots");
            }

            MappingMaterialToReceiptLots(materials, ref receiptLots);

            var receiptSubLots = new List<ReceiptSublot>();
            using (var receiptLotSplitter = new ReceiptLotSplitter(receiptLots, warehouse))
            {
                // Receipt Sublots do not include the Locations information
                receiptSubLots = receiptLotSplitter.GetReceiptSubLots().ToList();
            }

            // Order by descending based on the movement ratio of a product
            receiptSubLots = receiptSubLots.OrderByDescending(sublot =>
            {
                var material = sublot.GetMaterial();
                return material is not null ? material.GetMovementRatio() : 0.0;
            }).ToList();

            // Retrieve the available locations (not full) in the warehouse
            var availableLocations = warehouse.locations.Where(x => x.GetCurrentStoragePercentage() < 1.0);

            // Find the optimal solution of location assignment for each receipt sublot using Tabu Search algorithm
            TabuSearch tabuSearch = new TabuSearch(receiptSubLots, availableLocations.ToList());
            List<Location> optimalLocations = tabuSearch.Implement();

            UpdateLocationForReceiptSubLot(optimalLocations, ref receiptSubLots);
            return receiptSubLots;
        }

        private void MappingMaterialToReceiptLots(List<Material> materials, ref List<ReceiptLot> receiptLots)
        {
            var materialDictionary = materials.ToDictionary(x => x.materialId, y => y);

            foreach (var receiptLot in receiptLots)
            {
                var receiptEntry = receiptLot.inventoryReceiptEntry;
                var materialId = receiptEntry is not null ? receiptEntry.materialId : string.Empty;
                if (materialDictionary.TryGetValue(materialId, out Material? material))
                {
                    receiptLot.material = material;
                }
            }
        }

        /// <summary>
        /// Assign the Locations to each ReceiptSubLot based on the optimal solution
        /// </summary>
        /// <param name="locations"></param>
        /// <param name="receiptSubLots"></param>
        private void UpdateLocationForReceiptSubLot(List<Location> locations, ref List<ReceiptSublot> receiptSubLots)
        {
            if (receiptSubLots?.Count > 0 && locations?.Count > 0)
            {
                for (int i = 0; i < receiptSubLots.Count; i++)
                {
                    receiptSubLots[i].UpdateLocation(locations[i]);
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

            return warehouse is not null ? warehouse : throw new Exception($"There is no existing warehouse with warehouseId = {warehouseId}");
        }

        public async Task<List<ReceiptLot>> GetReceiptLotsByStatus(string lotStatus)
        {
            if (!Enum.TryParse<LotStatus>(lotStatus, out var status))
            {
                throw new ArgumentException("Invalid status value", nameof(lotStatus));
            }

            return await _context.ReceiptLots
                        .Where(x => x.receiptLotStatus == status)
                        .Include(x => x.inventoryReceiptEntry)
                        .Include(x => x.receiptSublots)
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
    }
}
