using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using webapi.Models;
using Nancy;

namespace webapi.Controllers
{
    public class ClientesModule: NancyModule
    {
        public ClientesModule(ClienteContext context): base("api/clientes")
        {
            if (context.Clientes.Count() == 0)
            {
                // Create a new Cliente if collection is empty,
                // which means you can't delete all Clientes.
                context.Clientes.Add(new Cliente { Nombre = "Cliente1", Apellido = "ap1", Direccion = "dir1" });
                context.SaveChanges();
            }

            // Get("/", opt => "Hello World!");
            Get("/", opt => {
                return Response.AsJson<List<Cliente>>(context.Clientes.ToList());
            });            
        }   
    }
}
