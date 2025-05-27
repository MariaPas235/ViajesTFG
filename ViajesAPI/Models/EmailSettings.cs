namespace ViajesAPI.Models
{
    /// <summary>
    /// Configuraci�n necesaria para el env�o de correos electr�nicos mediante SMTP.
    /// </summary>
    public class EmailSettings
    {
        /// <summary>
        /// Direcci�n del servidor SMTP (por ejemplo, smtp.gmail.com).
        /// </summary>
        public string SmtpServer { get; set; } = string.Empty;

        /// <summary>
        /// Puerto utilizado para la conexi�n SMTP (por ejemplo, 587).
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Nombre que aparecer� como remitente en los correos enviados.
        /// </summary>
        public string SenderName { get; set; } = string.Empty;

        /// <summary>
        /// Direcci�n de correo electr�nico que enviar� los mensajes.
        /// </summary>
        public string SenderEmail { get; set; } = string.Empty;

        /// <summary>
        /// Contrase�a o clave para autenticarse en el servidor SMTP.
        /// </summary>
        public string SenderPassword { get; set; } = string.Empty;
    }
}
