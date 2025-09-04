using LOMs.Api;
using LOMs.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

// áæ ÇáãÌáÏ ãÇ ßÇä ãæÌæÏ¡ ÃäÔÆå
if (!Directory.Exists(builder.Environment.WebRootPath))
{
    Directory.CreateDirectory(builder.Environment.WebRootPath);
}

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services
    .AddPresentation()
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

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

    //await app.InitialiseDatabaseAsync();
}
else
{
    app.UseHsts();
}

app.UseStaticFiles();

app.UseCoreMiddlewares(builder.Configuration);

app.MapControllers();

app.MapStaticAssets();

await SeedDataBase();

app.Run();
return;

async Task SeedDataBase()
{
    using var scope = app.Services.CreateScope();
    var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();
    await initialiser.InitialiseAsync();
}
