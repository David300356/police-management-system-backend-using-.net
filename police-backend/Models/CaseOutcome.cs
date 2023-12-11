namespace police_backend.Models
{
    public class CaseOutcome:modelBase
    {
        public int? ReportId { get; set; }
        public Report Report { get; set; } = new Report();
        public int? OutcomeId { get; set; }
        public Outcome Outcome { get; set; } = new Outcome();
        public bool isClosed { get; set; } = false;
        public string? createdOn { get; set; } = String.Empty;
    }
}
