using DemoSocial.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DemoSocial.Api.Registrars;

public class DbRegistrar : IWebApplicationBuilderRegistrar
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        var cs = builder.Configuration.GetConnectionString("default");
        builder.Services.AddDbContext<DataContext>(opt => opt.UseSqlServer(cs));
    }
}
