namespace Model
{
    public class PartUsage
    {
        public int PartUsageID { get; set; }
        public int WorkOrderID { get; set; }
        public int PartID { get; set; }
        public int QuantityUsed { get; set; }
        public DateTime UsedAt { get; set; }
    }
}