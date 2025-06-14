﻿namespace SLAPScheduling.Infrastructure.EFConfigurations.InventoryReceiptConfigurations
{
    public class InventoryReceiptEntryEntityTypeConfiguration : IEntityTypeConfiguration<InventoryReceiptEntry>
    {
        public void Configure(EntityTypeBuilder<InventoryReceiptEntry> builder)
        {
            builder.HasKey(s => s.inventoryReceiptEntryId);
            
            builder.Property(s => s.purchaseOrderNumber)
                .IsRequired();

            builder.Property(s => s.note)
                .HasMaxLength(500);

            builder.HasOne(s => s.receiptLot)
                .WithOne(s => s.inventoryReceiptEntry)
                .HasForeignKey<InventoryReceiptEntry>(s => s.lotNumber)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.inventoryReceipt)
                .WithMany(s => s.entries)
                .HasForeignKey(s => s.InventoryReceiptId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
