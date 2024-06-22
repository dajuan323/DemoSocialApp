namespace DemoSocial.Api.Registrars;

public class DbRegistrar : IWebApplicationBuilderRegistrar
{
    
    public void RegisterServices(WebApplicationBuilder builder)
    {

        builder.Host.ConfigureServices((hostContext, services) =>
        {
            services.AddPersistence(hostContext.Configuration);

        });

        // DELETE IF APP RUNS AND FUNCTIONS
        //string cs = builder.Configuration.GetConnectionString("Default");  

        //builder.Services
        //    .AddDbContext<DataContext>(optionsAction:opt => opt.UseSqlServer(cs));

        //builder.Services.AddIdentityCore<IdentityUser>(options =>
        //    {
        //        options.Password.RequireDigit = false;
        //        options.Password.RequiredLength = 5;
        //        options.Password.RequireLowercase = false;
        //        options.Password.RequireUppercase = false;
        //        options.Password.RequireNonAlphanumeric = false;
        //    })

        //    .AddEntityFrameworkStores<DataContext>();



    }
}
