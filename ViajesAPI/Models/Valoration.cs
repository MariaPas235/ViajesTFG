using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        // Claves foráneas
        public int UserId { get; set; }

        public int TravelId { get; set; }

    }
}
