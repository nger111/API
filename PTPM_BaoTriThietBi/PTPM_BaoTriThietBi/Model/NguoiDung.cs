namespace Model
{
    public class NguoiDung
    {
        // Users
        public int MaNguoiDung { get; set; }           // UserID
        public string HoTen { get; set; } = string.Empty; // FullName
        public string Email { get; set; } = string.Empty;
        public string MatKhauHash { get; set; } = string.Empty; // PasswordHash
        public string VaiTro { get; set; } = string.Empty;      // Role
        public string? DienThoai { get; set; }                  // Phone
        public string? TrinhDoKyNang { get; set; }              // SkillLevel
        public string? ChungChi { get; set; }                   // Certifications
        public DateTime NgayTao { get; set; }                   // CreatedAt
    }
}