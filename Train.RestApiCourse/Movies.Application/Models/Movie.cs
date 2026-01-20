using System.Text.RegularExpressions;

namespace Movies.Application.Models;

public sealed partial class Movie
{
    public required Guid Id { get; init; }

    public required string Title { get; init; }

    public string Slug => GenerateSlug();

    public required int YearOfRelease { get; set; }

    public required List<string> Genres { get; init; } = new();
    
    private string GenerateSlug()
    {
        var sluggedTitle = Regex.Replace(Title, "[^0-9A-Za-z _-]", string.Empty)
            .ToLower().Replace(" ", "-");
        return $"{sluggedTitle}-{YearOfRelease}";
    }
}
