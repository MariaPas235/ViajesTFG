using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ViajesAPI.Data;
using ViajesAPI.Models;

namespace ViajesAPI.Controllers
{
    /// <summary>
    /// Controlador para gestionar valoraciones de los viajes.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ValorationController : ControllerBase
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Constructor del ValorationController.
        /// </summary>
        /// <param name="context">Contexto de base de datos.</param>
        public ValorationController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Crea una nueva valoración.
        /// </summary>
        /// <param name="dto">Objeto Valoration recibido en el cuerpo.</param>
        /// <returns>Resultado de la acción HTTP.</returns>
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

        /// <summary>
        /// Obtiene una valoración por su ID.
        /// </summary>
        /// <param name="id">ID de la valoración.</param>
        /// <returns>Valoración encontrada o mensaje de error.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Valoration>> GetValoration(int id)
        {
            var valoration = await _context.valorations
                .FirstOrDefaultAsync(v => v.Id == id);

            if (valoration == null)
                return NotFound();

            return valoration;
        }

        /// <summary>
        /// Obtiene todas las valoraciones de un viaje específico.
        /// </summary>
        /// <param name="travelId">ID del viaje.</param>
        /// <returns>Lista de valoraciones con nombres de usuario.</returns>
        [HttpGet("GetValorationByTravelId/{travelId}")]
        public async Task<IActionResult> GetValorationsByTravelId(int travelId)
        {
            var valorations = await _context.valorations
                .Where(v => v.TravelId == travelId)
                .ToListAsync();

            if (valorations == null || !valorations.Any())
                return NotFound();

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

        /// <summary>
        /// Actualiza una valoración existente.
        /// </summary>
        /// <param name="id">ID de la valoración a actualizar.</param>
        /// <param name="updated">Objeto Valoration con los nuevos datos.</param>
        /// <returns>Valoración actualizada o mensaje de error.</returns>
        [HttpPut("UpdateValoration/{id}")]
        public async Task<IActionResult> UpdateValoration(int id, [FromBody] Valoration updated)
        {
            var existingValoration = await _context.valorations.FindAsync(id);

            if (existingValoration == null)
                return NotFound("Valoración no encontrada.");

            if (existingValoration.UserId != updated.UserId)
                return Forbid("No tienes permiso para modificar esta valoración.");

            existingValoration.Punctuation = updated.Punctuation;
            existingValoration.Comment = updated.Comment;

            await _context.SaveChangesAsync();

            return Ok(existingValoration);
        }

        /// <summary>
        /// Elimina una valoración por su ID.
        /// </summary>
        /// <param name="id">ID de la valoración a eliminar.</param>
        /// <param name="userId">ID del usuario que intenta eliminar la valoración.</param>
        /// <returns>Mensaje de confirmación o error.</returns>
        [HttpDelete("DeleteValoration/{id}")]
        public async Task<IActionResult> DeleteValoration(int id, [FromQuery] int userId)
        {
            var valoration = await _context.valorations.FindAsync(id);

            if (valoration == null)
                return NotFound(new { message = "Valoración no encontrada." });

            if (valoration.UserId != userId)
                return Forbid("No tienes permiso para eliminar esta valoración.");

            _context.valorations.Remove(valoration);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Valoración eliminada correctamente." });
        }
    }
}
