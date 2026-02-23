using Crm.Sample.Infrastructure.Options;

namespace Crm.Sample.Api.Extensions.Swagger
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app, IConfiguration configuration)
        {
            var swaggerSettings = configuration.GetSection(nameof(SwaggerOptions)).Get<SwaggerOptions>()
                ?? throw new InvalidOperationException($"Failed to bind {nameof(SwaggerOptions)} from configuration.");

            if (!swaggerSettings.Enabled)
                return app;

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint(
                    $"/swagger/{swaggerSettings.Version ?? "v1"}/swagger.json",
                    $"{swaggerSettings.Title ?? "LibraryManager API"} {swaggerSettings.Version ?? "v1"}");

                options.DocumentTitle = swaggerSettings.Title ?? "LibraryManager API";
                options.DefaultModelsExpandDepth(-1);
            });

            return app;
        }
    }
}
