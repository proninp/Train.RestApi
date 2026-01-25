using System.Text.Json;
using Movies.Api.Sdk;
using Refit;

var moviesApi = RestService.For<IMoviesApi>("http://localhost:5001");

var movie = moviesApi.GetMoviesAsync("jumanji-1995");

Console.WriteLine(JsonSerializer.Serialize(movie));