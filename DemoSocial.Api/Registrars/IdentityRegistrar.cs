
using DemoSocial.Application.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DemoSocial.Api.Registrars;

public class IdentityRegistrar : IWebApplicationBuilderRegistrar
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        var jwtSettings = new JwtSettings();
        builder.Configuration.Bind(nameof(JwtSettings), jwtSettings);

        var jwtSection = builder.Configuration.GetSection(nameof(JwtSettings));
        builder.Services.Configure<JwtSettings>(jwtSection);
        //builder.Services.AddIdentityCore<IdentityUser>();
        builder.Services
            .AddIdentity<IdentityOptions, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 5;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.ClaimsIdentity.UserIdClaimType = "IdentityId";
            });
        builder.Services.AddAuthentication(a =>
        {
            a.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            a.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            a.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(jwt =>
        {
            jwt.SaveToken = true;
            jwt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SigningKey)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudiences = jwtSettings.Audiences,
                RequireExpirationTime = false,
                ValidateLifetime = true
            };
            jwt.Audience = jwtSettings.Audiences[0];
            jwt.ClaimsIssuer = jwtSettings.Issuer;
        });
    }
}
