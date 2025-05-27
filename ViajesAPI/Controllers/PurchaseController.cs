using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;
using ViajesAPI.Data;
using ViajesAPI.Models;
using ViajesAPI.Models.DTO;
using ViajesAPI.Services;
using QuestPDF.Fluent;

namespace ViajesAPI.Controllers
{
    /// <summary>
    /// Controlador para gestionar las compras de viajes, incluyendo generación de facturas,
    /// envíos de correo y solicitudes de reembolso.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PurchaseController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ResponseDTO _response;
        private readonly EmailService _emailService;
        private readonly PurchaseCleanerService _purchaseCleanerService;

        /// <summary>
        /// Constructor del controlador.
        /// </summary>
        public PurchaseController(AppDbContext context, EmailService emailService, PurchaseCleanerService purchaseCleanerService)
        {
            _context = context;
            _response = new ResponseDTO();
            _emailService = emailService;
            _purchaseCleanerService = purchaseCleanerService;
        }

        /// <summary>
        /// Registra una nueva compra y envía un email con factura en PDF al cliente.
        /// </summary>
        [HttpPost("PostPurchase")]
        public async Task<IActionResult> PostPurchase([FromBody] Purchase dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Verifica si ya existe una compra con el mismo ID de operación
            if (_context.purchases.Any(p => p.id_operatio == dto.id_operatio))
                return BadRequest("Ya existe una compra con ese id_operatio.");

            var user = await _context.users.FindAsync(dto.UserId);
            var travel = await _context.travels.FindAsync(dto.TravelId);

            if (user == null || travel == null)
                return BadRequest("Usuario o viaje no válido.");

            // Crear objeto de compra
            var purchase = new Purchase
            {
                PurchaseDate = dto.PurchaseDate,
                State = "pending",
                id_operatio = dto.id_operatio,
                data = dto.data,
                order = dto.order,
                UserId = dto.UserId,
                TravelId = dto.TravelId,
                Destino = dto.Destino,
                InitDate = dto.InitDate,
                EndDate = dto.EndDate,
                Price = dto.Price,
                Image = dto.Image
            };

            _context.purchases.Add(purchase);
            await _context.SaveChangesAsync();

            // Generar el PDF de la factura
            var pdfBytes = GenerateInvoicePdfBytes(purchase);

            // Preparar y enviar correo al usuario
            var userEmail = user.Email;
            var userName = user.Name ?? "Cliente";
            var subject = "Confirmación de compra - Viajes TFG";

            var estadoTexto = purchase.State switch
            {
                "confirmed" => "✅ Confirmada",
                "cancelled" => "❌ Cancelada",
                "refunded" => "💸 Reembolsada",
                _ => "⏳ Pendiente"
            };

            // Contenido HTML del correo
            var body = $@"<div style='font-family: Arial; color: #333; max-width: 600px; margin: auto; border: 1px solid #ddd; padding: 20px; border-radius: 10px;'>
                <h2 style='color: #2a8ee0;'>🎉 ¡Gracias por tu compra, {userName}!</h2>
                <p>Tu reserva ha sido <strong>{estadoTexto}</strong> con el número de operación <strong>{purchase.id_operatio}</strong>.</p>
                <h3>✈️ Detalles del viaje</h3>
                <table style='width: 100%; border-collapse: collapse;'>
                    <tr><td><strong>Destino:</strong></td><td>{purchase.Destino}</td></tr>
                    <tr><td><strong>Fecha de inicio:</strong></td><td>{purchase.InitDate:dd/MM/yyyy}</td></tr>
                    <tr><td><strong>Fecha de fin:</strong></td><td>{purchase.EndDate:dd/MM/yyyy}</td></tr>
                    <tr><td><strong>Fecha de compra:</strong></td><td>{purchase.PurchaseDate:dd/MM/yyyy}</td></tr>
                    <tr><td><strong>Estado:</strong></td><td>{estadoTexto}</td></tr>
                    <tr><td><strong>Order ID:</strong></td><td>{purchase.order}</td></tr>
                    <tr><td><strong>Precio:</strong></td><td>${purchase.Price}</td></tr>
                </table>
                <p>🧳 ¡Te deseamos un viaje increíble!</p>
                <hr/><p style='text-align: center;'>Viajes TFG © {DateTime.Now.Year}</p></div>";

            await _emailService.SendEmailAsyncWithAttachment(userEmail, userName, subject, body, pdfBytes, $"Factura_{purchase.id_operatio}.pdf");

            return CreatedAtAction(nameof(PostPurchase), new { id = purchase.Id }, purchase);
        }

        /// <summary>
        /// Devuelve todas las compras registradas.
        /// </summary>
        [HttpGet("GetAllPurchases")]
        public async Task<IActionResult> GetAllPurchases()
        {
            try
            {
                var purchases = await _context.purchases
                    .Include(p => p.User)
                    .Include(p => p.Travel)
                    .ToListAsync();

                return Ok(purchases);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener las compras: {ex.Message}");
            }
        }

        /// <summary>
        /// Genera el PDF con los detalles de la factura.
        /// </summary>
        private byte[] GenerateInvoicePdfBytes(Purchase purchase)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(40);
                    page.Size(PageSizes.A4);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header().Text("Factura de Compra - Viajes TFG").SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                    page.Content().PaddingVertical(10).Column(col =>
                    {
                        col.Item().Text($"Fecha de compra: {purchase.PurchaseDate:dd/MM/yyyy}");
                        col.Item().Text($"ID de operación: {purchase.id_operatio}");
                        col.Item().Text($"Estado: {purchase.State}");
                        col.Item().Text($"Destino: {purchase.Destino}");
                        col.Item().Text($"Fecha inicio: {purchase.InitDate:dd/MM/yyyy}");
                        col.Item().Text($"Fecha fin: {purchase.EndDate:dd/MM/yyyy}");
                        col.Item().Text($"Precio: ${purchase.Price}");
                        col.Item().Text($"Order ID: {purchase.order}");
                    });

                    page.Footer().AlignCenter().Text(txt =>
                    {
                        txt.Span("Gracias por confiar en Viajes TFG.").FontSize(10);
                    });
                });
            });

            return document.GeneratePdf();
        }

        /// <summary>
        /// Obtiene todas las compras de un usuario específico.
        /// </summary>
        [HttpGet("GetPurchasesByUser/{userId}")]
        public async Task<IActionResult> GetPurchasesByUser(int userId)
        {
            try
            {
                var purchases = await _context.purchases
                    .Where(p => p.UserId == userId)
                    .Include(p => p.Travel)
                    .Include(p => p.User)
                    .ToListAsync();

                if (!purchases.Any())
                    return NotFound($"No se encontraron compras para el usuario con Id {userId}.");

                return Ok(purchases);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener las compras por usuario: {ex.Message}");
            }
        }

        /// <summary>
        /// Solicita un reembolso para una compra.
        /// </summary>
        [HttpPost("RequestRefund/{orderId}")]
        public async Task<IActionResult> RequestRefund(string orderId)
        {
            var purchase = await _context.purchases.FirstOrDefaultAsync(p => p.order == orderId);

            if (purchase == null)
                return NotFound("Purchase not found.");

            if (purchase.RefundStatus == "requested" || purchase.RefundStatus == "confirmed")
                return BadRequest("Refund already requested or confirmed.");

            purchase.RefundStatus = "requested";
            await _context.SaveChangesAsync();

            return Ok("Refund request submitted.");
        }

        /// <summary>
        /// Devuelve todas las solicitudes de reembolso pendientes.
        /// </summary>
        [HttpGet("GetRefundRequests")]
        public async Task<IActionResult> GetRefundRequests()
        {
            var requests = await _context.purchases
                .Where(p => p.RefundStatus == "requested")
                .Include(p => p.User)
                .Include(p => p.Travel)
                .ToListAsync();

            return Ok(requests);
        }

        /// <summary>
        /// Acepta una solicitud de reembolso.
        /// </summary>
        [HttpPost("AcceptRefund/{orderId}")]
        public async Task<IActionResult> AcceptRefund(string orderId)
        {
            var purchase = await _context.purchases.FirstOrDefaultAsync(p => p.order == orderId);

            if (purchase == null)
                return NotFound("Purchase not found.");

            if (purchase.RefundStatus != "requested")
                return BadRequest("Refund must be requested first.");

            purchase.RefundStatus = "accepted";
            await _context.SaveChangesAsync();

            return Ok("Refund accepted.");
        }

        /// <summary>
        /// Completa un reembolso previamente aceptado.
        /// </summary>
        [HttpPost("CompleteRefund/{orderId}")]
        public async Task<IActionResult> CompleteRefund(string orderId)
        {
            var purchase = await _context.purchases.FirstOrDefaultAsync(p => p.order == orderId);

            if (purchase == null)
                return NotFound("Purchase not found.");

            if (purchase.RefundStatus != "accepted")
                return BadRequest("Refund must be accepted before completing.");

            purchase.RefundStatus = "completed";

            // ✅ Sumar uno a la cantidad del viaje asociado
            var travel = await _context.travels.FindAsync(purchase.TravelId);
            if (travel != null)
            {
                travel.Cantidad += 1;
            }

            await _context.SaveChangesAsync();

            return Ok("Refund marked as completed and travel quantity updated.");
        }


        /// <summary>
        /// Limpia las compras pendientes antiguas y envía correos de cancelación.
        /// </summary>

        [HttpPost("CleanPendingPurchases")]
        public async Task<IActionResult> CleanPendingPurchases()
        {
            await _purchaseCleanerService.CleanPendingPurchasesAsync();
            return Ok("Pendientes antiguos cancelados y correos enviados.");
        }

        /// <summary>
        /// Confirma una compra marcándola como "confirmed".
        /// </summary>
        [HttpPost("ConfirmPurchase/{id}")]
        public async Task<IActionResult> ConfirmPurchase(int id)
        {
            var purchase = await _context.purchases.FindAsync(id);
            if (purchase == null) return NotFound();

            purchase.State = "confirmed";
            await _context.SaveChangesAsync();

            return Ok(purchase);
        }
    }
}
