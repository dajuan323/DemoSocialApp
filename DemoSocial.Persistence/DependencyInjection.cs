using DemoSocial.Application.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoSocial.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        //services.AddDbContext<DataContext>(op => 
        //op.UseSqlServer(configuration.GetConnectionString("Default")));



        //string cs = builder.Configuration.GetConnectionString("Default");

        services
            .AddDbContext<DataContext>(optionsAction: opt => opt.UseSqlServer(configuration.GetConnectionString("Default")));

        services.AddIdentityCore<IdentityUser>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 5;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
        })
            .AddEntityFrameworkStores<DataContext>();

        services.AddTransient<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IDataContext>(sp =>
            sp.GetRequiredService<DataContext>());

        

        return services;
    }
}
