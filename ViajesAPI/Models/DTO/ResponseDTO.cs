namespace ViajesAPI.Models.DTO
{
    /// <summary>
    /// DTO para estandarizar las respuestas de la API.
    /// Contiene datos, estado de éxito y mensaje informativo.
    /// </summary>
    public class ResponseDTO
    {
        /// <summary>
        /// Datos de la respuesta, puede ser cualquier objeto.
        /// </summary>
        public Object? Data { get; set; }

        /// <summary>
        /// Indica si la operación fue exitosa o no. Por defecto es true.
        /// </summary>
        public bool IsSuccess { get; set; } = true;

        /// <summary>
        /// Mensaje informativo sobre el resultado de la operación.
        /// </summary>
        public String Message { get; set; } = "";
    }
}
