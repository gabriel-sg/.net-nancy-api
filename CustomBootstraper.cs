
using System;
using System.Collections.Generic;
using System.Reflection;

using Nancy;
using Nancy.Bootstrapper;
using Nancy.Configuration;
using Nancy.Conventions;
using Nancy.Cryptography;
using Nancy.Diagnostics;
using Nancy.Security;
using Nancy.Session;
using Nancy.TinyIoc;
using Nancy.ViewEngines.Razor;
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