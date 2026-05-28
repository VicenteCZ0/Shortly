using Microsoft.EntityFrameworkCore;
using Shortly.Application.Interfaces;
using Shortly.Domain.Entities;
using Shortly.Infrastructure.Persistence;

namespace Shortly.Application.Services;

/// <summary>
/// Provides services for managing users, including registration and retrieval of user information.
/// </summary>
public sealed class UserService : IUserService
{
    private readonly ILogger<UserService> _logger;
    private readonly AppDbContext _context;

    public UserService(AppDbContext context, ILogger<UserService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<User> Register(User? user)
    {
        // Validar que el objeto no sea nulo
        if (user == null)
        {
            _logger.LogError("Registration failed: User object is null.");
            throw new ArgumentNullException(nameof(user));
        }

        _logger.LogDebug("Attempting to register user with email: {Email}", user.Email);

        // Validar si el correo ya existe
        var existUser = await _context.Users.AsNoTracking().AnyAsync(u => u.Email == user.Email);
        if (existUser)
        {
            _logger.LogError("Registration failed: Email {Email} is already in use.", user.Email);
            throw new InvalidOperationException("Email is already registered.");
        }

        // Encriptar la contraseña antes de guardar
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

        // Guardar en la base de datos
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("User registered successfully with email: {Email} and id: {Id}.", user.Email, user.Id);
        return user;
    }

    public async Task<List<User>> GetAllUsers()
    {
        _logger.LogDebug("Retrieving all users from the database..");
        var users = await _context.Users.AsNoTracking().ToListAsync();
        _logger.LogInformation("Retrieved {Count} users from the database.", users.Count);
        return users;
    }

    public async Task<User?> Login(string email, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
        {
            _logger.LogWarning("Login failed for email: {Email}", email);
            return null;
        }

        _logger.LogInformation("Login successful for email: {Email}", email);
        return user;
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
    }
}