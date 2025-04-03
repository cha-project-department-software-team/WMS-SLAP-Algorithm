namespace SLAPScheduling.Infrastructure.Repository
{
    public class BaseRepository
    {
        protected readonly SLAPDbContext _context;
        public IUnitOfWork UnitOfWork => _context;


        public BaseRepository(SLAPDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
    }
}
