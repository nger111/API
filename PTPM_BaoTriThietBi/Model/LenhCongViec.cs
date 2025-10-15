namespace Model
{
    public class LenhCongViec
    {
        // WorkOrders
        public int MaLenhCongViec { get; set; }             // WorkOrderID
        public int? MaLichBaoTri { get; set; }              // ScheduleID (nullable)
        public int? MaVeSuCo { get; set; }                  // TicketID (nullable)
        public int MaTaiSan { get; set; }                   // AssetID
        public int? PhanCongCho { get; set; }               // AssignedTo
        public string LoaiCongViec { get; set; } = string.Empty; // WorkType
        public string? MoTa { get; set; }                   // Description
        public string TrangThai { get; set; } = string.Empty;    // Status
        public DateTime NgayTao { get; set; }               // CreatedAt
        public DateTime? NgayHoanThanh { get; set; }        // CompletedAt
    }
}