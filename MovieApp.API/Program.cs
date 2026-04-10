using MovieApp.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSingleton<ExternalApiService>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();