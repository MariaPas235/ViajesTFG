namespace ViajesAPI.Models
{
    public class Valoration
    {
        public int Id { get; set; }
       
        public int Puntuacion { get; set; }
        public string Comentario { get; set; }

        public User User { get; set; }
        public Travel Travel { get; set; }
    }
}
