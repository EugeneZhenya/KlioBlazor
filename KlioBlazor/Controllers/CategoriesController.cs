using KlioBlazor.Data;
using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace KlioBlazor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public CategoriesController(ApplicationDbContext context)
        {
            this.context = context;
        }


        [HttpGet("all")]
        public async Task<ActionResult<List<Category>>> GetAllCategories()
        {
            return await context.Categories.ToListAsync();
        }


        [HttpGet]
        public async Task<ActionResult<IndexCategoriesDTO>> Get()
        {
            var movieLast = await context.Movies
                .OrderByDescending(x => x.PublicDate)
                .Include(x => x.Partition).ThenInclude(x => x.Category)
                .Include(x => x.MoviesGenres).ThenInclude(x => x.Genre)
                .Include(x => x.MoviesCountries).ThenInclude(x => x.Country)
                .FirstOrDefaultAsync();

            movieLast.MoviesGenres = movieLast.MoviesGenres.OrderBy(x => x.Order).ToList();
            movieLast.MoviesCountries = movieLast.MoviesCountries.OrderBy(x => x.Order).ToList();
            var Countries = movieLast.MoviesCountries.Select(x => x.Country).ToList();

            var allCategories = await context.Categories
                .Include(x => x.Partitions)
                .ToListAsync();

            var response = new IndexCategoriesDTO();
            response.LastMovie = movieLast;
            response.LastMovieCountries = Countries;
            response.AllCategories = allCategories;

            return response;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> Get (int id)
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null) { return NotFound(); }
            return category;
        }


        [HttpPost]
        public async Task<ActionResult<int>> Post(Category category)
        {
            context.Add(category);
            await context.SaveChangesAsync();
            return category.Id;
        }

        [HttpPut]
        public async Task<ActionResult> Put(Category category)
        {
            context.Attach(category).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
