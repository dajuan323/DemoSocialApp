namespace DemoSocial.Api.Registrars;

public class DbRegistrar : IWebApplicationBuilderRegistrar
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Host.ConfigureServices((hostContext, services) =>
        {
            services.AddPersistence(hostContext.Configuration);
        });
    }
}
