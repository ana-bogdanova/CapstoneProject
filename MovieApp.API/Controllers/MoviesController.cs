using Microsoft.AspNetCore.Mvc;
using MovieApp.Api.Services;

namespace MovieApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MoviesController : ControllerBase
{
    public ExternalApiService externalApiService { get; set; }
    public MoviesController(ExternalApiService externalApiService)
    {
        this.externalApiService = externalApiService;
    }
    
    //SEARCH
    [HttpGet("search")]
    public async Task<IActionResult> SearchMovies(string title)
    {
        var results = await externalApiService.SearchMovieAsync(title);

        if (results == null)
            return NotFound();

        return Ok(results);
    }
}