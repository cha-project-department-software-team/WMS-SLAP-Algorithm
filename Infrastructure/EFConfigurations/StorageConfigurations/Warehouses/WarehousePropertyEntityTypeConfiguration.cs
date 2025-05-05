namespace SLAPScheduling.Infrastructure.EFConfigurations.StorageConfigurations.Warehouses
{
    public class WarehousePropertyEntityTypeConfiguration : IEntityTypeConfiguration<WarehouseProperty>
    {
        public void Configure(EntityTypeBuilder<WarehouseProperty> builder)
        {

            builder.HasKey(wp => wp.propertyId);

            builder.Property(b => b.propertyName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(b => b.propertyValue)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(s => s.unitOfMeasure)
               .HasConversion(
                   v => v.ToString(),
                   v => (UnitOfMeasure)Enum.Parse(typeof(UnitOfMeasure), v))
               .HasMaxLength(50)
               .IsRequired();

            builder.HasOne(wp => wp.warehouse)
                .WithMany(w => w.properties)
                .HasForeignKey(wp => wp.warehouseId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
