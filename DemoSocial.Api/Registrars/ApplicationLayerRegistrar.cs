﻿using DemoSocial.Application;

namespace DemoSocial.Api.Registrars;

public class ApplicationLayerRegistrar : IWebApplicationBuilderRegistrar
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Services.AddApplication();
    }
}
