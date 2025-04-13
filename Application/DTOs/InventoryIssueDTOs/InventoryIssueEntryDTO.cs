namespace SLAPScheduling.Application.DTOs.InventoryIssueDTOs
{
    public class InventoryIssueEntryDTO
    {
        public string InventoryIssueEntryId { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public double RequestedQuantity { get; set; }
        public string Note { get; set; }
        public string MaterialName { get; set; }
        public string MaterialId { get; set; }
        public string InventoryIssueId { get; set; }
        public IssueLotDTO IssueLot { get; set; }

        public InventoryIssueEntryDTO()
        {
        }

        public InventoryIssueEntryDTO(string inventoryIssueEntryId, string purchaseOrderNumber, double requestedQuantity, string note, string materialName, string materialId, string inventoryIssueId)
        {
            InventoryIssueEntryId = inventoryIssueEntryId;
            PurchaseOrderNumber = purchaseOrderNumber;
            RequestedQuantity = requestedQuantity;
            Note = note;
            MaterialName = materialName;
            MaterialId = materialId;
            InventoryIssueId = inventoryIssueId;
        }

        public void MapName(string materialName)
        {
            MaterialName = materialName;
        }

    }
}
