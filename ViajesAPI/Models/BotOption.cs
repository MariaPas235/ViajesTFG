using System.Text.Json.Serialization;

namespace ViajesAPI.Models
{
    /// <summary>
    /// Representa una opción que puede mostrarse como botón en un nodo del bot.
    /// Cada opción tiene una etiqueta visible y la clave del siguiente nodo al que lleva.
    /// </summary>
    public class BotOption
    {
        /// <summary>
        /// Etiqueta visible de la opción que se muestra al usuario.
        /// Se serializa/deserializa con la propiedad JSON "label".
        /// </summary>
        [JsonPropertyName("label")]
        public string Label { get; set; }

        /// <summary>
        /// Clave del siguiente nodo al que se dirige el flujo si se selecciona esta opción.
        /// Se serializa/deserializa con la propiedad JSON "next".
        /// </summary>
        [JsonPropertyName("next")]
        public string Next { get; set; }
    }
}
