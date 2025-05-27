using System.ComponentModel.DataAnnotations;

namespace ViajesAPI.Models.DTO
{
    /// <summary>
    /// DTO para mostrar información detallada de una compra.
    /// Incluye datos de la compra, el usuario y el viaje asociado.
    /// </summary>
    public class PurchaseReadDTO
    {
        /// <summary>
        /// Identificador único de la compra.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Fecha en que se realizó la compra.
        /// </summary>
        public DateTime PurchaseDate { get; set; }

        /// <summary>
        /// Estado de la compra (activo/inactivo).
        /// </summary>
        public bool State { get; set; }

        /// <summary>
        /// Identificador de la operación de pago.
        /// </summary>
        public string id_operatio { get; set; }

        /// <summary>
        /// Información adicional de la compra.
        /// </summary>
        public string data { get; set; }

        /// <summary>
        /// Orden asociada a la compra.
        /// </summary>
        public string order { get; set; }

        /// <summary>
        /// Información del usuario que realizó la compra.
        /// </summary>
        public UserDTO User { get; set; }

        /// <summary>
        /// Información del viaje asociado a la compra.
        /// </summary>
        public TravelDTO Travel { get; set; }
    }

    /// <summary>
    /// DTO para representar datos básicos de un usuario.
    /// </summary>
    public class UserDTO
    {
        /// <summary>
        /// Identificador único del usuario.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Correo electrónico del usuario.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Nombre completo del usuario.
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// DTO para representar datos básicos de un viaje.
    /// </summary>
    public class TravelDTO
    {
        /// <summary>
        /// Identificador único del viaje.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Destino del viaje.
        /// </summary>
        public string Destination { get; set; }

        /// <summary>
        /// Descripción del viaje.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Fecha de inicio del viaje.
        /// </summary>
        public DateTime InitDate { get; set; }

        /// <summary>
        /// Fecha de fin del viaje.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Precio del viaje.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Imagen representativa del viaje (opcional).
        /// </summary>
        public string? Image { get; set; }
    }

}
