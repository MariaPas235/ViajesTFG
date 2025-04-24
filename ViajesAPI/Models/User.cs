using System.ComponentModel.DataAnnotations;

namespace ViajesAPI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Contraseña { get; set; }
        [Required]
        public string Nombre { get; set; }

        public List<Purchase>? Purchases { get; set; }
        public List<Valoration>? Valorations { get; set; }
    }
}
