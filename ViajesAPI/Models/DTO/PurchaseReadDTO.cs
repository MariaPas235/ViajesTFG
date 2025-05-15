using System.ComponentModel.DataAnnotations;

namespace ViajesAPI.Models.DTO
{
    public class PurchaseReadDTO
    {
        public int Id { get; set; }
        public DateTime PurchaseDate { get; set; }
        public bool State { get; set; }
        public string id_operatio { get; set; }
        public string data { get; set; }
        public string order { get; set; }
        public UserDTO User { get; set; }
        public TravelDTO Travel { get; set; }
    }

    public class UserDTO
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
    }

    public class TravelDTO
    {
        public int Id { get; set; }
        public string Destination { get; set; }
        public string Description { get; set; }
        public DateTime InitDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Price { get; set; }
        public string? Image { get; set; }

    }

}
