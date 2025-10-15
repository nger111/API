namespace Model
{
    public class SuDungLinhKien
    {
        // PartUsages
        public int MaSuDungLinhKien { get; set; }   // PartUsageID
        public int MaLenhCongViec { get; set; }     // WorkOrderID
        public int MaLinhKien { get; set; }         // PartID
        public int SoLuongSuDung { get; set; }      // QuantityUsed
        public DateTime ThoiGianSuDung { get; set; } // UsedAt
    }
}