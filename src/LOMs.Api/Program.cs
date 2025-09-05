using LOMs.Api;
using LOMs.Infrastructure.Data;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");


if (!Directory.Exists(builder.Environment.WebRootPath))
{
    Directory.CreateDirectory(builder.Environment.WebRootPath);
}

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "LOMs API", Version = "v1" });

    // ?? This line resolves the schema ID conflict
    options.CustomSchemaIds(type => type.FullName);
    // Optional: Include XML comments if you want summaries/descriptions
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
        policy
            .AllowAnyOrigin()   // Allow any (Postman, Swagger, Angular, React,..)
            .AllowAnyMethod()   // Allow any Request (GET, POST, PUT, DELETE ...)
            .AllowAnyHeader();  // Allow any Header
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
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

//await SeedDataBase();

app.Run();
return;

//async Task SeedDataBase()
//{
//    using var scope = app.Services.CreateScope();
//    var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();
//    await initialiser.InitialiseAsync();
//}
