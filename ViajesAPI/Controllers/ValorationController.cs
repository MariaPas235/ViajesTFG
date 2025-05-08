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

        [HttpGet("GetValorationByTravelId/{travelId}")]
        public async Task<IActionResult> GetValorationsByTravelId(int travelId)
        {
            var valorations = await _context.valorations
                .Where(v => v.TravelId == travelId)
                .ToListAsync();

            if (valorations == null || !valorations.Any())
            {
                return NotFound();
            }

            var userIds = valorations.Select(v => v.UserId).Distinct().ToList();

            var users = await _context.users
                .Where(u => userIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id);

            var valorationsWithUserNames = valorations.Select(valoration =>
            {
                users.TryGetValue(valoration.UserId, out var user);

                return new
                {
                    valoration.Id,
                    valoration.Punctuation,
                    valoration.Comment,
                    valoration.UserId,
                    valoration.TravelId,
                    UserName = user?.Name ?? "Usuario desconocido"
                };
            });

            return Ok(valorationsWithUserNames);
        }

    }
}
