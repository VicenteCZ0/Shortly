using Microsoft.EntityFrameworkCore;
using Serilog;
using Shortly.Application.Interfaces;
using Shortly.Application.Services;
using Shortly.Infrastructure.Persistence;
using Shortly.Infrastructure.Seed;

var builder = WebApplication.CreateBuilder(args);

// Agregar Razor Pages
builder.Services.AddRazorPages();

// Configurar el contexto de base de datos con SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("AppDbContext") ?? 
    throw new InvalidOperationException("Connection string 'AppDbContext' not found.")));

// Inyección de dependencias para los servicios (¡Punto clave para la rúbrica de Patrones!)
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ILinkService, LinkService>();

// Configurar Serilog
builder.Host.UseSerilog((hostingContext, services, configuration) =>
{
    configuration.ReadFrom.Configuration(hostingContext.Configuration);
});

var app = builder.Build();

// Configurar el flujo de solicitudes HTTP (Pipeline)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages().WithStaticAssets();

// Inicialización y acceso al Logger
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var logFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
    
    var log = logFactory.CreateLogger<Program>();
    var initLog = logFactory.CreateLogger<DbInitializer>();

    log.LogDebug("Initializing..");
    
    // Ejecutamos la migración y la siembra de datos
    await DbInitializer.Seed(dbContext, initLog);
    
    log.LogDebug("Initializing ok.");
}

app.MapGet("/{shortUrl}", async (string shortUrl, ILinkService linkService) =>
{
    var link = await linkService.GetLink(shortUrl);
    if (link == null) return Results.NotFound("Enlace no encontrado.");

    await linkService.IncrementClicks(link.Id);
    return Results.Redirect(link.Url);
});

app.Run();