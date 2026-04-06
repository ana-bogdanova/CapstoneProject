using System.Net.Http;
using System.Text.Json;
using MovieApp.Api.Models;

namespace MovieApp.Api.Services;

public class ExternalApiService
{
    private string apiKey = "dee47dcd";

    public async Task<Movie?> SearchMovieAsync(string title)
    {
        HttpClient client = new HttpClient();
        string url = $"http://www.omdbapi.com/?t={title}&apikey={apiKey}";

        HttpResponseMessage response = await client.GetAsync(url);

        if (response.IsSuccessStatusCode == false)
        {
            return null;
        }

        string json = await response.Content.ReadAsStringAsync();
        JsonDocument doc = JsonDocument.Parse(json);
        JsonElement root = doc.RootElement;

        string? responseValue = root.GetProperty("Response").GetString();
        if (responseValue == "False")
        {
            return null;
        }

        JsonElement titleElement = root.GetProperty("Title");
        JsonElement yearElement = root.GetProperty("Year");
        JsonElement imdbElement = root.GetProperty("imdbID");

        Movie movie = new Movie();
        movie.Title = titleElement.GetString();
        movie.Year = yearElement.GetString();
        movie.ImdbId = imdbElement.GetString();

        return movie;
    
    }
}