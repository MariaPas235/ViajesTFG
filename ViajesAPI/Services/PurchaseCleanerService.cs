using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using ViajesAPI.Data;
using ViajesAPI.Models;
using MailKit;
using MimeKit; // Para MailboxAddress.TryParse

namespace ViajesAPI.Services
{
    // Servicio para limpiar compras pendientes que llevan m�s de 5 minutos sin confirmarse
    public class PurchaseCleanerService
    {
        private readonly AppDbContext _context;
        private readonly EmailService _emailService;

        // Constructor con inyecci�n de dependencias para DB y servicio de email
        public PurchaseCleanerService(AppDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        /// <summary>
        /// Cambia a estado "cancelled" las compras pendientes con m�s de 5 minutos de antig�edad,
        /// aumenta la cantidad disponible del viaje y notifica al usuario por email.
        /// </summary>
        public async Task CleanPendingPurchasesAsync()
        {
            // Define el l�mite temporal para las compras consideradas "pendientes antiguas"
            var cutoff = DateTime.Now.AddMinutes(-5);

            // Obtiene todas las compras pendientes anteriores a cutoff, incluyendo info del usuario
            var pendingPurchases = await _context.purchases
                .Include(p => p.User) // Incluye la entidad User para acceso a email y nombre
                .Where(p => p.State == "pending" && p.PurchaseDate <= cutoff)
                .ToListAsync();

            foreach (var purchase in pendingPurchases)
            {
                // Cambia el estado a cancelado
                purchase.State = "cancelled";

                // Aumenta la cantidad disponible del viaje asociado, si existe
                var travel = await _context.travels.FindAsync(purchase.TravelId);
                if (travel != null)
                {
                    travel.Cantidad += 1;
                }

                // Env�a correo de notificaci�n solo si el email es v�lido
                var userEmail = purchase.User?.Email;
                if (!string.IsNullOrWhiteSpace(userEmail) &&
                    MailboxAddress.TryParse(userEmail, out var _))
                {
                    var subject = "Compra cancelada por inactividad";
                    var body = $"Hola {purchase.User.Name},\n\nTu compra con ID {purchase.Id} ha sido cancelada por inactividad despu�s de 5 minutos.";

                    await _emailService.SendEmailAsync(
                        userEmail,
                        purchase.User.Name ?? "Usuario",
                        subject,
                        $"<p>Hola {purchase.User.Name},</p><p>Tu compra con ID {purchase.Id} ha sido cancelada por inactividad despu�s de 5 minutos.</p>"
                    );
                }
                else
                {
                    // Log si el email es inv�lido o est� vac�o
                    Console.WriteLine($"[ERROR] Email inv�lido: '{userEmail}' para compra ID {purchase.Id}");
                }
            }

            // Guarda todos los cambios en la base de datos
            await _context.SaveChangesAsync();
        }
    }
}
