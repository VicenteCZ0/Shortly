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
}