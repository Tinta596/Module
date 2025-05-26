namespace FullApi.Models
{
    public class Factura
    {
        public int FacturaId { get; set; }
        public DateTime Fecha {  get; set; }
        public decimal Total {  get; set; }

        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        public ICollection<DetalleFactura> Detalles { get; set; }
    }
}
