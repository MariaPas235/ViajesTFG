using Microsoft.AspNetCore.Mvc;
using ViajesAPI.Data;
using ViajesAPI.Models;
using ViajesAPI.Models.DTO;

namespace ViajesAPI.Controllers
{
    /// <summary>
    /// Controlador para gestionar operaciones CRUD relacionadas con viajes.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TravelController : Controller
    {
        private readonly AppDbContext _context;
        private ResponseDTO _response;

        /// <summary>
        /// Constructor del controlador TravelController.
        /// </summary>
        /// <param name="context">Contexto de base de datos inyectado.</param>
        public TravelController(AppDbContext context)
        {
            _context = context;
            _response = new ResponseDTO();
        }

        /// <summary>
        /// Crea un nuevo viaje.
        /// </summary>
        /// <param name="travel">Objeto Travel recibido en el cuerpo de la solicitud.</param>
        /// <returns>Respuesta con el objeto creado o error.</returns>
        [HttpPost("PostTravel")]
        public ResponseDTO PostUser([FromBody] Travel travel)
        {
            try
            {
                _context.travels.Add(travel);
                _context.SaveChanges();
                _response.Data = travel;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        /// <summary>
        /// Obtiene la lista de todos los viajes.
        /// </summary>
        /// <returns>Respuesta con la lista de viajes o error.</returns>
        [HttpGet("GetTravels")]
        public ResponseDTO GetTravels()
        {
            try
            {
                var travels = _context.travels.ToList();
                _response.Data = travels;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        /// <summary>
        /// Obtiene un viaje por su ID.
        /// </summary>
        /// <param name="id">ID del viaje a buscar.</param>
        /// <returns>Respuesta con el viaje encontrado o error.</returns>
        [HttpGet("GetTravelById/{id}")]
        public ResponseDTO GetTravelByID(int id)
        {
            try
            {
                var travel = _context.travels.FirstOrDefault(x => x.Id == id);
                _response.Data = travel;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        /// <summary>
        /// Actualiza los datos de un viaje existente.
        /// </summary>
        /// <param name="id">ID del viaje a actualizar.</param>
        /// <param name="updatedTravel">Objeto Travel con los nuevos datos.</param>
        /// <returns>Respuesta con el viaje actualizado o error.</returns>
        [HttpPut("PutTravel/{id}")]
        public ResponseDTO PutTravel(int id, [FromBody] Travel updatedTravel)
        {
            try
            {
                var existingTravel = _context.travels.FirstOrDefault(x => x.Id == id);
                if (existingTravel == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Viaje no encontrado";
                    return _response;
                }

                // Actualizar propiedades
                existingTravel.Destination = updatedTravel.Destination;
                existingTravel.Description = updatedTravel.Description;
                existingTravel.InitDate = updatedTravel.InitDate;
                existingTravel.EndDate = updatedTravel.EndDate;
                existingTravel.Price = updatedTravel.Price;
                existingTravel.Image = updatedTravel.Image;
                existingTravel.Latitud = updatedTravel.Latitud;
                existingTravel.Longitud = updatedTravel.Longitud;
                existingTravel.Cantidad = updatedTravel.Cantidad;

                _context.SaveChanges();

                _response.Data = existingTravel;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        /// <summary>
        /// Elimina un viaje por su ID.
        /// </summary>
        /// <param name="id">ID del viaje a eliminar.</param>
        /// <returns>Respuesta indicando éxito o error.</returns>
        [HttpDelete("DeleteTravel/{id}")]
        public ResponseDTO DeleteTravel(int id)
        {
            try
            {
                var travel = _context.travels.FirstOrDefault(x => x.Id == id);
                if (travel == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Viaje no encontrado";
                    return _response;
                }

                _context.travels.Remove(travel);
                _context.SaveChanges();

                _response.Message = "Viaje eliminado correctamente";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        /// <summary>
        /// Disminuye en uno la cantidad disponible de un viaje.
        /// </summary>
        /// <param name="id">ID del viaje.</param>
        /// <returns>Respuesta HTTP con el viaje actualizado o error.</returns>
        [HttpPost("DecreaseQuantity/{id}")]
        public async Task<IActionResult> DecreaseQuantity(int id)
        {
            var travel = await _context.travels.FindAsync(id);
            if (travel == null)
                return NotFound("Viaje no encontrado");

            if (travel.Cantidad > 0)
            {
                travel.Cantidad -= 1;
                await _context.SaveChangesAsync();
            }

            return Ok(travel);
        }
    }
}
