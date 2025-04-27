namespace Shortly.NET.Api.Models;

public record Link
{
    /// <summary> Short key like: a123as </summary>
    public string Id { get; init; }
    public string OriginalUrl { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    
    /// <summary> Redirection counter </summary>
    public int Hits { get; set; }
}
