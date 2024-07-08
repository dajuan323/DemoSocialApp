
namespace DemoSocial.Api.Registrars;

public class CorsRegistrar : IWebApplicationBuilderRegistrar
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
            options.AddPolicy("DemoSocialPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .WithOrigins("https://demo-social-api.azurewebsites.net/")
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .AllowCredentials();
            })
        );
    }
}
