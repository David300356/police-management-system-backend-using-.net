namespace police_backend.Models
{
    public class Station:modelBase
    {
        public string name { get; set; } = string.Empty;
        public string phone { get; set; } = string.Empty;
        public string city { get; set; } = string.Empty;
        public string address { get; set; } = string.Empty;
        public string code { get; set; } = string.Empty;
        public string CreatedOn { get; set; } = string.Empty;

        public List<Police>? Police { get; set; }
    }
}
