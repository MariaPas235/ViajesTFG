using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using ViajesAPI.Models;
using Microsoft.Extensions.Options;

namespace ViajesAPI.Services
{
    public class EmailService
    {
        private readonly EmailSettings _emailSettings;

        // Constructor que recibe la configuración de email inyectada
        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        /// <summary>
        /// Envía un correo electrónico simple con contenido HTML.
        /// </summary>
        public async Task SendEmailAsync(string toEmail, string toName, string subject, string htmlContent)
        {
            // Valida que el email destino sea válido
            if (string.IsNullOrWhiteSpace(toEmail) || !MailboxAddress.TryParse(toEmail.Trim(), out var parsedAddress))
                return;

            var message = new MimeMessage();
            // Configura remitente y destinatario
            message.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
            message.To.Add(new MailboxAddress(toName ?? "Usuario", parsedAddress.Address));
            message.Subject = subject;

            // Define el cuerpo en formato HTML
            message.Body = new TextPart("html") { Text = htmlContent };

            using var client = new SmtpClient();
            // Conecta de forma segura al servidor SMTP
            await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, SecureSocketOptions.StartTls);
            // Autentica con usuario y contraseña
            await client.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.SenderPassword);
            // Envía el mensaje
            await client.SendAsync(message);
            // Desconecta del servidor
            await client.DisconnectAsync(true);
        }

        /// <summary>
        /// Envía un correo electrónico con contenido HTML y un archivo adjunto PDF.
        /// </summary>
        public async Task SendEmailAsyncWithAttachment(
            string toEmail,
            string toName,
            string subject,
            string htmlContent,
            byte[] attachmentBytes,
            string attachmentFilename)
        {
            // Valida que el email destino sea válido
            if (string.IsNullOrWhiteSpace(toEmail) || !MailboxAddress.TryParse(toEmail.Trim(), out var parsedAddress))
                return;

            var message = new MimeMessage();
            // Configura remitente y destinatario
            message.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
            message.To.Add(new MailboxAddress(toName ?? "Usuario", parsedAddress.Address));
            message.Subject = subject;

            // Construye el cuerpo del correo con HTML y adjunto PDF
            var builder = new BodyBuilder { HtmlBody = htmlContent };
            builder.Attachments.Add(attachmentFilename, attachmentBytes, new ContentType("application", "pdf"));
            message.Body = builder.ToMessageBody();

            using var client = new SmtpClient();
            // Conecta de forma segura al servidor SMTP
            await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, SecureSocketOptions.StartTls);
            // Autentica con usuario y contraseña
            await client.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.SenderPassword);
            // Envía el mensaje con adjunto
            await client.SendAsync(message);
            // Desconecta del servidor
            await client.DisconnectAsync(true);
        }
    }
}
