namespace police_backend.Models
{
    public class Interview:modelBase
    {
        public string? name { get; set; } = string.Empty;
        public string? idNumber { get; set; } = string.Empty;
        public string? description { get; set; } = string.Empty;
        public string CreatedOn { get; set; } = string.Empty;
        public int? FindingId { get; set; }
        public Finding Finding { get; set; } = new Finding();
    }
}
