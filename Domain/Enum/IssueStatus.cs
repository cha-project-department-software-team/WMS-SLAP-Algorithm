namespace SLAPScheduling.Domain.Enum
{
    public enum IssueStatus
    {
        Draft,
        PendingApproval,
        Approved,
        Picking,
        Packed,
        Shipped,
        Completed,
        Cancelled,
        Pending
    }
}
