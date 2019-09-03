using Microsoft.EntityFrameworkCore;

namespace webapi.Models
{
    public class ClienteContext : DbContext
    {
        public ClienteContext(DbContextOptions<ClienteContext> options) : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
    }
}