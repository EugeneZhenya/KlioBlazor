using KlioAPI.Data;
using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString, o => o.UseCompatibilityLevel(120).UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));

var app = builder.Build();

app.MapGet("/", () => "Цей прикладний програмний інтерфейс надає доступ лише додаткам, розробленим для медійного архіву KLIO");

app.MapGet("/movies", async (ApplicationDbContext db) =>
    await db.Movies.ToListAsync());

app.MapGet("/movie/{id}", async (int id, ApplicationDbContext db) =>
{
    //await db.Movies.FindAsync(id)
    //is Movie movie
    //    ? Results.Ok(movie)
    //    : Results.NotFound()

    var movie = await db.Movies.Where(x => x.Id == id).FirstOrDefaultAsync();

    if (movie == null) { return Results.NotFound(); }

    var model = new DetailsMovieDTO();
    model.Movie = movie;

    return Results.Ok(movie);
});

app.Run();
