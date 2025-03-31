using Microsoft.EntityFrameworkCore.Storage;
using SLAPScheduling.Domain.AggregateModels.InventoryIssueAggregate;
using SLAPScheduling.Domain.AggregateModels.InventoryLogAggregate;
using SLAPScheduling.Domain.AggregateModels.InventoryReceiptAggregate;
using SLAPScheduling.Domain.AggregateModels.MaterialAggregate.MaterialClasses;
using SLAPScheduling.Domain.AggregateModels.MaterialAggregate.MaterialLots;
using SLAPScheduling.Domain.AggregateModels.MaterialAggregate.Materials;
using SLAPScheduling.Domain.AggregateModels.MaterialAggregate.MaterialSubLots;
using SLAPScheduling.Domain.AggregateModels.PartyAggregate.Customers;
using SLAPScheduling.Domain.AggregateModels.PartyAggregate.People;
using SLAPScheduling.Domain.AggregateModels.PartyAggregate.Suppliers;
using SLAPScheduling.Domain.AggregateModels.StorageAggregate.Warehouses;

namespace SLAPScheduling.Infrastructure
{
    public class SLAPDbContext : DbContext, IUnitOfWork
    {
        // Party Aggregate
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<PersonProperty> PersonProperties { get; set; }

        // Storage Aggregate
        public DbSet<Location> Locations { get; set; }
        public DbSet<LocationProperty> LocationProperties { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<WarehouseProperty> WarehouseProperties { get; set; }

        // Material Aggregate
        public DbSet<Material> Materials { get; set; }
        public DbSet<MaterialProperty> MaterialProperties { get; set; }
        public DbSet<MaterialClass> MaterialClasses { get; set; }
        public DbSet<MaterialClassProperty> MaterialClassProperties { get; set; }
        public DbSet<MaterialLot> MaterialLots { get; set; }
        public DbSet<MaterialLotProperty> MaterialLotProperties { get; set; }
        public DbSet<MaterialSubLot> MaterialSubLots { get; set; }

        // Inventory Receipt Aggregate
        public DbSet<InventoryReceipt> InventoryReceipts { get; set; }
        public DbSet<InventoryReceiptEntry> InventoryReceiptEntries { get; set; }
        public DbSet<ReceiptLot> ReceiptLots { get; set; }
        public DbSet<ReceiptSublot> ReceiptSublots { get; set; }

        // Inventory Issue Aggregate
        public DbSet<InventoryIssue> InventoryIssues { get; set; }
        public DbSet<InventoryIssueEntry> InventoryIssueEntries { get; set; }
        public DbSet<IssueLot> IssueLots { get; set; }
        public DbSet<IssueSublot> IssueSublots { get; set; }

        // InventoryLog Aggregate
        public DbSet<InventoryLog> InventoryLogs { get; set; }

        // MaterialLotAdjustment Aggregate
        //public DbSet<MaterialLotAdjustment> MaterialLotAdjustments { get; set; }

        private readonly IMediator _mediator;
        private IDbContextTransaction? _currentTransaction;
        public IDbContextTransaction? GetCurrentTransaction() => _currentTransaction;
        public bool HasActiveTransaction => _currentTransaction != null;

//#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
//        public SLAPDbContext(DbContextOptions options) : base(options) { }
//#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public SLAPDbContext(DbContextOptions options, IMediator mediator) : base(options)
        {
            _mediator = mediator;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            

            modelBuilder.Entity<IssueLot>()
                .HasOne(il => il.inventoryIssueEntry)  
                .WithOne(iie => iie.issueLot)       
                .HasForeignKey<InventoryIssueEntry>(iie => iie.issueLotId) 
                .OnDelete(DeleteBehavior.Cascade); 






            base.OnModelCreating(modelBuilder);
        }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            await _mediator.DispatchDomainEventsAsync(this);
            await base.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<IDbContextTransaction?> BeginTransactionAsync()
        {
            if (_currentTransaction != null) return null;

            _currentTransaction = await Database.BeginTransactionAsync();

            return _currentTransaction;
        }
        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

            try
            {
                await SaveChangesAsync();
                transaction.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }
    }
}
