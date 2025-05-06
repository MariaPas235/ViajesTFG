using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ViajesAPI.Data;
using ViajesAPI.Models;

namespace ViajesAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValorationController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ValorationController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/Valoration
        [HttpPost]
        public async Task<IActionResult> PostValoration([FromBody] Valoration valoration)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Optional: Ensure Travel and User exist
            var travelExists = await _context.travels.AnyAsync(t => t.Id == valoration.Travel.Id);
            var userExists = await _context.users.AnyAsync(u => u.Id == valoration.User.Id);

            if (!travelExists || !userExists)
                return BadRequest("Usuario o viaje no válido.");

            // Attach User and Travel if not being tracked
            _context.Attach(valoration.User);
            _context.Attach(valoration.Travel);

            _context.valorations.Add(valoration);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetValoration), new { id = valoration.Id }, valoration);
        }

        // GET: api/Valoration/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Valoration>> GetValoration(int id)
        {
            var valoration = await _context.valorations
                .Include(v => v.User)
                .Include(v => v.Travel)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (valoration == null)
                return NotFound();

            return valoration;
        }
    }
}
