namespace Model
{
    public class TaiSan
    {
        // Assets
        public int MaTaiSan { get; set; }                 // AssetID
        public string TenTaiSan { get; set; } = string.Empty; // AssetName
        public string SoSerial { get; set; } = string.Empty;   // SerialNumber
        public string? ViTri { get; set; }                       // Location
        public DateTime? NgayMua { get; set; }                   // PurchaseDate
        public string TrangThai { get; set; } = string.Empty;    // Status
        public DateTime NgayTao { get; set; }                    // CreatedAt
    }
}