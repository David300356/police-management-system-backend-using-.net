namespace police_backend.Models
{
    public class Assign:modelBase
    {
        public int? ReportId { get; set; }
        public Report Report { get; set; } = new Report();
        public int? PoliceId { get; set; }
        public Police Police { get; set; } = new Police();
        public string? createdOn { get; set; } = String.Empty;
    }
}
