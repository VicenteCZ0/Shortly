using Shortly.Domain.Entities;

namespace Shortly.Application.Interfaces;

/// <summary>
/// The IUserService interface defines the contract for user-related operations in the application.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Registers a new user in the system.
    /// </summary>
    Task<User> Register(User? user);

    /// <summary>
    /// Retrieves a list of all registered users in the system.
    /// </summary>
    Task<List<User>> GetAllUsers();

    /// <summary>
    /// Authenticates a user by email and password. Returns null if credentials are invalid.
    /// </summary>
    Task<User?> Login(string email, string password);

    /// <summary>
    /// Retrieves a user by their email address. Returns null if not found.
    /// </summary>
    Task<User?> GetUserByEmail(string email);
}