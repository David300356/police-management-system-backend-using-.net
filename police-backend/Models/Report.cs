using Microsoft.Extensions.Hosting;
using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;

namespace police_backend.Models
{
    public class Report:modelBase
    {
        public string ob { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string phone { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string occupation { get; set; } = string.Empty;
        public string city { get; set; } = string.Empty;
        public string address { get; set; } = string.Empty;
        public string idNumber { get; set; } = string.Empty;
        public string CreatedOn { get; set; } = string.Empty;
        public bool takeFingerprint { get; set; } = false;
        public int? GenderId { get; set; }
        public Gender Gender { get; set; } = new Gender();
        public int? PoliceId { get; set; }
        public Police Police { get; set; } = new Police();
        public int? CasetypeId { get; set; }
        public Casetype Casetype { get; set; } = new Casetype();
        public List<int>? caseList { get; set; }=new List<int> { };
        public List<Witness>? Witnesses { get; set; }
        public List<Suspect>? Suspects { get; set; }
        public List<Statement>?  Statements { get; set; }
        public List<Finding>? Finding { get; set; }
        //public List<Assign>? Assign { get; set; }

        [NotMapped]
        public List<CaseListArray>? CaseListArrays { get; set; }
        [NotMapped]
        public IEnumerable<Suspect>? Suspectss { get; set; }
        [NotMapped]
        public IEnumerable<Witness>? Witnesss { get; set; }
        [NotMapped]
        public IEnumerable<Statement>? Statementss { get; set; }
        [NotMapped]
        public IEnumerable<CaseListArray>? CaseListArrayss { get; set; }

    }
}
