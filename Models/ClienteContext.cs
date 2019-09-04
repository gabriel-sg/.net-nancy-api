using Microsoft.EntityFrameworkCore;

namespace webapi.Models
{
    public class ClienteContext : DbContext
    {
        public ClienteContext(DbContextOptions<ClienteContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("ListaClientes");
        }

        public DbSet<Cliente> Clientes { get; set; }
    }
}