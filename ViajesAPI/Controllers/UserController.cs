using Microsoft.AspNetCore.Mvc;
using ViajesAPI.Data;
using ViajesAPI.Models;
using ViajesAPI.Models.DTO;
using BCrypt.Net; // Necesario para el hash

namespace ViajesAPI.Controllers
{
    /// <summary>
    /// Controlador para gestionar usuarios, incluyendo registro, autenticación y actualización de datos.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly AppDbContext _context;
        private ResponseDTO _response;

        /// <summary>
        /// Constructor del controlador UserController.
        /// </summary>
        /// <param name="context">Contexto de base de datos inyectado.</param>
        public UserController(AppDbContext context)
        {
            _context = context;
            _response = new ResponseDTO();
        }

        /// <summary>
        /// Registra un nuevo usuario.
        /// </summary>
        /// <param name="user">Objeto User recibido en el cuerpo de la solicitud.</param>
        /// <returns>Respuesta con el usuario registrado o mensaje de error.</returns>
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

                // Forzar rol predeterminado
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

        /// <summary>
        /// Actualiza los datos de un usuario existente.
        /// </summary>
        /// <param name="user">Objeto User con los nuevos datos.</param>
        /// <returns>Respuesta con el usuario actualizado o mensaje de error.</returns>
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

                // Actualizar los campos permitidos
                existingUser.Name = user.Name;
                existingUser.Email = user.Email;

                // Rehashear la contraseña si fue enviada
                if (!string.IsNullOrWhiteSpace(user.Password))
                {
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

        /// <summary>
        /// Obtiene un usuario por su correo electrónico.
        /// </summary>
        /// <param name="email">Correo electrónico del usuario.</param>
        /// <returns>Respuesta con los datos del usuario o mensaje de error.</returns>
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

        /// <summary>
        /// Obtiene todos los usuarios registrados.
        /// </summary>
        /// <returns>Respuesta con la lista de usuarios o mensaje de error.</returns>
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

        /// <summary>
        /// Obtiene un usuario por su ID.
        /// </summary>
        /// <param name="id">ID del usuario.</param>
        /// <returns>Respuesta con los datos del usuario o mensaje de error.</returns>
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

        /// <summary>
        /// Verifica las credenciales del usuario para iniciar sesión.
        /// </summary>
        /// <param name="login">Objeto LoginDTO con email y contraseña.</param>
        /// <returns>Respuesta con el usuario autenticado o mensaje de error.</returns>
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

                // Elimina la contraseña antes de devolver el objeto
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
