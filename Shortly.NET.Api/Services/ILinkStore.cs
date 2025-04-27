using Shortly.NET.Api.Models;

namespace Shortly.NET.Api.Services;

public interface ILinkStore
{
    public IEnumerable<Link> GetAll();
    public bool TryGet(string id, out Link link);
    public Link Create(string originalUrl);
    public Link Create(Uri originalUrl);
    public void IncrementHints(string id);
}
