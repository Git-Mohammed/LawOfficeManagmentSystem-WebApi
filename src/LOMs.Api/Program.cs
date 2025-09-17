using LOMs.Api;
using LOMs.Domain.Cases.Enums;
using LOMs.Infrastructure.BackgroundJobs;
using LOMs.Infrastructure.BackgroundServicesJob.CourtSessionReminder;
using LOMs.Infrastructure.Data;
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
    });

builder.Services
    .AddPresentation(builder.Configuration)
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddHostedService<CourtSessionReminderService>();

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
