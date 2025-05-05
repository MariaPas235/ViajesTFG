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

    }

}
