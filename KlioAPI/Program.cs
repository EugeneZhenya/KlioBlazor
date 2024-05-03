using KlioAPI.Data;
using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString, o => o.UseCompatibilityLevel(120).UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));

var app = builder.Build();

app.MapGet("/", () => "Цей прикладний програмний інтерфейс надає доступ лише додаткам, розробленим для медійного архіву KLIO");

app.MapGet("/search", async (
    ApplicationDbContext db,
    [FromQuery(Name = "maxResults")] int MaxResults = 15,
    [FromQuery(Name = "q")] string QueryString = "",
    [FromQuery(Name = "pageNumber")] int PageNumber = 1,
    [FromQuery(Name = "type")] string SearchType = "video") =>
{
    double maxViews = (double)db.Movies.Max(p => p.ViewCounter);

    var moviesQueryable = db.Movies.AsQueryable();

    switch (SearchType)
    {
        case "video":
            moviesQueryable = moviesQueryable.Where(x => x.Title.Contains(QueryString));
            break;
        default:
            break;
    }

    // if (moviesQueryable.Count() == 0) { return Results.NotFound(); }

    return Results.Ok(moviesQueryable);
});

app.Run();
