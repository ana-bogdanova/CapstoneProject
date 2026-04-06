using System.Net.Http.Json;
using System.Text.Json;
using MovieApp.Console.Models;

var httpClient = new HttpClient();
httpClient.BaseAddress = new Uri("http://localhost:5016/");

while (true)
{
    Console.WriteLine("\nSelect one of the options to proceed");
    Console.WriteLine("1. Search and save movie");
    Console.WriteLine("2. View favorites / update / delete");
    Console.WriteLine("3. Exit");

    var input = Console.ReadLine();

    if (input == "1")
    {
        await SearchAndSave();
    }
    else if (input == "2")
    {
        await ViewUpdateDeleteFavorites();
    }
    else if (input == "3")
    {
        break;
    }
}

async Task SearchAndSave()
{
    Console.WriteLine("Enter movie name:");
    var name = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(name))
    {
        Console.WriteLine("Please enter a valid movie name.");
        return;
    }

    HttpResponseMessage response = await httpClient.GetAsync($"api/movies/search?title={name}");
    if (!response.IsSuccessStatusCode)
    {
        Console.WriteLine($"Error: {response.StatusCode}");
        Console.WriteLine(await response.Content.ReadAsStringAsync());
        return;
    }

    string jsonResponse = await response.Content.ReadAsStringAsync();
    var movie = JsonSerializer.Deserialize<MovieData>(jsonResponse);

    if (movie == null)
    {
        Console.WriteLine("It seems like there was a problem with getting your movie data. Try again later.");
        return;
    }

    Console.WriteLine($"Found:\nTitle: {movie.Title}\nYear: {movie.Year}");
    Console.WriteLine("Save to favorites? (y/n)");

    if (Console.ReadLine()?.ToLower() == "y")
    {
        await httpClient.PostAsJsonAsync("api/favorites", movie);
        Console.WriteLine("Saved!");
    }
}

async Task ViewUpdateDeleteFavorites()
{
    var movies = await httpClient.GetFromJsonAsync<List<MovieData>>("api/favorites");

    if (movies == null || movies.Count == 0)
    {
        Console.WriteLine("No movies have been saved yet.");
        return;
    }

    Console.WriteLine("Favorites:");
    foreach (var m in movies)
    {
        string commentExists = "";
        if (!string.IsNullOrWhiteSpace(m.Comment))
        {
            commentExists = ", Comment: " + m.Comment;
        }
        Console.WriteLine($"ID: {m.Id}, Title: {m.Title}, Year: {m.Year}{commentExists}");
    }

    Console.WriteLine("\nSelect one of the options:");
    Console.WriteLine("1. Add a Comment by Id");
    Console.WriteLine("2. Delete all favorites");
    Console.WriteLine("3. Return");

    var input = Console.ReadLine();
    if (input == "1")
    {
        Console.WriteLine("Enter Id:");
        int id;
        if (!int.TryParse(Console.ReadLine(), out id))
        {
            Console.WriteLine("Invalid Id.");
            return;
        }

        var movie = movies.Find(m => m.Id == id);
        if (movie == null)
        {
            Console.WriteLine($"Movie with Id {id} not found.");
            return;
        }

        Console.WriteLine("Enter Comment:");
        var comment = Console.ReadLine();

        await httpClient.PutAsync($"api/favorites/{id}/comment?comment={comment}", null);
        Console.WriteLine("Comment added!");
    }
    else if (input == "2")
    {
        await httpClient.DeleteAsync("api/favorites");
        Console.WriteLine("All favorites deleted!");
    }
}