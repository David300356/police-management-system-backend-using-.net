namespace police_backend.Models
{
    public class ArrestItem:modelBase
    {
        public string? item { get; set; } = string.Empty;
        public int? ArrestId { get; set; }
        public Arrest Arrest { get; set; } = new Arrest();
    }
}
