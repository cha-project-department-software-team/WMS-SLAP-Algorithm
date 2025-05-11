using SLAPScheduling.Algorithm.DifferentialEvolutions;

namespace SLAPScheduling.Infrastructure.Repository.Scheduling
{
    public class ReceiptSchedulingRepository : BaseRepository, IReceiptSchedulingRepository
    {
        public ReceiptSchedulingRepository(SLAPDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Retrieve API to implement the scheduling based on the input data.
        /// </summary>
        /// <param name="warehouseId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<(ReceiptSublot SubLot, double StoragePercentage)>> Execute(string warehouseId)
        {
            var warehouse = await GetSchedulingWarehouse(warehouseId);
            if (warehouse is null)
            {
                throw new Exception("No result for Warehouse");
            }

            var receiptLots = await GetReceiptLotsByStatus(warehouseId, LotStatus.Pending);
            if (receiptLots == null || receiptLots.Count == 0)
            {
                throw new Exception("No result for Receipt Lots");
            }

            var materials = await GetAllMaterials();
            if (materials is null || materials.Count == 0)
            {
                throw new Exception("No result for Receipt Lots");
            }

            UpdateMaterialForReceiptLots(materials, ref receiptLots);

            var receiptSubLots = new List<ReceiptSublot>();
            using (var receiptLotSplitter = new ReceiptLotSplitter(receiptLots, warehouse))
            {
                // Receipt Sublots do not include the Locations information
                receiptSubLots = receiptLotSplitter.GetReceiptSubLots().ToList();

                // Order by descending based on the movement ratio of a product
                receiptSubLots = receiptSubLots.OrderByDescending(sublot =>
                {
                    var material = sublot.GetMaterial();
                    return material is not null ? material.GetMovementRatio() : 0.0;
                }).ToList();
            }

            // Retrieve the available locations (not full) in the warehouse
            var locations = warehouse.locations;
            UpdateMaterialForMaterialLots(materials, ref locations);

            //var locationInformation = locations.Where(x => x.GetCurrentStoragePercentage() > 0.0).Select(x =>
            //{
            //    var lotNumbers = x.materialSubLots.Select(x => (x.materialLot.lotNumber, x.materialLot.exisitingQuantity)).ToList();
            //    return (x.locationId, lotNumbers, x.GetCurrentStoragePercentage());
            //}).ToList();

            var availableLocations = GetAvailableLocations(locations, receiptLots);
            ConstraintsChecking.SetPenaltyCoefficientValues(availableLocations);

            // Find the optimal solution of location assignment for each receipt sublot using Tabu Search algorithm
            TabuSearch tabuSearch = new TabuSearch(receiptSubLots, availableLocations.ToList());
            List<Location> optimalLocations = tabuSearch.Implement();

            //// Find the optimal solution of location assignment for each receipt sublot using Genetic Algorithm
            //GeneticAlgorithms GA = new GeneticAlgorithms(receiptSubLots, availableLocations.ToList());
            //List<Location> optimalLocations = GA.Implement();

            //// Find the optimal solution of location assignment for each receipt sublot using Differential Evolution
            //DESolver DESolver = new DESolver();
            //List<Location> optimalLocations = DESolver.Implement(receiptSubLots, availableLocations.ToList());

            ReceiptSublotReallocation receiptLotReallocation = new ReceiptSublotReallocation();
            var results = receiptLotReallocation.Reallocate(optimalLocations, receiptSubLots);

            //var results = AssignLocationsForReceiptSubLots(optimalLocations, receiptSubLots);
            return results.ToList();
        }

        /// <summary>
        /// Assign the Locations to each ReceiptSubLot based on the optimal solution
        /// </summary>
        /// <param name="locations"></param>
        /// <param name="receiptSubLots"></param>
        private IEnumerable<(ReceiptSublot SubLot, double StoragePercentage)> AssignLocationsForReceiptSubLots(List<Location> locations, List<ReceiptSublot> receiptSubLots)
        {
            if (receiptSubLots?.Count > 0 && locations?.Count > 0)
            {
                for (int i = 0; i < receiptSubLots.Count; i++)
                {
                    receiptSubLots[i].UpdateLocation(locations[i]);
                }

                foreach (var receiptSubLot in receiptSubLots)
                {
                    var location = receiptSubLot.location;
                    var storagePercentage = receiptSubLot.GetStoragePercentage(location);

                    yield return (receiptSubLot, storagePercentage);
                }
            }
        }


        /// <summary>
        /// Assign the Material to each receiptLot from materialId in ReceiptEntry
        /// </summary>
        /// <param name="materials"></param>
        /// <param name="receiptLots"></param>
        private void UpdateMaterialForReceiptLots(List<Material> materials, ref List<ReceiptLot> receiptLots)
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
        /// Assign the Material to each receiptLot from materialId in ReceiptEntry
        /// </summary>
        /// <param name="materials"></param>
        /// <param name="receiptLots"></param>
        private void UpdateMaterialForMaterialLots(List<Material> materials, ref List<Location> locations)
        {
            var materialDictionary = materials.ToDictionary(x => x.materialId, y => y);

            foreach (var location in locations)
            {
                var materialSublots = location.materialSubLots;
                if (materialSublots?.Count > 0)
                {
                    foreach (var materialSublot in materialSublots)
                    {
                        var materialLot = materialSublot.materialLot;
                        var materialId = materialLot is not null ? materialLot.materialId : string.Empty;
                        if (materialLot is not null && materialDictionary.TryGetValue(materialId, out Material? material))
                        {
                            materialLot.material = material;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Filter the locations which is not full and has the storage level is lower than max acceptable level of receipt material
        /// </summary>
        /// <param name="locations"></param>
        /// <param name="receiptLots"></param>
        /// <returns></returns>
        private IEnumerable<Location> GetAvailableLocations(List<Location> locations, List<ReceiptLot> receiptLots)
        {
            var maxAcceptableLevel = receiptLots.Max(lot =>
            {
                var material = lot.material;
                return material is not null ? material.GetLimitStorageLevel() : 0.0;
            });

            return locations.Where(location => location.GetCurrentStoragePercentage() < 1.0 && location.GetStorageLevel() <= maxAcceptableLevel && location.GetRowIndex() == 1);
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

        public async Task<List<ReceiptLot>> GetReceiptLotsByStatus(string warehouseId, LotStatus lotStatus)
        {
            return await _context.ReceiptLots
                                .Include(x => x.inventoryReceiptEntry)
                                    .ThenInclude(x => x.inventoryReceipt)
                                .Where(x => x.receiptLotStatus == lotStatus && x.inventoryReceiptEntry.inventoryReceipt.warehouseId == warehouseId)
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
                                 .Where(x => x.locationId == locationId)
                                 .Include(x => x.materialLot)
                                 .ToListAsync();
        }
    }
}
