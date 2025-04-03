namespace SLAPScheduling.Infrastructure.EFConfigurations.StorageConfigurations.Locaitons
{
    public class LocationPropertyEntityTypeConfiguration : IEntityTypeConfiguration<LocationProperty>
    {
        public void Configure(EntityTypeBuilder<LocationProperty> builder)
        {
            builder.HasKey(b => b.propertyId);

            builder.HasOne(b => b.location)
                .WithMany(b => b.properties)
                .HasForeignKey(b => b.locationId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);



        }


    }
}
