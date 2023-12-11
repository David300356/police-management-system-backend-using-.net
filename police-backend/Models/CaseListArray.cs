using System.ComponentModel.DataAnnotations.Schema;

namespace police_backend.Models
{
    public class CaseListArray:modelBase
    {
        public int? CaselistId { get; set; }
        public Caselist Caselist { get; set; } = new Caselist();
        public int? ReportId { get; set; }
        public Report Report { get; set; } = new Report();
    }
}
