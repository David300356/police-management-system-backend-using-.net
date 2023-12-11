using System.ComponentModel.DataAnnotations.Schema;

namespace police_backend.Models
{
    public class Arrest:modelBase
    {
        public int? ReportId { get; set; }
        public Report Report { get; set; } = new Report();
        public int? SuspectId { get; set; }
        public Suspect Suspect { get; set; } = new Suspect();
        public int? CellListId { get; set; }
        public CellList CellList { get; set; } = new CellList();
        public string CreatedOn { get; set; } = string.Empty;
        public List<ArrestItem>? ArrestItem { get; set; }
        [NotMapped]
        public IEnumerable<Suspect>? Suspectss { get; set; }
        //[NotMapped]
        //public Suspect? Suspectss { get; set; }
        [NotMapped]
        public IEnumerable<ArrestItem>? ArrestItemss { get; set; }
    }
}
