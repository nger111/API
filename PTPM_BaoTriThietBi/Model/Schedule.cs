namespace Model
{
    public class Schedule
    {
        public int ScheduleID { get; set; }
        public int AssetID { get; set; }
        public string MaintenanceType { get; set; } = string.Empty;
        public DateTime NextMaintenanceDate { get; set; }
        public DateTime? LastMaintenanceDate { get; set; }
        public string? Checklist { get; set; }
    }
}