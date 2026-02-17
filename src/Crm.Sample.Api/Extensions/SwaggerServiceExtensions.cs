using Crm.Sample.Infrastructure.Options;
using Microsoft.OpenApi;
using System.Reflection;

namespace Crm.Sample.Api.Extensions
{
    public static class SwaggerServiceExtensions
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services, IConfiguration configuration)
        {
            var swaggerSettings = configuration.GetSection(nameof(SwaggerOptions)).Get<SwaggerOptions>()
                ?? throw new InvalidOperationException($"Failed to bind {nameof(SwaggerOptions)} from configuration."); ;

            if (swaggerSettings?.Enabled != true)
                return services;

            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(swaggerSettings.Version, new OpenApiInfo
                {
                    Title = swaggerSettings.Title,
                    Version = swaggerSettings.Version,
                    Description = swaggerSettings.Description,
                    Contact = new OpenApiContact
                    {
                        Name = swaggerSettings.ContactName,
                        Email = swaggerSettings.ContactEmail,
                        Url = string.IsNullOrEmpty(swaggerSettings.ContactUrl) ? null : new Uri(swaggerSettings.ContactUrl)
                    },
                    License = new OpenApiLicense
                    {
                        Name = swaggerSettings.LicenseName,
                        Url = string.IsNullOrEmpty(swaggerSettings.LicenseUrl) ? null : new Uri(swaggerSettings.LicenseUrl)
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                
                if (File.Exists(xmlPath))
                    options.IncludeXmlComments(xmlPath);
            });

            return services;
        }
    }
}
