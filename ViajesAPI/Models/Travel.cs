namespace ViajesAPI.Models
{
    public class Travel
    {
        public int Id { get; set; }
        public string Destino { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal Precio { get; set; }

        public List<Purchase> Purchases { get; set; }
        public List<Valoration> Valorations { get; set; }
    }
}
