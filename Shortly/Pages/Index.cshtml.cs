using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shortly.Application.Interfaces;
using Shortly.Domain.Entities;

namespace Shortly.Pages;

public class IndexModel : PageModel
{
    private readonly ILinkService _linkService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILinkService linkService, ILogger<IndexModel> logger)
    {
        _linkService = linkService;
        _logger = logger;
    }

    // Propiedad para la lista de enlaces
    public List<Link> Links { get; set; } = new();

    // Propiedad para recibir la URL del formulario
    [BindProperty]
    public string InputUrl { get; set; } = string.Empty;

    public string UserEmail => HttpContext.Session.GetString("user") ?? string.Empty;

    public async Task<IActionResult> OnGetAsync()
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("user")))
            return RedirectToPage("/Login");

        Links = await _linkService.GetAllLinks();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("user")))
            return RedirectToPage("/Login");

        if (!string.IsNullOrWhiteSpace(InputUrl))
        {
            await _linkService.CreateLink(InputUrl, 1);
            _logger.LogInformation("Nueva URL acortada: {Url}", InputUrl);
        }

        return RedirectToPage();
    }

    public IActionResult OnPostLogout()
    {
        HttpContext.Session.Clear();
        return RedirectToPage("/Login");
    }
}