using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using webapi.Models;


namespace webapi.Controllers
{
    [Route("api/[controller]")] // [controller] es para tomar el nombre desde la clase "Clientes"Controlles.
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly ClienteContext _context;

        public ClientesController(ClienteContext context)
        {
            _context = context;

            if (_context.Clientes.Count() == 0)
            {
                // Create a new Cliente if collection is empty,
                // which means you can't delete all Clientes.
                _context.Clientes.Add(new Cliente { Nombre = "Cliente1", Apellido = "ap1", Direccion = "dir1" });
                _context.SaveChanges();
            }
        }

        // GET /api/clientes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            return await _context.Clientes.ToListAsync();
        }

        // GET /api/clientes/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetCliente(long id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if(cliente == null)
            {
                return NotFound();
            }
            return cliente; 
        }

        // POST /api/clientes
        [HttpPost]
        public async Task<ActionResult<Cliente>> StoreCliente(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
            
            return CreatedAtAction(nameof(GetCliente), new { id = cliente.Id }, cliente);
        }




    }
}