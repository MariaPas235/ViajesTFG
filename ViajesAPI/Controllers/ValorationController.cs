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

       [HttpPost]
public async Task<IActionResult> PostValoration([FromBody] Valoration dto)
{
    if (!ModelState.IsValid)
        return BadRequest(ModelState);

    var travel = await _context.travels.FindAsync(dto.TravelId);
    var user = await _context.users.FindAsync(dto.UserId);

    if (travel == null || user == null)
        return BadRequest("Usuario o viaje no válido.");

    var valoration = new Valoration
    {
        Punctuation = dto.Punctuation,
        Comment = dto.Comment,
        UserId = dto.UserId,
        TravelId = dto.TravelId
    };

    _context.valorations.Add(valoration);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetValoration), new { id = valoration.Id }, valoration);
}


        // GET: api/Valoration/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Valoration>> GetValoration(int id)
        {
            var valoration = await _context.valorations
                .Include(v => v.UserId)
                .Include(v => v.TravelId)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (valoration == null)
                return NotFound();

            return valoration;
        }
    }
}
