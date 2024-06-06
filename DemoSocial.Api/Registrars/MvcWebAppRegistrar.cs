
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
            IReadOnlyList<ApiVersionDescription> descriptions = app.DescribeApiVersions();

            foreach (ApiVersionDescription description in descriptions)
            {
                string url = $"/swagger/{description.GroupName}/swagger.json";
                string name = description.GroupName.ToUpperInvariant();

                options.SwaggerEndpoint(url, name);
            }
        });
        app.UseHttpsRedirection();

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
