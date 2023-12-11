using System.ComponentModel.DataAnnotations;

namespace police_backend.Models
{
    public class Rank:modelBase
    {
        public string? rankName{ get; set; }

        public List<Police>? Police { get; set; }
    }
}
