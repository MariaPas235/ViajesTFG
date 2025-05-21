namespace ViajesAPI.Models
{
    public class BotFlow
    {
        public int Id { get; set; }
        public string NodeKey { get; set; }         // Clave del nodo (ej: "START", "SALDO")
        public string Message { get; set; }         // Mensaje que muestra el bot
        public string? OptionsJson { get; set; }    // Opciones en formato JSON (botones)
        public string? NextNodeKey { get; set; }    // Siguiente nodo si no hay opciones
    }
}
