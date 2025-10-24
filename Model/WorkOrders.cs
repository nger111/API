namespace Model
{
    public class WorkOrders
    {
        public int WorkOrderID { get; set; }
        public int? ScheduleID { get; set; }
        public int? TicketID { get; set; }
        public int AssetID { get; set; }
        public int? AssignedTo { get; set; }
        public string WorkType { get; set; } = string.Empty;        public string? Description { get; set; }
        public string Status { get; set; } = string.Empty;
        public string UsageStatus { get; set; } = "Active";
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }

        // Tiện join
        public string? AssetName { get; set; }
        public string? AssignedToName { get; set; }
    }
}