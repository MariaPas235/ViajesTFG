using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ViajesAPI.Models
{
    public class Valoration
    {
        /// <summary>
        /// Identificador único de la valoración.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Puntuación otorgada (obligatoria).
        /// Puede ser un valor numérico que representa la valoración (ejemplo: de 1 a 5).
        /// </summary>
        [Required]
        public int Punctuation { get; set; }

        /// <summary>
        /// Comentario asociado a la valoración (obligatorio).
        /// Puede ser una opinión o feedback del usuario.
        /// </summary>
        [Required]
        public string Comment { get; set; }

        /// <summary>
        /// Id del usuario que hizo la valoración.
        /// Clave foránea para relacionar la valoración con el usuario.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Id del viaje al que pertenece la valoración.
        /// Clave foránea para relacionar la valoración con el viaje.
        /// </summary>
        public int TravelId { get; set; }
    }
}
