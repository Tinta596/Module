namespace FullApi.Models
{
    public class Cliente
    {
        public int ClienteId { get; set; }
        public string Nombre { get; set; }
        public string DocumentoIdentidad { get; set; }
        public string Direccion { get; set; }
        public string Email { get; set; }

        public ICollection<Factura> Facturas { get; set; }
    }
}
