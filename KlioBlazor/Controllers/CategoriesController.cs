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


        [HttpGet("category/{name}")]
        public async Task<ActionResult<IndexCategoriesDTO>> Get(string name)
        {
            int catId = 0;
            Movie? movieLast;
            List<Category> allCategories;
            List<Country> Countries = new List<Country>();
            List<Partition> allPartitions = new List<Partition>();

            switch (name)
            {
                case "history":
                    catId = 1;
                    break;
                case "literacy":
                    catId = 2;
                    break;
                case "music":
                    catId = 3;
                    break;
                case "movies":
                    catId = 4;
                    break;
                case "serials":
                    catId = 5;
                    break;
                case "animation":
                    catId = 6;
                    break;
                case "series":
                    catId = 7;
                    break;
            }

            if (catId == 0)
            {
                movieLast = await context.Movies
                    .OrderByDescending(x => x.PublicDate)
                    .Include(x => x.Partition).ThenInclude(x => x.Category)
                    .Include(x => x.MoviesGenres).ThenInclude(x => x.Genre)
                    .Include(x => x.MoviesCountries).ThenInclude(x => x.Country)
                    .FirstOrDefaultAsync();
            }
            else
            {
                movieLast = await context.Movies
                    .OrderByDescending(x => x.PublicDate)
                    .Include(x => x.Partition).ThenInclude(x => x.Category)
                    .Include(x => x.MoviesGenres).ThenInclude(x => x.Genre)
                    .Include(x => x.MoviesCountries).ThenInclude(x => x.Country)
                    .Where(x => x.Partition.CategoryId == catId)
                    .FirstOrDefaultAsync();
            }

            if (movieLast != null)
            {
                movieLast.MoviesGenres = movieLast.MoviesGenres.OrderBy(x => x.Order).ToList();
                movieLast.MoviesCountries = movieLast.MoviesCountries.OrderBy(x => x.Order).ToList();
                Countries = movieLast.MoviesCountries.Select(x => x.Country).ToList();
            }


            if (catId == 0)
            {
                allCategories = await context.Categories
                    .Include(x => x.Partitions)
                    .ToListAsync();
            }
            else
            {
                allCategories = await context.Categories
                    .Include(x => x.Partitions)
                    .Where(x => x.Id == catId)
                    .OrderBy(x => x.Name)
                    .ToListAsync();

                allPartitions = await context.Partitions
                    .Include(x => x.Category)
                    .Include(x => x.Movies)
                    .Where(x => x.CategoryId == catId)
                    .OrderBy(x => x.Name)
                    .ToListAsync();
            }

            var response = new IndexCategoriesDTO();
            response.LastMovie = movieLast;
            response.LastMovieCountries = Countries;
            response.AllCategories = allCategories;
            response.CategoryPartitions = allPartitions;

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

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            context.Remove(category);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
