using Shortly.Domain.Entities;

namespace Shortly.Application.Interfaces;

public interface ILinkService
{
    Task<Link> CreateLink(string url, long userId);
    Task<Link> IncrementClicks(long linkId);
    Task<Link> GetLink(string shortUrl);
    Task<List<Link>> GetAllLinks();
}