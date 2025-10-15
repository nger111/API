namespace Model
{
    public class Ticket
    {
        public int TicketID { get; set; }
        public int AssetID { get; set; }
        public int CreatedBy { get; set; }
        public int? AssignedTo { get; set; }
        public string Priority { get; set; } = string.Empty;
        public int SLAHours { get; set; }
        public string? IssueDescription { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}