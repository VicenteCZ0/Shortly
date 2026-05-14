using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shortly.Domain.Entities;

/// <summary>
/// Represents a user in the system.
/// </summary>
[Table("users")]
public class User
{
    /// <summary>
    /// Gets the unique identifier for the user.
    /// </summary>
    [Key]
    public long Id { get; private set; }

    /// <summary>
    /// Gets the email address of the user.
    /// This is a required field and must be unique.
    /// </summary>
    [Required]
    [MaxLength(320)]
    public string Email { get; private set; } = null!;

    /// <summary>
    /// Gets the password of the user.
    /// This is a required field and should be stored securely (e.g., hashed).
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Password { get; set; } = null!;

    /// <summary>
    /// Gets the collection of links associated with the user.
    /// </summary>
    public ICollection<Link> Links { get; private set; } = new List<Link>();

    // Constructor para Entity Framework
    private User() { }

    /// <summary>
    /// Initializes a new instance with email and password.
    /// </summary>
    public User(string email, string password)
    {
        Email = string.IsNullOrWhiteSpace(email) 
            ? throw new ArgumentException("Email is required", nameof(email)) 
            : email.Trim();
            
        Password = string.IsNullOrWhiteSpace(password) 
            ? throw new ArgumentException("Password is required", nameof(password)) 
            : password;
    }
}