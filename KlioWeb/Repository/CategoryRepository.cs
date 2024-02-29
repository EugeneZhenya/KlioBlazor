using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;
using KlioWeb.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace KlioWeb.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext context;

        public CategoryRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<List<Category>> GetAllCategories()
        {
            return await context.Categories.ToListAsync();
        }

        public async Task<Category> GetCategory(int Id)
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == Id);
            if (category == null) { return null; }
            return category;
        }

        public async Task<IndexCategoriesDTO> GetCategoryByName(string name)
        {
            int catId = 0;
            var limitLasts = 3;
            Movie? movieLast;
            List<Category> allCategories;
            List<Country> Countries = new List<Country>();
            List<Partition> allPartitions = new List<Partition>();
            List<Movie> allLastMovies;

            double maxViews = (double)context.Movies.Max(p => p.ViewCounter);

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
                    catId = 6;
                    break;
                case "animation":
                    catId = 5;
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
                    .Include(x => x.MovieInfos)
                    .FirstOrDefaultAsync();
            }
            else
            {
                movieLast = await context.Movies
                    .OrderByDescending(x => x.PublicDate)
                    .Include(x => x.Partition).ThenInclude(x => x.Category)
                    .Include(x => x.MoviesGenres).ThenInclude(x => x.Genre)
                    .Include(x => x.MoviesCountries).ThenInclude(x => x.Country)
                    .Include(x => x.MovieInfos)
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

                allLastMovies = await context.Movies
                    .OrderByDescending(x => x.PublicDate)
                    .Include(x => x.Partition)
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

                allLastMovies = await context.Movies
                    .OrderByDescending(x => x.PublicDate)
                    .Include(x => x.Partition).Where(x => x.Partition.CategoryId == catId)
                    .ToListAsync();
            }

            var moviesLast = allLastMovies.Take(limitLasts).ToList();

            foreach (var movie in moviesLast)
            {
                movie.Rating = Math.Truncate((double)movie.ViewCounter / (double)maxViews * 10000) / 100;
            }

            var response = new IndexCategoriesDTO();
            response.LastMovie = movieLast;
            response.LastMovieCountries = Countries;
            response.AllCategories = allCategories;
            response.CategoryPartitions = allPartitions;
            response.LastAdded = moviesLast;

            return response;
        }
    }
}
