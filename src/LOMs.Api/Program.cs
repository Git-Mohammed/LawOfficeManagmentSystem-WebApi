using LOMs.Api;
using LOMs.Domain.Identity;
using LOMs.Infrastructure.BackgroundJobs;
using LOMs.Infrastructure.Data;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
Directory.CreateDirectory(builder.Environment.WebRootPath);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration));

builder.Services
    .AddControllers()
    .ConfigureJsonConverters();

builder.Services
    .AddPresentation(builder.Configuration,builder.Environment)
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddHostedService<CourtSessionReminderService>();

var app = builder.Build();

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
}
else
{
    app.UseHsts();
}

app.UseCoreMiddlewares(builder.Configuration);
app.MapControllers();
app.MapStaticAssets();

await app.InitialiseDatabaseAsync();
app.Run();
