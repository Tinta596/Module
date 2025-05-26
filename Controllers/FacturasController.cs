using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FacturaElectronicaApi.Data;
using FullApi.Models;

namespace FullApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FacturasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FacturasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Facturas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Factura>>> GetFacturas()
        {
            return await _context.Facturas
                .Include(f => f.Cliente)
                .Include(f => f.Detalles)
                    .ThenInclude(d => d.Producto)
                .ToListAsync();
        }

        // GET: api/Facturas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Factura>> GetFactura(int id)
        {
            var factura = await _context.Facturas
                .Include(f => f.Cliente)
                .Include(f => f.Detalles)
                    .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(f => f.FacturaId == id);

            if (factura == null)
                return NotFound();

            return factura;
        }

        // POST: api/Facturas
        [HttpPost]
        public async Task<ActionResult<Factura>> CrearFactura(Factura factura)
        {
            foreach (var detalle in factura.Detalles)
            {
                detalle.Total = detalle.Cantidad * detalle.PrecioUnitario;
            }

            factura.Total = factura.Detalles.Sum(d => d.Total);

            _context.Facturas.Add(factura);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFactura), new { id = factura.FacturaId }, factura);
        }

        // DELETE: api/Facturas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarFactura(int id)
        {
            var factura = await _context.Facturas
                .Include(f => f.Detalles)
                .FirstOrDefaultAsync(f => f.FacturaId == id);

            if (factura == null)
                return NotFound();

            _context.DetalleFacturas.RemoveRange(factura.Detalles);
            _context.Facturas.Remove(factura);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
