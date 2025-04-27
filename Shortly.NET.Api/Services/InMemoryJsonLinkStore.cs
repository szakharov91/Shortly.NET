using System.Security.Cryptography;
using System.Text.Json;
using System.Xml.Linq;
using Shortly.NET.Api.Models;


namespace Shortly.NET.Api.Services;

public class InMemoryJsonLinkStore : ILinkStore
{
    private readonly string _file = "links.json";
    private readonly Dictionary<string, Link> _links;

    private readonly JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };

    public InMemoryJsonLinkStore()
    {
        if (File.Exists(_file))
            _links = JsonSerializer.Deserialize<Dictionary<string, Link>>(File.ReadAllText(_file))!;
        else
            _links = new();
    }

    public Link Create(string originalUrl)
    {
        var id = GenerateId();
        var link = new Link { Id = id, OriginalUrl = originalUrl };
        _links[id] = link;
        Save();

        return link;
    }

    public IEnumerable<Link> GetAll() => _links.Values;

    public void IncrementHints(string id)
    {
        if(_links.TryGetValue(id, out var link))
        {
            link.Hits++;
            Save();
        }
    }

    public bool TryGet(string id, out Link link) => _links.TryGetValue(id, out link!);

    private void Save()
    {
        File.WriteAllText(_file, JsonSerializer.Serialize(_links, options));
    }

    private static string GenerateId()
    {
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        return new string(Enumerable.Range(0, 6).Select(_ => chars[RandomNumberGenerator.GetInt32(chars.Length)]).ToArray());
    }

    public Link Create(Uri originalUrl) => Create(originalUrl.AbsoluteUri);
}
