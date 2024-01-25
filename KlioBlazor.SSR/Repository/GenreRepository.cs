using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;
using KlioBlazor.SSR.Helpers;
using Microsoft.EntityFrameworkCore;
using System;

namespace KlioBlazor.SSR.Repository
{
    public class GenreRepository : IGenreRepository
    {
        private ApplicationDbContext context { get; set; }
        private IFileStorageService fileStorageService;
        private string containerName = "genres";

        public GenreRepository(ApplicationDbContext context, IFileStorageService fileStorageService)
        {
            this.context = context;
            this.fileStorageService = fileStorageService;
        }

        public async Task<List<Genre>> GetAllGenres()
        {
            return await context.Genres.ToListAsync();
        }

        public async Task<IndexGenresDTO> GetIndexGenresDTO()
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

            var allGenres = await context.Genres
                .Include(x => x.MoviesGenres)
                .ToListAsync();

            var response = new IndexGenresDTO();
            response.LastMovie = movieLast;
            response.LastMovieCountries = Countries;
            response.AllGenres = allGenres;

            return response;
        }

        public async Task<Genre> GetGenre(int Id)
        {
            var genre = await context.Genres.FirstOrDefaultAsync(x => x.Id == Id);
            if (genre == null) { return null; }
            return genre;
        }

        public async Task CreateGenre(Genre genre)
        {
            context.Add(genre);
            await context.SaveChangesAsync();

            if (!string.IsNullOrWhiteSpace(genre.Icon))
            {
                var genreIcon = Convert.FromBase64String(genre.Icon);
                var filePath = await fileStorageService.SaveFile(genreIcon, genre.IconUrl, containerName);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(filePath);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public async Task UpdateGenre(Genre genre)
        {
            if (!string.IsNullOrWhiteSpace(genre.Icon))
            {
                var genreIcon = Convert.FromBase64String(genre.Icon);
                var filePath = await fileStorageService.SaveFile(genreIcon, genre.IconUrl, containerName);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(filePath);
                Console.ForegroundColor = ConsoleColor.White;
            }

            context.Attach(genre).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task DeleteGenre(int Id)
        {
            var genre = await context.Genres.FirstOrDefaultAsync(x => x.Id == Id);
            if (genre == null)
            {
                return;
            }

            context.Remove(genre);
            await context.SaveChangesAsync();
        }
    }
}
