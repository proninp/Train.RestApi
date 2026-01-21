using FluentValidation;
using Movies.Application.Models;
using Movies.Application.Repositories;

namespace Movies.Application.Validators;

public sealed class MovieValidator : AbstractValidator<Movie>
{
    private const int FirstMovieReleaseYear = 1888;

    private readonly IMovieRepository _movieRepository;

    public MovieValidator(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;

        RuleFor(m => m.Id)
            .NotEmpty();

        RuleFor(m => m.Genres)
            .NotEmpty();

        RuleFor(m => m.Title)
            .NotEmpty();

        RuleFor(m => m.YearOfRelease)
            .LessThanOrEqualTo(DateTime.UtcNow.Year)
            .GreaterThanOrEqualTo(FirstMovieReleaseYear);

        RuleFor(m => m.Slug)
            .MustAsync(ValidateSlug)
            .WithMessage("This movie already exists in the system");
    }

    private async Task<bool> ValidateSlug(Movie movie, string slug, CancellationToken token = default)
    {
        var existingMovie = await _movieRepository.GetBySlugAsync(slug, token: token);
        if (existingMovie is not null)
        {
            return existingMovie.Id == movie.Id;
        }

        return existingMovie is null;
    }
}