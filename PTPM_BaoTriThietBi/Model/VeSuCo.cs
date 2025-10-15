namespace Model
{
    public class VeSuCo
    {
        public int MaVeSuCo { get; set; }          // TicketID
        public int MaTaiSan { get; set; }          // AssetID
        public int TaoBoi { get; set; }            // CreatedBy
        public int? PhanCongCho { get; set; }      // AssignedTo
        public string MucDoUuTien { get; set; } = string.Empty; // Priority
        public int GioSLA { get; set; }            // SLAHours
        public string? MoTaSuCo { get; set; }      // IssueDescription
        public string TrangThai { get; set; } = string.Empty;   // Status
        public DateTime NgayTao { get; set; }      // CreatedAt
    }
}