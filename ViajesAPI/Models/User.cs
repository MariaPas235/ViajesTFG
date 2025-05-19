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
        public string Password { get; set; }
        [Required]
        public string Name { get; set; }
        public string Role { get; set; } 

        public List<Purchase>? Purchases { get; set; }
        public List<Valoration>? Valorations { get; set; }
    }
}
