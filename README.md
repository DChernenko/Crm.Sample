# Crm.Sample Solution

Overview

This solution contains a sample CRM application implemented with clean architecture layers: API, Application, and Infrastructure. It targets .NET 10.

Projects

- `src/Crm.Sample.Api` - ASP.NET Core Web API project (entry point)
- `src/Crm.Sample.Application` - Application layer (business logic, interfaces)
- `src/Crm.Sample.Infrastructure` - Infrastructure layer (EF Core, email, MassTransit, Quartz jobs, etc.)

Requirements

- .NET 10 SDK
- (Optional) `dotnet-ef` tool to manage EF Core migrations: `dotnet tool install --global dotnet-ef`
- Services used by the solution may include SQL Server/Postgres, RabbitMQ, SMTP server depending on configuration.

Build and run

1. Restore and build:

   `dotnet restore`
   `dotnet build`

2. Run the API (from solution root):

   `dotnet run --project src/Crm.Sample.Api`

Configuration

- Application configuration is in `appsettings.json` (and environment-specific variants). An optional `appsecrets.json` may be loaded by the API at startup.
- Common settings include database connection string, email settings (`EmailOptions`), RabbitMQ settings for MassTransit, and Quartz job configuration.
- Use user secrets or environment variables for sensitive values in development and production.

Database migrations

The API performs automatic migrations at startup (the sample calls `db.Database.Migrate()` during startup). To manage migrations manually:

1. Add the `dotnet-ef` tool (if not already installed).
2. Create or apply migrations from the appropriate project, for example:

   `dotnet ef migrations add InitialCreate --project src/Crm.Sample.Infrastructure --startup-project src/Crm.Sample.Api`
   `dotnet ef database update --project src/Crm.Sample.Infrastructure --startup-project src/Crm.Sample.Api`

Email

Email settings are configured via `EmailOptions` (see `src/Crm.Sample.Infrastructure/Options`). Ensure SMTP server, port, username and password are provided when email delivery is required.

Troubleshooting

- If you see "The service collection cannot be modified because it is read-only", ensure all service registrations (`builder.Services.Add...`) happen before `var app = builder.Build()`.
- Check logs for startup exceptions and ensure external services (database, RabbitMQ, SMTP) are reachable.

Contributing

- Open issues or PRs with concise description and steps to reproduce.

License

- No license specified. Add a license file if needed.
