using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using webapi.Models;
using Nancy;
using Nancy.ModelBinding;

namespace webapi.Controllers
{
    public class ClientesModule : NancyModule
    {
        public ClientesModule(ClienteContext context) : base("api/clientes")
        {
            if (context.Clientes.Count() == 0)
            {
                // Create a new Cliente if collection is empty,
                // which means you can't delete all Clientes.
                context.Clientes.Add(new Cliente { Nombre = "Cliente1", Apellido = "ap1", Direccion = "dir1" });
                // context.Clientes.Add(new Cliente { Nombre = "Cliente2", Apellido = "ap2", Direccion = "dir2" });
                context.SaveChanges();
            }

            // Get("/", opt => "Hello World!");
            Get("/", opt =>
            {
                return Response.AsJson<List<Cliente>>(context.Clientes.ToList());
            });

            Get("/{id:long}", opt =>
            {
                long id = opt.id;
                Cliente cliente = context.Clientes.Find(id);
                if (cliente == null)
                {
                    return Response.AsJson(new { msg = "Not Found" }, Nancy.HttpStatusCode.NotFound);
                }
                return Response.AsJson<Cliente>(cliente);
            });

            Post("/", opt =>
            {
                var cliente = this.Bind<Cliente>(c => c.Id); // para que el id no pueda ser seteado y solo sea autoincrement.

                // Los campos de cliente no pueden estar vacios o ser nulos. (Por decision propia)
                if (string.IsNullOrEmpty(cliente.Nombre) || string.IsNullOrEmpty(cliente.Apellido) || string.IsNullOrEmpty(cliente.Direccion))
                {
                    return Response.AsJson(new { msg = "Bad request" }, Nancy.HttpStatusCode.BadRequest);
                }

                context.Clientes.Add(cliente);
                context.SaveChanges();

                // Respondo OK con el id asignado
                return this.Response.AsJson(new { id = cliente.Id });
            });

            Put("/{id:long}", opt =>
            {
                long id = opt.id;
                Cliente newCli = this.Bind<Cliente>(new BindingConfig { BodyOnly = true }); // tiene que tomar los datos solo del body. Sino estaba utilizando el id de la query string.

                // Validación
                if (id != newCli.Id || string.IsNullOrEmpty(newCli.Nombre) || string.IsNullOrEmpty(newCli.Apellido) || string.IsNullOrEmpty(newCli.Direccion))
                {
                    return Response.AsJson(new { msg = "Bad request" }, Nancy.HttpStatusCode.BadRequest);
                }

                // Verifico que exista un cliente con el id indicado.
                Cliente oldCli = context.Clientes.Find(id); // si existe lo adjunta al contexto (DbContext).
                if (oldCli == null)
                {
                    return Response.AsJson(new { msg = "Not Found" }, Nancy.HttpStatusCode.NotFound);
                }

                oldCli.Nombre = newCli.Nombre;
                oldCli.Apellido = newCli.Apellido;
                oldCli.Direccion = newCli.Direccion;

                // context.Entry(cliente).State = EntityState.Modified;
                context.SaveChanges();

                return Response.AsJson("", Nancy.HttpStatusCode.NoContent);
            });

            Delete("/{id:long}", opt =>
            {
                long id = opt.id;
                Cliente cliente = context.Clientes.Find(id);

                if (cliente == null)
                {
                    return Response.AsJson(new { msg = "Not Found" }, Nancy.HttpStatusCode.NotFound);
                }

                context.Clientes.Remove(cliente);
                context.SaveChanges();

                return Response.AsJson("", Nancy.HttpStatusCode.NoContent);
            });
        }
    }
}
