namespace FullApi.Models
{
    public class DetalleFactura
    {
        public int DettalleFacturaId { get; set; }
        public Factura Factura { get; set; }
        public int FacturaId { get; set; }

        public int ProductoId { get; set; }
        public Producto Producto { get; set; } 

        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Total {  get; set; }
    }
}
