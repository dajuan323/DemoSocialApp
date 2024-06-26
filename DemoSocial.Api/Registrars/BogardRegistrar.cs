﻿namespace DemoSocial.Api.Registrars;

public class BogardRegistrar : IWebApplicationBuilderRegistrar
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Services.AddAutoMapper(typeof(Program).Assembly, typeof(GetAllUserProfiles).Assembly);
        builder.Services.AddMediatR(config=>config.RegisterServicesFromAssembly(Assembly.GetAssembly(typeof(GetAllUserProfiles))));
    }
}
