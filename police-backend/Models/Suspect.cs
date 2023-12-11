using System.ComponentModel.DataAnnotations.Schema;

namespace police_backend.Models
{
    public class Suspect:modelBase
    {
        public string name { get; set; } = string.Empty;
        public string idNumber { get; set; } = string.Empty;
        public string phone { get; set; } = string.Empty;
        public string city { get; set; } = string.Empty;
        public string address { get; set; } = string.Empty;
        public string CreatedOn { get; set; } = string.Empty;
        public string Sstatement { get; set; } = string.Empty;
        public int? ReportId { get; set; }
        public Report Report { get; set; } = new Report();

    }
}
