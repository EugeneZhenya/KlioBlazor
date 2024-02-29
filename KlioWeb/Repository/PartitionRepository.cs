using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;
using KlioWeb.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace KlioWeb.Repository
{
    public class PartitionRepository : IPartitionRepository
    {
        private readonly ApplicationDbContext context;

        public PartitionRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<List<Partition>> GetAllPartitions()
        {
            return await context.Partitions.ToListAsync();
        }

        public async Task<IndexPartitionsDTO> GetIndexPartitionsDTO()
        {
            var limitLasts = 3;
            double maxViews = (double)context.Movies.Max(p => p.ViewCounter);

            var movieLast = await context.Movies
                .OrderByDescending(x => x.PublicDate)
                .Include(x => x.Partition).ThenInclude(x => x.Category)
                .Include(x => x.MoviesGenres).ThenInclude(x => x.Genre)
                .Include(x => x.MoviesCountries).ThenInclude(x => x.Country)
                .Include(x => x.MovieInfos)
                .FirstOrDefaultAsync();

            movieLast.MoviesGenres = movieLast.MoviesGenres.OrderBy(x => x.Order).ToList();
            movieLast.MoviesCountries = movieLast.MoviesCountries.OrderBy(x => x.Order).ToList();
            var Countries = movieLast.MoviesCountries.Select(x => x.Country).ToList();

            var allPartitions = await context.Partitions
                .OrderBy(x => x.Name)
                .Include(x => x.Category)
                .Include(x => x.Movies)
                .ToListAsync();

            var allLastMovies = await context.Movies
                    .OrderByDescending(x => x.PublicDate)
                    .Include(x => x.Partition)
                    .ToListAsync();

            var moviesLast = allLastMovies.Take(limitLasts).ToList();

            foreach (var movie in moviesLast)
            {
                movie.Rating = Math.Truncate((double)movie.ViewCounter / (double)maxViews * 10000) / 100;
            }

            var response = new IndexPartitionsDTO();
            response.LastMovie = movieLast;
            response.LastMovieCountries = Countries;
            response.AllPartitions = allPartitions;
            response.LastAdded = moviesLast;

            return response;
        }

        public async Task<Partition> GetPartition(int Id)
        {
            var partition = await context.Partitions.FirstOrDefaultAsync(x => x.Id == Id);
            if (partition == null) { return null; }
            return partition;
        }

        public async Task<DetailsPartitionDTO> GetDetailsPartitionDTO(int Id)
        {
            var limitLasts = 3;
            double maxViews = (double)context.Movies.Max(p => p.ViewCounter);

            var partition = await context.Partitions
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == Id);

            if (partition == null) { return null; }

            var lsstMovie = await context.Movies
                .Where(x => x.PatitionId == Id)
                .OrderByDescending(x => x.PublicDate)
                .FirstOrDefaultAsync();

            var allMoviews = await context.Movies
                .Where(x => x.PatitionId == Id)
                .OrderBy(x => x.ReleaseDate)
                .ToListAsync();

            foreach (var film in allMoviews)
            {
                film.Rating = Math.Truncate((double)film.ViewCounter / (double)maxViews * 10000) / 100;
            }

            var allLastMovies = await context.Movies
                    .OrderByDescending(x => x.PublicDate)
                    .Include(x => x.Partition).ThenInclude(x => x.Category)
                    .ToListAsync();

            var moviesLast = allLastMovies.Take(limitLasts).ToList();

            foreach (var movie in moviesLast)
            {
                movie.Rating = Math.Truncate((double)movie.ViewCounter / (double)maxViews * 10000) / 100;
            }

            var model = new DetailsPartitionDTO();
            model.Partition = partition;
            model.LastMovie = lsstMovie;
            model.PartitionMovies = allMoviews;
            model.LastAdded = moviesLast;

            return model;
        }
    }
}
