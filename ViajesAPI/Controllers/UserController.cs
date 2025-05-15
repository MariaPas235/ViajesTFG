using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using ViajesAPI.Data;
using ViajesAPI.Models;
using ViajesAPI.Models.DTO;

namespace ViajesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : Controller
    {
        private readonly AppDbContext _context;
        private ResponseDTO _response;

        public UserController(AppDbContext context)
        {
            _context = context;
            _response = new ResponseDTO();
        }
        [HttpPost("PostUser")]
        public ResponseDTO PostUser([FromBody] User user)
        {
            try
            {
                // Verificar si ya existe un usuario con el mismo email
                var existingUser = _context.users.FirstOrDefault(u => u.Email == user.Email);
                if (existingUser != null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Ya existe un usuario registrado con ese email.";
                    return _response;
                }

                _context.users.Add(user);
                _context.SaveChanges();
                _response.Data = user;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }


        [HttpPut("PutUser")]
        public ResponseDTO PutUser([FromBody] User user)
        {
            try
            {
                var existingUser = _context.users.FirstOrDefault(u => u.Id == user.Id);
                if (existingUser == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Usuario no encontrado.";
                    return _response;
                }

                var emailExists = _context.users
                    .Any(u => u.Email == user.Email && u.Id != user.Id);

                if (emailExists)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Ya existe un usuario con ese correo electrónico.";
                    return _response;
                }

                existingUser.Name = user.Name;
                existingUser.Email = user.Email;
                existingUser.Password = user.Password;

                _context.SaveChanges();
                _response.Data = existingUser;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }




        [HttpGet("GetUserByEmail/{email}")]
        public ResponseDTO GetUserByEmail(string email)
        {
            try
            {
                var user = _context.users.FirstOrDefault(x => x.Email == email);
                _response.Data = user;
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