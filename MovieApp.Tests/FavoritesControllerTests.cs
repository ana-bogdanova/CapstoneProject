using Xunit;
using MovieApp.Api.Controllers;
using MovieApp.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

public class FavoritesControllerTests
{
    // CREATE

    [Fact]
    public void AddFavorite_ReturnsCreatedMovie()
    {
        // Arrange
        var controller = new FavoritesController();
        controller.DeleteAll();

        var movie = new Movie { Title = "Love" };

        // Act
        var result = controller.AddFavorite(movie);

        // Assert
        var createdResult = Assert.IsType<CreatedResult>(result.Result);
        var returnedMovie = Assert.IsType<Movie>(createdResult.Value);

        Assert.Equal("Love", returnedMovie.Title);
        Assert.True(returnedMovie.Id > 0);
    }

    [Fact]
    public void AddFavorite_AddsMovieToList()
    {
        // Arrange
        var controller = new FavoritesController();
        controller.DeleteAll();

        var movie = new Movie { Title = "Weapons" };

        // Act
        controller.AddFavorite(movie);
        var result = controller.GetFavorites();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var movies = Assert.IsType<List<Movie>>(okResult.Value);

        Assert.Single(movies);
    }

    // READ

    [Fact]
    public void GetFavorites_ReturnsMovies_WhenListNotEmpty()
    {
        // Arrange
        var controller = new FavoritesController();
        controller.DeleteAll();

        controller.AddFavorite(new Movie { Title = "Youth" });

        // Act
        var result = controller.GetFavorites();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var movies = Assert.IsType<List<Movie>>(okResult.Value);

        Assert.NotEmpty(movies);
    }

    [Fact]
    public void GetFavorites_ReturnsEmptyList_WhenNoMovies()
    {
        // Arrange
        var controller = new FavoritesController();
        controller.DeleteAll();

        // Act
        var result = controller.GetFavorites();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var movies = Assert.IsType<List<Movie>>(okResult.Value);

        Assert.Empty(movies);
    }

    // UPDATE

    [Fact]
    public void UpdateComment_ReturnsUpdatedMovie_WhenMovieExists()
    {
        // Arrange
        var controller = new FavoritesController();
        controller.DeleteAll();

        var addResult = controller.AddFavorite(new Movie { Title = "Runner" });
        var created = (addResult.Result as CreatedResult)?.Value as Movie;

        // Act
        var result = controller.UpdateComment(created.Id, "Great!");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var updatedMovie = Assert.IsType<Movie>(okResult.Value);

        Assert.Equal("Great!", updatedMovie.Comment);
    }

    [Fact]
    public void UpdateComment_ReturnsNotFound_WhenMovieDoesNotExist()
    {
        // Arrange
        var controller = new FavoritesController();
        controller.DeleteAll();

        // Act
        var result = controller.UpdateComment(999, "Bugonia");

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    // DELETE

    [Fact]
    public void DeleteAll_RemovesAllMovies()
    {
        // Arrange
        var controller = new FavoritesController();
        controller.DeleteAll();

        controller.AddFavorite(new Movie { Title = "Nobody" });

        // Act
        controller.DeleteAll();
        var result = controller.GetFavorites();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var movies = Assert.IsType<List<Movie>>(okResult.Value);

        Assert.Empty(movies);
    }

    [Fact]
    public void DeleteAll_ReturnsOk_WhenListAlreadyEmpty()
    {
        // Arrange
        var controller = new FavoritesController();
        controller.DeleteAll();

        // Act
        var result = controller.DeleteAll();

        // Assert
        Assert.IsType<OkResult>(result);
    }
}