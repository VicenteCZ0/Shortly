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

    public async Task OnGetAsync()
    {
        // Al cargar la página, traemos todos los links
        Links = await _linkService.GetAllLinks();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!string.IsNullOrWhiteSpace(InputUrl))
        {
            // Usamos el ID 1 (el admin que sembramos) para simplificar
            await _linkService.CreateLink(InputUrl, 1);
            _logger.LogInformation("Nueva URL acortada: {Url}", InputUrl);
        }

        return RedirectToPage();
    }
}