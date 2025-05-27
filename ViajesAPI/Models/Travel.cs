using System.ComponentModel.DataAnnotations;

namespace ViajesAPI.Models
{
    public class Travel
    {
        /// <summary>
        /// Identificador único del viaje.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Destino del viaje (obligatorio).
        /// </summary>
        [Required]
        public string Destination { get; set; }

        /// <summary>
        /// Descripción del viaje (obligatorio).
        /// </summary>
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// Fecha de inicio del viaje (obligatorio).
        /// </summary>
        [Required]
        public DateTime InitDate { get; set; }

        /// <summary>
        /// Fecha de fin del viaje (obligatorio).
        /// </summary>
        [Required]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Precio del viaje (obligatorio).
        /// </summary>
        [Required]
        public decimal Price { get; set; }

        /// <summary>
        /// Imagen relacionada al viaje (opcional).
        /// Puede ser una URL o ruta de archivo.
        /// </summary>
        public string? Image { get; set; }

        /// <summary>
        /// Latitud geográfica del destino (obligatorio).
        /// </summary>
        [Required]
        public decimal Latitud { get; set; }

        /// <summary>
        /// Longitud geográfica del destino (obligatorio).
        /// </summary>
        [Required]
        public decimal Longitud { get; set; }

        /// <summary>
        /// Cantidad disponible para el viaje (obligatorio).
        /// Por ejemplo, número de plazas o tickets disponibles.
        /// </summary>
        [Required]
        public int Cantidad { get; set; }

        /// <summary>
        /// Lista de compras relacionadas a este viaje.
        /// Relación uno a muchos con la entidad Purchase.
        /// </summary>
        public List<Purchase> Purchases { get; set; } = new List<Purchase>();

        /// <summary>
        /// Lista de valoraciones relacionadas a este viaje.
        /// Relación uno a muchos con la entidad Valoration.
        /// </summary>
        public List<Valoration> Valorations { get; set; } = new List<Valoration>();
    }
}
