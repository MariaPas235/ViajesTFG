using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using ViajesAPI.Data;
using ViajesAPI.Models;
using ViajesAPI.Models.DTO;
using ViajesAPI.Services;  // <-- Importar EmailService

namespace ViajesAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PurchaseController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ResponseDTO _response;
        private readonly EmailService _emailService;  // <-- Campo para EmailService

        // Modificamos constructor para inyectar EmailService
        public PurchaseController(AppDbContext context, EmailService emailService)
        {
            _context = context;
            _response = new ResponseDTO();
            _emailService = emailService;
        }

        [HttpPost("PostPurchase")]
        public async Task<IActionResult> PostPurchase([FromBody] Purchase dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool exists = _context.purchases.Any(p => p.id_operatio == dto.id_operatio);
            if (exists)
                return BadRequest("Ya existe una compra con ese id_operatio.");

            var user = await _context.users.FindAsync(dto.UserId);
            var travel = await _context.travels.FindAsync(dto.TravelId);

            if (user == null || travel == null)
                return BadRequest("Usuario o viaje no válido.");

            var purchase = new Purchase
            {
                PurchaseDate = dto.PurchaseDate,
                State = dto.State,
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

            // Enviar email de confirmación
            var userEmail = user.Email;
            var userName = user.Name ?? "Cliente";

            var subject = "Confirmación de compra - Viajes TFG";
            var body = $@"
    <div style='font-family: Arial, sans-serif; color: #333; max-width: 600px; margin: auto; border: 1px solid #ddd; padding: 20px; border-radius: 10px;'>
        <h2 style='color: #2a8ee0;'>🎉 ¡Gracias por tu compra, {userName}!</h2>

        <p style='font-size: 16px;'>Tu reserva ha sido <strong>{(purchase.State ? "confirmada" : "registrada y pendiente")}</strong> con el número de operación <strong>{purchase.id_operatio}</strong>.</p>

        <h3 style='color: #2a8ee0;'>✈️ Detalles del viaje</h3>
        <table style='width: 100%; border-collapse: collapse; font-size: 15px;'>
            <tr>
                <td style='padding: 8px; font-weight: bold;'>Destino:</td>
                <td style='padding: 8px;'>{purchase.Destino}</td>
            </tr>
            <tr style='background-color: #f9f9f9;'>
                <td style='padding: 8px; font-weight: bold;'>Fecha de inicio:</td>
                <td style='padding: 8px;'>{purchase.InitDate:dd/MM/yyyy}</td>
            </tr>
            <tr>
                <td style='padding: 8px; font-weight: bold;'>Fecha de fin:</td>
                <td style='padding: 8px;'>{purchase.EndDate:dd/MM/yyyy}</td>
            </tr>
            <tr style='background-color: #f9f9f9;'>
                <td style='padding: 8px; font-weight: bold;'>Fecha de compra:</td>
                <td style='padding: 8px;'>{purchase.PurchaseDate:dd/MM/yyyy}</td>
            </tr>
            <tr>
                <td style='padding: 8px; font-weight: bold;'>Estado:</td>
                <td style='padding: 8px;'>{(purchase.State ? "✅ Confirmada" : "⏳ Pendiente")}</td>
            </tr>
            <tr style='background-color: #f9f9f9;'>
                <td style='padding: 8px; font-weight: bold;'>Order ID:</td>
                <td style='padding: 8px;'>{purchase.order}</td>
            </tr>
            <tr>
                <td style='padding: 8px; font-weight: bold;'>Precio:</td>
                <td style='padding: 8px;'>${purchase.Price}</td>
            </tr>
        </table>

        <p style='margin-top: 20px;'>🧳 ¡Te deseamos un viaje increíble!</p>
        <p style='font-size: 14px; color: #777;'>Este correo es una confirmación automática. No respondas a esta dirección.</p>

        <hr style='margin: 30px 0;' />
        <p style='text-align: center; font-size: 13px; color: #aaa;'>Viajes TFG © {(DateTime.Now.Year)}</p>
    </div>
";


            await _emailService.SendEmailAsync(userEmail, userName, subject, body);

            return CreatedAtAction(nameof(PostPurchase), new { id = purchase.Id }, purchase);
        }

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
    }
}
