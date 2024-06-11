using DemoSocial.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DemoSocial.Api.Registrars;

public class DbRegistrar : IWebApplicationBuilderRegistrar
{
    private readonly string _conn = "Default"; 
    public void RegisterServices(WebApplicationBuilder builder)
    {
        string cs = builder.Configuration.GetConnectionString(name:_conn);  

        builder.Services
            .AddDbContext<DataContext>(optionsAction:opt => opt.UseSqlServer(cs));

        builder.Services.AddIdentityCore<IdentityUser>()
        //builder.Services
        //    .AddIdentity<IdentityUser, IdentityRole>(options =>
        //    {
        //        options.Password.RequireDigit = false;
        //        options.Password.RequiredLength = 5;
        //        options.Password.RequireLowercase = false;
        //        options.Password.RequireUppercase = false;
        //        options.Password.RequireNonAlphanumeric = false;
        //        options.ClaimsIdentity.UserIdClaimType = "IdentityId";
        //    })

            .AddEntityFrameworkStores<DataContext>();



    }
}
