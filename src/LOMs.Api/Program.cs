using LOMs.Api;
using LOMs.Contract.Commons.Enums;
using LOMs.Domain.Cases.Enums;
using LOMs.Infrastructure.BackgroundJobs;
using LOMs.Infrastructure.BackgroundServicesJob.CourtSessionReminder;
using LOMs.Infrastructure.Data;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

//  Configure web root
builder.Environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
Directory.CreateDirectory(builder.Environment.WebRootPath);

//  Register services
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new PartyRoleJsonConverter());
        options.JsonSerializerOptions.Converters.Add(new ContractTypeJsonConverter());
        options.JsonSerializerOptions.Converters.Add(new CaseStatusJsonConverter());
        options.JsonSerializerOptions.Converters.Add(new PartyRoleArabicJsonConverter());
        options.JsonSerializerOptions.Converters.Add(new CaseStatusArabicJsonConverter());



    });

builder.Services
    .AddPresentation(builder.Configuration)
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddHostedService<CourtSessionReminderService>();

/** 
 * Configure Serilog as the primary logging provider for the application host.
 * This replaces the default logging providers (Console, Debug, etc.) with Serilog.
 * 
 * The lambda receives:
 * - context: gives access to configuration and environment
 * - loggerConfig: the Serilog LoggerConfiguration instance
 * 
 * We instruct Serilog to read its settings from the configuration file (e.g., appsettings.json),
 * including sinks (like Seq), enrichers, filters, and output templates.
 */

builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration));

/**
 * ⚠️ Important:
 * When using Serilog this way, it becomes the **only active logging provider**.
 * Other built-in providers (ConsoleLoggerProvider, DebugLoggerProvider, etc.)
 * are **not registered** unless explicitly added.
 * 
 * 🔗 Why this matters for Seq:
 * Seq relies on Serilog's sink to receive structured logs.
 * If Serilog is not the active provider, logs won't be routed to Seq
 * even if the sink is configured correctly.
 * 
 * ✅ This setup ensures full compatibility with Seq and other Serilog-based sinks.
 */

var app = builder.Build();

// ?? Configure middleware
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "LOMs API V1");
        options.DocumentTitle = "LOMs API Docs";
        options.EnableDeepLinking();
        options.DisplayRequestDuration();
        options.EnableFilter();
    });

    await app.InitialiseDatabaseAsync();
}
else
{
    app.UseHsts();
}

app.UseCoreMiddlewares(builder.Configuration);
app.MapControllers();
app.MapStaticAssets();

app.Run();
