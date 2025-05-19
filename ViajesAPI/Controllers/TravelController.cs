using Microsoft.AspNetCore.Mvc;
using ViajesAPI.Data;
using ViajesAPI.Models;
using ViajesAPI.Models.DTO;

namespace ViajesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TravelController : Controller
    {
        private readonly AppDbContext _context;
        private ResponseDTO _response;

        public TravelController(AppDbContext context)
        {
            _context = context;
            _response = new ResponseDTO();
        }



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

    }
}
