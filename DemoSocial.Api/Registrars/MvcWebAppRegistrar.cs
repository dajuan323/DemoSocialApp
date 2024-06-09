
using Asp.Versioning.Builder;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;

namespace DemoSocial.Api.Registrars;

public class MvcWebAppRegistrar : IWebApplicationRegistrar
{
    public void RegisterPipelineComponents(WebApplication app)
    {

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                    description.ApiVersion.ToString());
            }
        });
        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        ApiVersionSet apiVersionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .HasApiVersion(new ApiVersion(2))
            .ReportApiVersions()
            .Build();

        RouteGroupBuilder versionedBroup = app
            .MapGroup("api/v{apiVersion:apiVersion}")
            .WithApiVersionSet(apiVersionSet);
    }
}
