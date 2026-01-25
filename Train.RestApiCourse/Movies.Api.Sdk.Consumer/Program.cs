using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Movies.Api.Sdk;
using Movies.Contracts.Requests;
using Refit;

var services = new ServiceCollection();

services.AddRefitClient<IMoviesApi>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://localhost:5001"));

var provider = services.BuildServiceProvider();

var moviesApi = provider.GetRequiredService<IMoviesApi>();

var getAllRequest = new GetAllMoviesRequest
{
    SortBy = null,
    Title = null,
    Year = null,
    Page = 1,
    PageSize = 3
};

var movies = await moviesApi.GetMoviesAsync(getAllRequest);

foreach (var movie in movies.Items)
{
    Console.WriteLine(JsonSerializer.Serialize(movie));
}