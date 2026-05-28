using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shortly.Application.Interfaces;

namespace Shortly.Pages;

public class LoginModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ILogger<LoginModel> _logger;

    public LoginModel(IUserService userService, ILogger<LoginModel> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [BindProperty]
    public string Email { get; set; } = string.Empty;

    [BindProperty]
    public string Password { get; set; } = string.Empty;

    public string? ErrorMessage { get; set; }
    public string? SuccessMessage { get; set; }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _userService.Login(Email, Password);
        if (user == null)
        {
            ErrorMessage = "Correo o contraseña incorrectos.";
            return Page();
        }

        _logger.LogInformation("User {Email} logged in.", Email);
        HttpContext.Session.SetString("user", user.Email);
        return RedirectToPage("/Index");
    }
}
