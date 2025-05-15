using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using ViajesAPI.Data;
using ViajesAPI.Models;
using ViajesAPI.Models.DTO;

namespace ViajesAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PurchaseController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ResponseDTO _response;

        public PurchaseController(AppDbContext context)
        {
            _context = context;
            _response = new ResponseDTO();
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
    }
    }
