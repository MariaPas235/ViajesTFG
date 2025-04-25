using System.ComponentModel.DataAnnotations;

namespace ViajesAPI.Models
{
    public class Valoration
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Punctuation { get; set; }
        [Required]
        public string Comment { get; set; }

        public User User { get; set; }
        public Travel Travel { get; set; }
    }
}
