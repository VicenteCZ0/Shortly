using Microsoft.EntityFrameworkCore;
using Shortly.Application.Interfaces;
using Shortly.Domain.Entities;
using Shortly.Infrastructure.Persistence;

namespace Shortly.Application.Services;

public sealed class LinkService : ILinkService
{
    private readonly ILogger<LinkService> _logger;
    private readonly AppDbContext _context;

    public LinkService(AppDbContext context, ILogger<LinkService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Link> CreateLink(string url, long userId)
    {
        // Generamos una cadena corta aleatoria de 6 caracteres para la URL
        var shortUrl = Guid.NewGuid().ToString("N").Substring(0, 6); 
        var link = new Link(url, shortUrl, userId);
        
        _context.Links.Add(link);
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("Link created successfully. ShortUrl: {ShortUrl}", shortUrl);
        return link;
    }

    public async Task<Link> IncrementClicks(long linkId)
    {
        var link = await _context.Links.FindAsync(linkId);
        if (link != null)
        {
            link.IncrementClicks();
            await _context.SaveChangesAsync();
        }
        return link!;
    }

    public async Task<Link> GetLink(string shortUrl)
    {
        return await _context.Links.FirstOrDefaultAsync(l => l.ShortUrl == shortUrl);
    }

    public async Task<List<Link>> GetAllLinks()
    {
        return await _context.Links.AsNoTracking().ToListAsync();
    }
}