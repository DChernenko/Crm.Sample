using Crm.Sample.Api.Extensions;
using Crm.Sample.Api.Middleware;
using Crm.Sample.Application;
using Crm.Sample.Infrastructure;
using Crm.Sample.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

public partial class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container
        builder.Services.AddControllers();
        
        // Add application layer
        builder.Services.AddApplication(builder.Configuration);

        // Add infrastructure layer
        builder.Services.AddInfrastructure(builder.Configuration);

        builder.Services.AddSwaggerDocumentation(builder.Configuration);
        builder.Services.AddMassTransitWithRabbitMq(builder.Configuration);

        // Add CORS
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
        });
        builder.Services.AddQuartzJobs(builder.Configuration);
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        
        var app = builder.Build();

        // apply migrations automatically on startup
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.Migrate();
        }

        builder.Services.AddProblemDetails();
        app.UseExceptionHandler();

        app.UseHttpsRedirection();
        app.UseCors("AllowAll");
        app.UseAuthorization();
        app.MapControllers();
        app.UseSwaggerDocumentation(app.Configuration, app.Environment);

        app.Run();
    }
}