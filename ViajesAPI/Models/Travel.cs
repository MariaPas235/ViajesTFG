using System.ComponentModel.DataAnnotations;

namespace ViajesAPI.Models
{
    public class Travel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Destination { get; set; }
        [Required]
        public string Description{ get; set; }
        [Required]
        public DateTime InitDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public decimal Price { get; set; }

        public List<Purchase> Purchases { get; set; } = new List<Purchase>();
        public List<Valoration> Valorations { get; set; } = new List<Valoration>();
    }
}
