using System;
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

    [MaxLength(450)]
    public string? id_operatio { get; set; }  // Ahora nullable y con max length

    [Required]
    public string data { get; set; }

    [Required]
    public string order { get; set; }

    [Required]
    public int UserId { get; set; }

    [JsonIgnore]
    public User? User { get; set; }

    [Required]
    public int TravelId { get; set; }

    [JsonIgnore]
    public Travel? Travel { get; set; }
}
