using LOMs.Api;
using LOMs.Domain.Cases.Enums;
using LOMs.Infrastructure.Data;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("LOMs.Test")]

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

        if (!Directory.Exists(builder.Environment.WebRootPath))
        {
            Directory.CreateDirectory(builder.Environment.WebRootPath);
        }

        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new PartyRoleJsonConverter());
                options.JsonSerializerOptions.Converters.Add(new ContractTypeJsonConverter());
                options.JsonSerializerOptions.Converters.Add(new CaseStatusJsonConverter());

            });
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new() { Title = "LOMs API", Version = "v1" });
            options.CustomSchemaIds(type => type.FullName);

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                options.IncludeXmlComments(xmlPath);
            }
        });

        builder.Services
            .AddPresentation()
            .AddApplication()
            .AddInfrastructure(builder.Configuration);

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            });
        });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "LOMs API V1");
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

        app.UseStaticFiles();
        app.UseCoreMiddlewares(builder.Configuration);
        app.UseCors("AllowAll");
        app.MapControllers();
        app.MapStaticAssets();

        app.Run();
    }
}
