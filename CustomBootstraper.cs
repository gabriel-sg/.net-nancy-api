
using System;
using System.Diagnostics;

using Nancy;
using Nancy.Bootstrapper;
using Nancy.Configuration;
using Nancy.Conventions;
using Nancy.Cryptography;
using Nancy.Diagnostics;
using Nancy.Security;
using Nancy.Session;
using Nancy.TinyIoc;
using webapi.Utils;
using Metrics;

public class CustomBootstrapper : DefaultNancyBootstrapper
{
    protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
    {
        // your customization goes here
        base.ApplicationStartup(container, pipelines);

        pipelines.BeforeRequest.AddItemToStartOfPipeline(ctx =>
        {
            ctx.Items["RequestStartTimeKey"] = Clock.Default.Nanoseconds;
            return null;
        });

        pipelines.AfterRequest.AddItemToEndOfPipeline(ctx =>
        {
            long endTime = Clock.Default.Nanoseconds;
            if (ctx.ResolvedRoute != null && !(ctx.ResolvedRoute is Nancy.Routing.NotFoundRoute))
            {
                long startTime = (long)ctx.Items["RequestStartTimeKey"];
                long elapsed = endTime - startTime;
                string name = string.Format("{0} {1}", ctx.ResolvedRoute.Description.Method, ctx.ResolvedRoute.Description.Path);
                double milisegundos = elapsed / (double)1000000;
                Console.WriteLine($"{milisegundos}ms -> {name}");
            }
        });
    }

    protected override void ConfigureApplicationContainer(TinyIoCContainer container)
    {
        base.ConfigureApplicationContainer(container);

        // container.Register<ClienteContext>();
    }

    public override void Configure(INancyEnvironment environment)
    {
        environment.Diagnostics(
                enabled: true,
                password: "password",
                path: "/_Nancy",
                cookieName: "__custom_cookie",
                slidingTimeout: 30,
                cryptographyConfiguration: CryptographyConfiguration.NoEncryption);

        environment.Tracing(
            enabled: true,
            displayErrorTraces: true);
    }




}