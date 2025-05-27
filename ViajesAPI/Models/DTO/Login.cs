namespace ViajesAPI.Models.DTO
{
    /// <summary>
    /// DTO para el proceso de login de usuarios.
    /// Contiene las credenciales necesarias para autenticación.
    /// </summary>
    public class LoginDTO
    {
        /// <summary>
        /// Correo electrónico del usuario que intenta iniciar sesión.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Contraseña del usuario para autenticación.
        /// </summary>
        public string Password { get; set; }
    }
}
