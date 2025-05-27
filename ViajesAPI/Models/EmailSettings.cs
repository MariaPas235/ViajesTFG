namespace ViajesAPI.Models
{
    /// <summary>
    /// Configuración necesaria para el envío de correos electrónicos mediante SMTP.
    /// </summary>
    public class EmailSettings
    {
        /// <summary>
        /// Dirección del servidor SMTP (por ejemplo, smtp.gmail.com).
        /// </summary>
        public string SmtpServer { get; set; } = string.Empty;

        /// <summary>
        /// Puerto utilizado para la conexión SMTP (por ejemplo, 587).
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Nombre que aparecerá como remitente en los correos enviados.
        /// </summary>
        public string SenderName { get; set; } = string.Empty;

        /// <summary>
        /// Dirección de correo electrónico que enviará los mensajes.
        /// </summary>
        public string SenderEmail { get; set; } = string.Empty;

        /// <summary>
        /// Contraseña o clave para autenticarse en el servidor SMTP.
        /// </summary>
        public string SenderPassword { get; set; } = string.Empty;
    }
}
