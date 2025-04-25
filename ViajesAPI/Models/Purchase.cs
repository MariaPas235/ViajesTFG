using System.ComponentModel.DataAnnotations;

namespace ViajesAPI.Models
{
    public class Purchase
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime PurchaseDate { get; set; }
        [Required]
        public bool State { get; set; }
        [Required]
        public string id_operatio { get; set; } //dado por Redsys a la hora de confirmar la compra 
        [Required]
        public string data { get; set; }
        [Required]
        public string order { get; set; }

        public User User { get; set; }
        public Travel Travel { get; set; }
    }
}
