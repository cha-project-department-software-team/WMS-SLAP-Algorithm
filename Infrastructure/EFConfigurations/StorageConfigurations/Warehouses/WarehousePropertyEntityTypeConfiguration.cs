namespace SLAPScheduling.Infrastructure.EFConfigurations.StorageConfigurations.Warehouses
{
    public class WarehousePropertyEntityTypeConfiguration : IEntityTypeConfiguration<WarehouseProperty>
    {
        public void Configure(EntityTypeBuilder<WarehouseProperty> builder)
        {

            builder.HasKey(wp => wp.propertyId);


            builder.HasOne(wp => wp.warehouse)
                .WithMany(w => w.properties)
                .HasForeignKey(wp => wp.warehouseId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);


        }



    }
}
