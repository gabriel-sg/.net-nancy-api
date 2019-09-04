using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using webapi.Models;

public class CustomBootstrapper : DefaultNancyBootstrapper
{
    protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
    {
         // your customization goes here
    }

    protected override void ConfigureApplicationContainer(TinyIoCContainer container)
    {
        base.ConfigureApplicationContainer(container);

        container.Register<ClienteContext>();
    }
}