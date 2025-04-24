namespace ViajesAPI.Models
{
    public class Purchase
    {
        public int Id { get; set; }
        public DateTime FechaCompra { get; set; }

        public User User { get; set; }
        public Travel Travel { get; set; }
    }
}
