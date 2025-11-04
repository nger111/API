namespace Model
{
    public class Schedules
    {
        public int ScheduleID { get; set; }
        public int AssetID { get; set; }

        public string MaintenanceType { get; set; } = string.Empty; // Monthly/Quarterly/Yearly
        public DateTime NextMaintenanceDate { get; set; }
        public DateTime? LastMaintenanceDate { get; set; }
        public string? Checklist { get; set; }
        public string UsageStatus { get; set; } = "Active";

        // tiện khi join
        public string? AssetName { get; set; }
    }
}