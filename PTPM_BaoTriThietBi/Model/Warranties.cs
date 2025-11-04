namespace Model
{
    public class Warranties
    {
        public int WarrantyID { get; set; }
        public int AssetID { get; set; }

        public string? WarrantyProvider { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Terms { get; set; }

        // Đồng bộ với SQL và dùng để lọc/sắp xếp giống Tickets
        public string Status { get; set; } = "Active";
        public DateTime CreatedAt { get; set; }

        // Tiện khi JOIN Assets trong get-all/get-by-id
        public string? AssetName { get; set; }
    }
}