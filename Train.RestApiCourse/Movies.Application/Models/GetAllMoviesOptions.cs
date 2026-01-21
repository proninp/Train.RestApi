namespace Movies.Application.Models;

public sealed class GetAllMoviesOptions
{
    public string? Title { get; set; }
    
    public int? YearOfRelease { get; set; }
    
    public Guid? UserId { get; set; }
}