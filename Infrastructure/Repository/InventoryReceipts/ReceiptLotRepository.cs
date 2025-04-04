namespace SLAPScheduling.Infrastructure.Repository.InventoryReceipts
{
    public class ReceiptLotRepository : BaseRepository, IReceiptLotRepository
    {
        public ReceiptLotRepository(SLAPDbContext context) : base(context)
        {
        }

        public async Task<List<ReceiptLot>> GetAllAsync()
        {
            var receiptLots = await _context.ReceiptLots
                                    .AsNoTracking()
                                    .ToListAsync();

            return receiptLots;
        }
    }
}
