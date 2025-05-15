using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ViajesAPI.Models;

public class Purchase
{
    [Key]
    public int Id { get; set; }
    [Required]
    public DateTime PurchaseDate { get; set; }
    [Required]
    public bool State { get; set; }
    [Required]
    public string id_operatio { get; set; }
    [Required]
    public string data { get; set; }
    [Required]
    public string order { get; set; }

    public int UserId { get; set; }
    [JsonIgnore]
    public User User { get; set; }  // Navegación correcta

    public int TravelId { get; set; }
    [JsonIgnore]
    public Travel Travel { get; set; }  // Navegación correcta
}
