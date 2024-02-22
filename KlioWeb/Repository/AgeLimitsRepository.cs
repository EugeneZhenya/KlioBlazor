using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;
using KlioBlazor.Shared.Enums;
using KlioWeb.Data;
using KlioWeb.Helpers;
using Microsoft.EntityFrameworkCore;

namespace KlioWeb.Repository
{
    public class AgeLimitsRepository : IAgeLimitsRepository
    {
        private readonly ApplicationDbContext context;

        public AgeLimitsRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IndexAgeLimitsDTO> GetIndexAgeLimitsDTO()
        {
            var limitLasts = 3;
            double maxViews = (double)context.Movies.Max(p => p.ViewCounter);

            var movieLast = await context.Movies
                .OrderByDescending(x => x.PublicDate)
                .Include(x => x.Partition).ThenInclude(x => x.Category)
                .Include(x => x.MoviesGenres).ThenInclude(x => x.Genre)
                .Include(x => x.MoviesCountries).ThenInclude(x => x.Country)
                .FirstOrDefaultAsync();

            movieLast.MoviesGenres = movieLast.MoviesGenres.OrderBy(x => x.Order).ToList();
            movieLast.MoviesCountries = movieLast.MoviesCountries.OrderBy(x => x.Order).ToList();
            var Countries = movieLast.MoviesCountries.Select(x => x.Country).ToList();

            List<AgeCategory> allAges = new List<AgeCategory>();
            allAges.Add(AgeCategory.Zero);
            allAges.Add(AgeCategory.Twelve);
            allAges.Add(AgeCategory.Sixteen);
            allAges.Add(AgeCategory.Eighteen);

            List<int> listCounters = new List<int>();
            int zeroCount = await context.Movies.Where(x => x.AgeLimit == (int)AgeCategory.Zero).CountAsync();
            listCounters.Add(zeroCount);
            int twelveCount = await context.Movies.Where(x => x.AgeLimit == (int)AgeCategory.Twelve).CountAsync();
            listCounters.Add(twelveCount);
            int sixteenCount = await context.Movies.Where(x => x.AgeLimit == (int)AgeCategory.Sixteen).CountAsync();
            listCounters.Add(sixteenCount);
            int eighteenCount = await context.Movies.Where(x => x.AgeLimit == (int)AgeCategory.Eighteen).CountAsync();
            listCounters.Add(eighteenCount);

            var allLastMovies = await context.Movies
                    .OrderByDescending(x => x.PublicDate)
                    .Include(x => x.Partition).ThenInclude(x => x.Category)
                    .ToListAsync();

            var moviesLast = allLastMovies.Take(limitLasts).ToList();

            foreach (var movie in moviesLast)
            {
                movie.Rating = Math.Truncate((double)movie.ViewCounter / (double)maxViews * 10000) / 100;
            }

            var response = new IndexAgeLimitsDTO();
            response.LastMovie = movieLast;
            response.LastMovieCountries = Countries;
            response.AllAges = allAges;
            response.AgeCounters = listCounters;
            response.LastAdded = moviesLast;

            return response;
        }

        public async Task<DetailsAgeLimitsDTO> GetDetailsAgeLimitsDTO(FilterMoviesDTO filterMoviesDTO)
        {
            var limitLasts = 3;
            double maxViews = (double)context.Movies.Max(p => p.ViewCounter);
            var moviesQueryable = context.Movies.AsQueryable();
            moviesQueryable = moviesQueryable.Where(x => x.AgeLimit == filterMoviesDTO.Age).OrderBy(x => x.ReleaseDate);

            double count = await moviesQueryable.CountAsync();
            double totalAmountPages = Math.Ceiling(count / filterMoviesDTO.RecordsPerPage);

            var lsstMovie = await context.Movies
                .Where(x => x.AgeLimit == filterMoviesDTO.Age)
                .OrderByDescending(x => x.PublicDate)
                .FirstOrDefaultAsync();

            var allMovies = await moviesQueryable
                .Paginate(filterMoviesDTO.Pagination)
                .Include(x => x.Partition).ThenInclude(x => x.Category)
                .ToListAsync();

            foreach (var film in allMovies)
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

            var ageMovies = new PaginatedResponse<List<Movie>>() { Response = allMovies, TotalAmountPages = (int)totalAmountPages, TotalRecords = (int)count };

            var model = new DetailsAgeLimitsDTO();
            model.LastMovie = lsstMovie;
            model.AgesMovies = ageMovies;
            model.LastAdded = moviesLast;

            return model;
        }
    }
}
