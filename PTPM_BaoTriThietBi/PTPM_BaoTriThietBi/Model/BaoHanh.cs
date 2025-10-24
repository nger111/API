namespace Model
{
    public class BaoHanh
    {
        // Warranties
        public int MaBaoHanh { get; set; }              // WarrantyID
        public int MaTaiSan { get; set; }               // AssetID
        public string? NhaCungCapBaoHanh { get; set; }  // WarrantyProvider
        public DateTime? NgayBatDau { get; set; }       // StartDate
        public DateTime? NgayKetThuc { get; set; }      // EndDate
        public string? DieuKhoan { get; set; }          // Terms
    }
}