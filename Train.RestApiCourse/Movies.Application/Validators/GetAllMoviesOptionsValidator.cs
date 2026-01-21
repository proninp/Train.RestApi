using FluentValidation;
using Movies.Application.Models;

namespace Movies.Application.Validators;

public class GetAllMoviesOptionsValidator : AbstractValidator<GetAllMoviesOptions>
{
    private static readonly string[] AcceptableSortFields =
    {
        "title",
        "yearofrelease"
    };
    
    public GetAllMoviesOptionsValidator()
    {
        RuleFor(o => o.YearOfRelease)
            .LessThanOrEqualTo(DateTime.UtcNow.Year)
            .GreaterThanOrEqualTo(ValidationConstants.FirstMovieReleaseYear);

        RuleFor(o => o.SortField)
            .Must(o => o is null || AcceptableSortFields.Contains(o, StringComparer.OrdinalIgnoreCase))
            .WithMessage("You can only sort by 'title' or 'yearofrelease'.");
    }
}