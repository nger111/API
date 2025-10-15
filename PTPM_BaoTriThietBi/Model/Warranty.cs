namespace Model
{
    public class Warranty
    {
        public int WarrantyID { get; set; }
        public int AssetID { get; set; }
        public string? WarrantyProvider { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Terms { get; set; }
    }
}