namespace police_backend.Models
{
    public class Caselist : modelBase
    {
        public string CaseName { get; set; }=string.Empty;
        public int CasetypeId { get; set; }
        public Casetype Casetype { get; set; } = new Casetype();
        public List<Report>? Report { get; set; }
    }
}
