using LOMs.Api;
using LOMs.Contract.Commons.Enums;
using LOMs.Domain.Cases.Enums;
using LOMs.Domain.Cases.Judgments;
using LOMs.Domain.Identity;
using LOMs.Infrastructure.BackgroundJobs;
using LOMs.Infrastructure.Data;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
Directory.CreateDirectory(builder.Environment.WebRootPath);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new PartyRoleJsonConverter());
        options.JsonSerializerOptions.Converters.Add(new ContractTypeJsonConverter());
        options.JsonSerializerOptions.Converters.Add(new CaseStatusJsonConverter());
        options.JsonSerializerOptions.Converters.Add(new PartyRoleArabicJsonConverter());
        options.JsonSerializerOptions.Converters.Add(new CaseStatusArabicJsonConverter());
        options.JsonSerializerOptions.Converters.Add(new cContractTypeJsonConverter());
        options.JsonSerializerOptions.Converters.Add(new JudgmentTypeJsonConverter());
        options.JsonSerializerOptions.Converters.Add(new JudgmentSubTypeJsonConverter());
        options.JsonSerializerOptions.Converters.Add(new cJudgmentTypeJsonConverter());
        options.JsonSerializerOptions.Converters.Add(new cJudgmentSubTypeJsonConverter());
        options.JsonSerializerOptions.Converters.Add(new cRoleJsonConverter());
        options.JsonSerializerOptions.Converters.Add(new RoleJsonConverter());
    });

builder.Services
    .AddPresentation(builder.Configuration)
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddHostedService<CourtSessionReminderService>();

builder.Configuration.AddEnvironmentVariables();

builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration));

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
