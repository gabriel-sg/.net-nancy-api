using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using webapi.Models;
using Nancy;
using Nancy.ModelBinding;
using System;

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
                context.Clientes.Add(new Cliente { Nombre = "Cliente2", Apellido = "ap2", Direccion = "dir2" });
                context.SaveChanges();
            }

            // Get("/", opt => "Hello World!");
            Get("/", opt => {
                return Response.AsJson<List<Cliente>>(context.Clientes.ToList());
            });            

            Get("/{id:long}", opt => {
                long id = opt.id;
                Cliente cliente = context.Clientes.Find(id);
                if(cliente == null){
                    return Response.AsJson(new {msg = "Bad request"}, Nancy.HttpStatusCode.BadRequest);
                    // return $"No existe cliente con id: {opt.id}";
                }
                return Response.AsJson<Cliente>(cliente);
            });

            Post("/", opt => {
                var cliente = this.Bind<Cliente>();

                // TODO: Validar cliente

                if(string.IsNullOrEmpty(cliente.Nombre) || string.IsNullOrEmpty(cliente.Apellido) || string.IsNullOrEmpty(cliente.Direccion)){
                    return Response.AsJson(new {msg = "Bad request"}, Nancy.HttpStatusCode.BadRequest);
                    // return "400 bad request?";
                }
                
                context.Clientes.Add(cliente);
                context.SaveChanges();
                
                return this.Response.AsJson(new { id = cliente.Id });
            });

            Put("/{id:long}", opt => {
                long id = opt.id;
                Cliente cliente = this.Bind<Cliente>(new BindingConfig{BodyOnly = true});
                
                // TODO:Validar cliente

                if(id != cliente.Id){
                    return Response.AsJson(new {msg = "Bad request"}, Nancy.HttpStatusCode.BadRequest);
                }

                // valido que exista un cliente con el id indicado
                if(context.Clientes.Find(id) == null){
                    return Response.AsJson(new {msg = "Bad request"}, Nancy.HttpStatusCode.BadRequest);                    
                }

                context.Entry(cliente).State = EntityState.Modified;
                context.SaveChanges();

                return Response.AsJson(new {msg = "No Content"}, Nancy.HttpStatusCode.NoContent);
            });

            Delete("/{id:long}", opt => {
                long id = opt.id;
                Cliente cliente = context.Clientes.Find(id);
                
                if(cliente == null){
                    return Response.AsJson(new {msg = "Not Found"}, Nancy.HttpStatusCode.NotFound);    
                }
                
                context.Clientes.Remove(cliente);
                context.SaveChanges();

                return Response.AsJson(new {msg = "No Content"}, Nancy.HttpStatusCode.NoContent); 
            });
            
        }   
    }
}
