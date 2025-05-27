using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ViajesAPI.Models;

public class Purchase
{
    /// <summary>
    /// Identificador único de la compra.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Fecha en que se realizó la compra.
    /// </summary>
    [Required]
    public DateTime PurchaseDate { get; set; }

    /// <summary>
    /// Estado actual de la compra (ejemplo: "pending", "completed").
    /// Valor por defecto: "pending".
    /// </summary>
    [Required]
    public string State { get; set; } = "pending";

    /// <summary>
    /// Identificador de la operación, puede ser null y tiene un tamaño máximo de 450 caracteres.
    /// </summary>
    [MaxLength(450)]
    public string? id_operatio { get; set; }

    /// <summary>
    /// Datos adicionales relacionados con la compra (obligatorio).
    /// </summary>
    [Required]
    public string data { get; set; }

    /// <summary>
    /// Identificador o referencia del pedido (obligatorio).
    /// </summary>
    [Required]
    public string order { get; set; }

    /// <summary>
    /// Identificador del usuario que realizó la compra (clave foránea).
    /// </summary>
    [Required]
    public int UserId { get; set; }

    /// <summary>
    /// Navegación a la entidad User (no se serializa en JSON).
    /// </summary>
    [JsonIgnore]
    public User? User { get; set; }

    /// <summary>
    /// Identificador del viaje asociado a la compra (clave foránea).
    /// </summary>
    [Required]
    public int TravelId { get; set; }

    /// <summary>
    /// Navegación a la entidad Travel (no se serializa en JSON).
    /// </summary>
    [JsonIgnore]
    public Travel? Travel { get; set; }

    /// <summary>
    /// Destino del viaje comprado (obligatorio).
    /// </summary>
    [Required]
    public string Destino { get; set; }

    /// <summary>
    /// Fecha de inicio del viaje comprado (obligatorio).
    /// </summary>
    [Required]
    public DateTime InitDate { get; set; }

    /// <summary>
    /// Fecha de fin del viaje comprado (obligatorio).
    /// </summary>
    [Required]
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Precio pagado por el viaje (obligatorio).
    /// </summary>
    [Required]
    public decimal Price { get; set; }

    /// <summary>
    /// URL o ruta de imagen asociada al viaje (opcional).
    /// </summary>
    public string? Image { get; set; }

    /// <summary>
    /// Estado de la devolución o reembolso ("none" por defecto).
    /// </summary>
    public string RefundStatus { get; set; } = "none";
}
