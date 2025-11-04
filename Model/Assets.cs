namespace Model
{
    public class Assets
    {
        // Assets
        public int AssetID { get; set; }                  // AssetID
        public string AssetName { get; set; } = string.Empty; // AssetName
        public string SerialNumber { get; set; } = string.Empty; // SerialNumber
        public string? Location { get; set; }             // Location
        public DateTime? PurchaseDate { get; set; }       // PurchaseDate
        public string Status { get; set; } = string.Empty; // Status
        public DateTime CreatedAt { get; set; }           // CreatedAt
        public string? UsageStatus { get; set; }          // UsageStatus
    }
}