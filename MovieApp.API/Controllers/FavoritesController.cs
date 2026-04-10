using Microsoft.AspNetCore.Mvc;
using MovieApp.Api.Models;

namespace MovieApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FavoritesController : ControllerBase
{
    // In-memory storage
    private static List<Movie> favoriteMovies = new();
    private static int nextId = 1;

    // CREATE
    [HttpPost]
    public ActionResult<Movie> AddFavorite(Movie movie)
    {
        movie.Id = nextId++;
        favoriteMovies.Add(movie);

        return Created($"/api/favorites/{movie.Id}", movie);
    }

    // READ
    [HttpGet]
    public ActionResult<List<Movie>> GetFavorites()
    {
        return Ok(favoriteMovies);
    }

    // UPDATE
    [HttpPut("{id}/comment")]
    public ActionResult<Movie> UpdateComment(int id, string comment)
    {
        var movie = favoriteMovies.FirstOrDefault(m => m.Id == id);

        if (movie == null)
            return NotFound();

        movie.Comment = comment;

        return Ok(movie);
    }

    // DELETE
    [HttpDelete]
    public ActionResult DeleteAll()
    {
        favoriteMovies.Clear();
        return Ok();
    }
}