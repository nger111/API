namespace Model
{
    public class LinhKien
    {
        // Parts
        public int MaLinhKien { get; set; }                 // PartID
        public string TenLinhKien { get; set; } = string.Empty; // PartName
        public string? MaLinhKienCode { get; set; }         // PartCode
        public int SoLuongTon { get; set; }                 // StockQuantity
        public decimal? DonGia { get; set; }                // UnitPrice
        public string? ViTri { get; set; }                  // Location
    }
}