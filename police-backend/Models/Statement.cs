using System.Reflection.Metadata;

namespace police_backend.Models
{
    public class Statement:modelBase
    {
        public string statement { get; set; }=string.Empty;
        
        public string CreatedOn { get; set; } = string.Empty;

        public int? ReportId { get; set; }
        public Report Report { get; set; } = new Report();
    }
}
