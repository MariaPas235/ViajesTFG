namespace ViajesAPI.Models
{
    /// <summary>
    /// Representa un nodo dentro del flujo de conversación del bot.
    /// Cada nodo tiene una clave única, un mensaje y puede contener opciones o un nodo siguiente.
    /// </summary>
    public class BotFlow
    {
        /// <summary>
        /// Identificador único del nodo en la base de datos.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Clave única del nodo, por ejemplo "START" o "SALDO".
        /// </summary>
        public string NodeKey { get; set; }

        /// <summary>
        /// Mensaje que el bot muestra al usuario en este nodo.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Opciones en formato JSON que representan botones u opciones para el usuario.
        /// Puede ser null si no hay opciones para este nodo.
        /// </summary>
        public string? OptionsJson { get; set; }

        /// <summary>
        /// Clave del siguiente nodo al que se dirige el flujo si no hay opciones.
        /// Puede ser null si no hay siguiente nodo definido.
        /// </summary>
        public string? NextNodeKey { get; set; }
    }
}
