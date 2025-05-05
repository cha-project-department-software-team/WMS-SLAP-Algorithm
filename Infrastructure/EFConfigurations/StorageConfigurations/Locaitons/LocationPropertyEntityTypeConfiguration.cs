namespace SLAPScheduling.Infrastructure.EFConfigurations.StorageConfigurations.Locaitons
{
    public class LocationPropertyEntityTypeConfiguration : IEntityTypeConfiguration<LocationProperty>
    {
        public void Configure(EntityTypeBuilder<LocationProperty> builder)
        {
            builder.HasKey(b => b.propertyId);

            builder.Property(b => b.propertyName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(b => b.propertyValue)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasOne(b => b.location)
                .WithMany(b => b.properties)
                .HasForeignKey(b => b.locationId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
