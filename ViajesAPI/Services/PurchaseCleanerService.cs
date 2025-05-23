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
    public class PurchaseCleanerService
    {
        private readonly AppDbContext _context;
        private readonly EmailService _emailService;

        public PurchaseCleanerService(AppDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task CleanPendingPurchasesAsync()
        {
            var cutoff = DateTime.Now.AddMinutes(-5);

            var pendingPurchases = await _context.purchases
                .Include(p => p.User) // Asegúrate de incluir User, no UserId
                .Where(p => p.State == "pending" && p.PurchaseDate <= cutoff)
                .ToListAsync();

            foreach (var purchase in pendingPurchases)
            {
                purchase.State = "cancelled";

                var travel = await _context.travels.FindAsync(purchase.TravelId);
                if (travel != null)
                {
                    travel.Cantidad += 1;
                }

                // Enviar correo solo si el email es válido
                var userEmail = purchase.User?.Email;
                if (!string.IsNullOrWhiteSpace(userEmail) &&
                    MailboxAddress.TryParse(userEmail, out var _))
                {
                    var subject = "Compra cancelada por inactividad";
                    var body = $"Hola {purchase.User.Name},\n\nTu compra con ID {purchase.Id} ha sido cancelada por inactividad después de 5 minutos.";

                    await _emailService.SendEmailAsync(
                        userEmail,
                        purchase.User.Name ?? "Usuario",
                        subject,
                        $"<p>Hola {purchase.User.Name},</p><p>Tu compra con ID {purchase.Id} ha sido cancelada por inactividad después de 5 minutos.</p>"
                    );
                }
                else
                {
                    Console.WriteLine($"[ERROR] Email inválido: '{userEmail}' para compra ID {purchase.Id}");
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
