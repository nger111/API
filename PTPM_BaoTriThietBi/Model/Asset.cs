namespace Model
{
    public class Asset
    {
        public int AssetID { get; set; }
        public string AssetName { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public string? Location { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}