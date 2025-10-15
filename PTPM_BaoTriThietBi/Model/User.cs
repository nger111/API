namespace Model
{
    public class User
    {
        public int UserID { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? SkillLevel { get; set; }
        public string? Certifications { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

