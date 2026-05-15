using Microsoft.EntityFrameworkCore;
using Shortly.Domain.Entities;

namespace Shortly.Infrastructure.Persistence;

/// <summary>
/// Database context for Managing Users and Links.
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; } = null!;
    
    public DbSet<Link> Links { get; set; } = null!;
}