    namespace Model
{
    public class LichBaoTri
    {
        // Schedules
        public int MaLichBaoTri { get; set; }                  // ScheduleID
        public int MaTaiSan { get; set; }                      // AssetID
        public string LoaiBaoTri { get; set; } = string.Empty; // MaintenanceType
        public DateTime NgayBaoTriTiepTheo { get; set; }       // NextMaintenanceDate
        public DateTime? NgayBaoTriGanNhat { get; set; }       // LastMaintenanceDate
        public string? DanhSachKiemTra { get; set; }           // Checklist
    }
}