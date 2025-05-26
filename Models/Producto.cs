namespace FullApi.Models
{
    public class Producto
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }

        public ICollection<DetalleFactura> DetallesFactura { get; set; }
    }
}
