using KlioAPI.Data;
using KlioBlazor.Shared.API;
using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

const string KlioContentUrl = "https://raw.githubusercontent.com/EugeneZhenya/KlioMoviesContent/main/";
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

    double count = await moviesQueryable.CountAsync();
    double totalAmountPages = Math.Ceiling(count / (double)MaxResults);

    var result = new VideoSearchResult();
    result.Items = moviesQueryable
                .OrderBy(x => x.ReleaseDate)
                .Skip((PageNumber - 1) * MaxResults)
                .Take(MaxResults)
                .Include(x => x.Partition)
                .ToList();
    result.PageInfo = new PageInfo() { ResultsPerPage = MaxResults, TotalResults = (int)count };

    if (PageNumber < totalAmountPages) {
        result.NextPageNumber = PageNumber + 1;
    }
    else
    {
        result.NextPageNumber = 1;
    }

    foreach (var film in result.Items)
    {
        film.Rating = Math.Truncate((double)film.ViewCounter / (double)maxViews * 10000) / 100;
        film.Poster = $"{KlioContentUrl}movies/{film.Id}/cover.jpg";
        film.Background = $"{KlioContentUrl}movies/{film.Id}/background.jpg";
        var partition = db.Partitions.FirstOrDefault(x => x.Id == film.PatitionId);
        film.Partition = new Partition()
        {
            Id = film.PatitionId,
            Name = partition.Name,
            CategoryId = partition.CategoryId,
            Description = partition.Description,
            Picture = $"{KlioContentUrl}partitions/{partition.PictureUrl}"
        };
        var category = db.Categories.FirstOrDefault(x => x.Id == partition.CategoryId);
        film.Partition.Category = new Category()
        {
            Id = partition.CategoryId,
            Name = category.Name,
            Description = category.Description
        };
    }

    return Results.Ok(result);
});

app.Run();
