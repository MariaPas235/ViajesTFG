using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ViajesAPI.Models
{
    public class User
    {
        /// <summary>
        /// Identificador único del usuario.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Correo electrónico del usuario (obligatorio).
        /// Se suele usar para el login y comunicación.
        /// </summary>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// Contraseña del usuario (obligatoria).
        /// Debe almacenarse hasheada para seguridad.
        /// </summary>
        [Required]

        public string Password { get; set; }

        /// <summary>
        /// Nombre del usuario (obligatorio).
        /// Puede ser nombre completo o alias.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Rol del usuario (ejemplo: "user", "admin").
        /// Permite controlar permisos y acceso.
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// Lista de compras asociadas a este usuario.
        /// Puede ser nula si no tiene compras.
        /// Relación uno a muchos con Purchase.
        /// </summary>
        public List<Purchase>? Purchases { get; set; }

        /// <summary>
        /// Lista de valoraciones hechas por el usuario.
        /// Puede ser nula si no ha valorado nada.
        /// Relación uno a muchos con Valoration.
        /// </summary>
        public List<Valoration>? Valorations { get; set; }
    }
}
