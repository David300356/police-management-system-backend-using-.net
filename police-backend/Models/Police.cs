namespace police_backend.Models
{
    public class Police:modelBase
    {
        public string name { get; set; } = string.Empty;
        public string phone { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string serialNumber { get; set; } = string.Empty;
        public string idNumber { get; set; } = string.Empty;
        public int? RankId { get; set; }
        public Rank Rank { get; set; } = new Rank();
        public int? StationId { get; set; }
        public Station Station { get; set; } = new Station();
        public string CreatedOn { get; set; } = string.Empty;
        public List<Report>? Report { get; set; }
        //public List<Assign>? Assign { get; set; }
    }
}
