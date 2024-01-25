using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;
using KlioBlazor.SSR.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace KlioBlazor.SSR.Repository
{
    public class CreatorRepository : ICreatorRepository
    {
        private ApplicationDbContext context { get; set; }
        private IFileStorageService fileStorageService;
        private string containerName = "creators";

        public CreatorRepository(ApplicationDbContext context, IFileStorageService fileStorageService)
        {
            this.context = context;
            this.fileStorageService = fileStorageService;
        }

        public async Task<List<Creator>> GetAllCreators()
        {
            return await context.Creators.ToListAsync();
        }

        public async Task<IndexCreatorsDTO> GetIndexCreatorsDTO()
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

            var allCreators = await context.Creators
                .Include(x => x.MoviesCreators)
                .ToListAsync();

            var response = new IndexCreatorsDTO();
            response.LastMovie = movieLast;
            response.LastMovieCountries = Countries;
            response.AllCreators = allCreators;

            return response;
        }

        public async Task<Creator> GetCreator(int Id)
        {
            var creator = await context.Creators.FirstOrDefaultAsync(x => x.Id == Id);
            if (creator == null) { return null; }
            return creator;
        }

        public async Task<List<Creator>> GetCreatorByTitle(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) { return new List<Creator>(); }
            return await context.Creators.Where(x => x.Title.Contains(name)).Take(5).ToListAsync();
        }

        public async Task CreateCreator(Creator creator)
        {
            context.Add(creator);
            await context.SaveChangesAsync();

            if (!string.IsNullOrWhiteSpace(creator.Logo))
            {
                var creatorLogo = Convert.FromBase64String(creator.Logo);
                var filePath = await fileStorageService.SaveFile(creatorLogo, creator.LogoUrl, containerName);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(filePath);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public async Task UpdateCreator(Creator creator)
        {
            if (!string.IsNullOrWhiteSpace(creator.Logo))
            {
                var creatorLogo = Convert.FromBase64String(creator.Logo);
                var filePath = await fileStorageService.SaveFile(creatorLogo, creator.LogoUrl, containerName);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(filePath);
                Console.ForegroundColor = ConsoleColor.White;
            }

            context.Attach(creator).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task DeleteCreator(int Id)
        {
            var creator = await context.Creators.FirstOrDefaultAsync(x => x.Id == Id);
            if (creator == null)
            {
                return;
            }

            context.Remove(creator);
            await context.SaveChangesAsync();
        }
    }
}
