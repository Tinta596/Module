using Microsoft.AspNetCore.Mvc;
using FacturaElectronicaApi.Data;
using FullApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FullApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class ProductosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> ObtenerProducto()
        {
            return await _context.Productos.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> ObtenerProductoId(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) 
                return NotFound();
            return producto;
        }

        [HttpPost]
        public async Task<ActionResult<Producto>> CrearProducto(Producto producto)
        {
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(ObtenerProducto), new { id = producto.ProductoId }, producto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> ActualizarProducto(int id, Producto producto)
        {
            if (id != producto.ProductoId)
                return BadRequest();

            _context.Entry(producto).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> EliminarProducto (int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
                return NotFound();

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
