namespace Model
{
    public class Tickets
    {
        public int TicketID { get; set; }          // TicketID
        public int AssetID { get; set; }           // AssetID
        public int CreatedBy { get; set; }         // CreatedBy
        public int? AssignedTo { get; set; }       // AssignedTo, nullable
        public string Priority { get; set; }       // Priority
        public int? SLAHours { get; set; }         // SLAHours, nullable
        public string IssueDescription { get; set; } // IssueDescription
        public string Status { get; set; }         // Status
        public DateTime CreatedAt { get; set; }    // CreatedAt
    }
}
