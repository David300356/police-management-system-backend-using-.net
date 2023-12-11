using System.ComponentModel.DataAnnotations.Schema;

namespace police_backend.Models
{
    public class Finding:modelBase
    {
        public int? ReportId { get; set; }
        public Report Report { get; set; } = new Report();
        public string? description { get; set; }=string.Empty;
        public string CreatedOn { get; set; } = string.Empty;
        public List<Evidence>? Evidencess { get; set; }
        public List<Interview>? Interviewss { get; set; }
        [NotMapped]
        public IEnumerable<Evidence>? Evidencesss { get; set; }
        [NotMapped]
        public IEnumerable<Interview>? Interviewsss { get; set; }
    }
}
