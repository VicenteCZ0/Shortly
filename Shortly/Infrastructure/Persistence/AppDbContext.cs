using Microsoft.EntityFrameworkCore;
using Shortly.Domain.Entities;

namespace Shortly.Infrastructure.Persistence;

/// <summary>
/// Database context for Managing Users and Links.
/// </summary>
public class AppDbContext : DbContext
{
    [cite_start]public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { } [cite: 605]

    public DbSet<User> Users { get; set; } = null!; [cite: 624]
    public DbSet<Link> Links { get; set; } = null!; [cite: 627]
}