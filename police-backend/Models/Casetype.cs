namespace police_backend.Models
{
    public class Casetype:modelBase
    {
        public string name { get; set; } = string.Empty;
        public List<Report>? Report { get; set; }
    }
}
