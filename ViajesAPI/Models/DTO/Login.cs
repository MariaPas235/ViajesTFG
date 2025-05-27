namespace ViajesAPI.Models.DTO
{
    /// <summary>
    /// DTO para el proceso de login de usuarios.
    /// Contiene las credenciales necesarias para autenticaci�n.
    /// </summary>
    public class LoginDTO
    {
        /// <summary>
        /// Correo electr�nico del usuario que intenta iniciar sesi�n.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Contrase�a del usuario para autenticaci�n.
        /// </summary>
        public string Password { get; set; }
    }
}
