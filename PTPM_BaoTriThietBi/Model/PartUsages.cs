namespace Model
{
    public class PartUsages
    {
        public int PartUsageID { get; set; }
        public int WorkOrderID { get; set; }
        public int PartID { get; set; }
        public int QuantityUsed { get; set; }
        public DateTime CreatedAt { get; set; }

        // Thuận tiện khi join
        public string? PartName { get; set; }
    }
}