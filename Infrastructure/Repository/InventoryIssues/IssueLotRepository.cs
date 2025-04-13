using SLAPScheduling.Domain.InterfaceRepositories.IInventoryIssues;

namespace SLAPScheduling.Infrastructure.Repository.InventoryIssues
{
    public class IssueLotRepository : BaseRepository, IIssueLotRepository
    {
        public IssueLotRepository(SLAPDbContext context) : base(context)
        {
        }

        public async Task<List<IssueLot>> GetAllAsync()
        {
            var issueLots = await _context.IssueLots
                                    .AsNoTracking()
                                    .ToListAsync();

            return issueLots;
        }
    }
}
