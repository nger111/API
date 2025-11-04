namespace Model
{
    public class Parts
    {
        public int PartID { get; set; }
        public string PartName { get; set; } = string.Empty;
        public string? PartCode { get; set; }
        public int StockQuantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public string? Location { get; set; }
        public string? UsageStatus { get; set; }
    }
}