using Microsoft.EntityFrameworkCore;
using Shortly.Domain.Entities;
using Shortly.Infrastructure.Persistence;

namespace Shortly.Infrastructure.Seed;

public class DbInitializer
{
    public static async Task Seed(AppDbContext context, ILogger<DbInitializer> log)
    {
        log.LogInformation("Applying migrations...");
        // Esto crea la base de datos automáticamente si no existe y aplica las migraciones
        await context.Database.MigrateAsync();

        log.LogInformation("Seeding database...");
        // Si no hay ningún usuario en la tabla, creamos uno por defecto
        if (!context.Users.Any())
        {
            var admin = new User("admin@shortly.com", BCrypt.Net.BCrypt.HashPassword("123456"));
            context.Users.Add(admin);
            await context.SaveChangesAsync();
            log.LogInformation("Default user seeded.");
        }
    }
}