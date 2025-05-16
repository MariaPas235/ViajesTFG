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
                TravelId = dto.TravelId
            };

            _context.purchases.Add(purchase);
            await _context.SaveChangesAsync();

            // Enviar email de confirmación
            var userEmail = user.Email;
            var userName = user.Name ?? "Cliente";

            var subject = "Confirmación de compra - Viajes TFG";
            var body = $@"
                <h2>Gracias por tu compra</h2>
                <p>Hola {userName},</p>
                <p>Tu compra con número de operación <strong>{purchase.id_operatio}</strong> ha sido registrada correctamente.</p>
                <p><strong>Detalles del viaje:</strong></p>
                <ul>
                  <li>Orden: {purchase.order}</li>
                  <li>Fecha: {purchase.PurchaseDate}</li>
                  <li>Estado: {(purchase.State ? "Confirmada" : "Pendiente")}</li>
                  <li>Datos: {purchase.data}</li>
                </ul>
                <p>Gracias por confiar en nosotros.</p>
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
