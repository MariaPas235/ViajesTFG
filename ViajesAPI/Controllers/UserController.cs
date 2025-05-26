using Microsoft.AspNetCore.Mvc;
using ViajesAPI.Data;
using ViajesAPI.Models;
using ViajesAPI.Models.DTO;
using BCrypt.Net; // Necesario para el hash

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
                var existingUser = _context.users.FirstOrDefault(u => u.Email == user.Email);
                if (existingUser != null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Ya existe un usuario registrado con ese email.";
                    return _response;
                }

                // Hashear la contraseña usando BCrypt
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

                // Forzar rol
                user.Role = "user";

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

                var emailExists = _context.users.Any(u => u.Email == user.Email && u.Id != user.Id);
                if (emailExists)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Ya existe un usuario con ese correo electrónico.";
                    return _response;
                }

                existingUser.Name = user.Name;
                existingUser.Email = user.Email;

                if (!string.IsNullOrWhiteSpace(user.Password))
                {
                    // Rehashear si se cambia la contraseña
                    existingUser.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                }

                existingUser.Role = user.Role;

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

        [HttpGet("GetAllUsers")]
        public ResponseDTO GetAllUsers()
        {
            try
            {
                var users = _context.users.ToList();
                _response.Data = users;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpGet("GetUserById/{id}")]
        public ResponseDTO GetUserById(int id)
        {
            try
            {
                var user = _context.users.FirstOrDefault(u => u.Id == id);
                _response.Data = user;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
    
    [HttpPost("LoginUser")]
        public ResponseDTO LoginUser([FromBody] LoginDTO login)
        {
            try
            {
                var user = _context.users.FirstOrDefault(u => u.Email == login.Email);
                if (user == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Usuario no encontrado.";
                    return _response;
                }

                // Verifica la contraseña hasheada
                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(login.Password, user.Password);
                if (!isPasswordValid)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Contraseña incorrecta.";
                    return _response;
                }

                // Por seguridad, elimina la contraseña antes de devolver el usuario
                user.Password = null;

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
