using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FullApi.Models;
using FacturaElectronicaApi.Data;
using Microsoft.EntityFrameworkCore;

namespace FullApi.Services
{
    public interface IFacturaService
    {
        Task<Factura> CrearFactura(Factura factura);
        Task<Factura> ObtenerFacturaPorId (int id);
        Task<IEnumerable<Factura>> ObtenerTodasLasFacturas();
        Task<bool> EliminarFactura(int id);
        Task<decimal> CalcularTotalFactura(int facturaId);
    }

    public class FacturaService : IFacturaService
    {
        private readonly AppDbContext _context;

        public FacturaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Factura> CrearFactura(Factura factura)
        {
            if(!await _context.Clientes.AnyAsync(c => c.ClienteId == factura.ClienteId))
                
                throw new KeyNotFoundException("Cliente no existe");
            //Calculos
            
            factura.Fecha = DateTime.Now;
            foreach (var detalle in factura.Detalles)
            {
                var producto = await _context.Productos.FindAsync(detalle.ProductoId);

                detalle.PrecioUnitario = producto?.Precio ?? 0;
                detalle.Total = detalle.Cantidad * detalle.PrecioUnitario;
            }

            factura.Total = factura.Detalles.Sum(d => d.Total);

            await _context.Facturas.AddAsync(factura);
            await _context.SaveChangesAsync();

            return factura;
        }

        public async Task<Factura> ObtenerFactura(int id)
        {
                return await _context.Facturas
                    .Include(f => f.Cliente)
                    .Include(f => f.Detalles)
                    .ThenInclude(d => d.Producto)
                    .FirstOrDefaultAsync(f => f.FacturaId == id);
        }

         public async Task<List<Factura>> ObtenerTodas()
        {
            return await _context.Facturas
                .Include(f => f.Cliente)
                .Include(f => f.Detalles)
                .ThenInclude(d => d.Producto)
                .ToListAsync();
        }

        public async Task<bool> EliminarFactura(int id)
        {
            var factura = await _context.Facturas
                .Include(f => f.Detalles)
                .FirstOrDefaultAsync(f => f.FacturaId == id);

            if (factura == null) return false;

            _context.DetalleFacturas.RemoveRange(factura.Detalles);
            _context.Facturas.Remove(factura);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<decimal> CalcularTotal(int facturaId)
        {
            var factura = await ObtenerFactura(facturaId);
            return factura?.Total ?? 0;
        }
    }
}
