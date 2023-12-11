namespace police_backend.Models
{
    public class Court:modelBase
    {
        public string Name { get; set; } = string.Empty;
        public string CourtDate { get; set; } = string.Empty;
        public string DocketNo { get; set; } = string.Empty;
        public int? ReportId { get; set; }
        public Report Report { get; set; } = new Report();
    }
}
