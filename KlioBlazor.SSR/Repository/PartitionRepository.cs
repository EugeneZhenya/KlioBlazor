using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;
using KlioBlazor.SSR.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace KlioBlazor.SSR.Repository
{
    public class PartitionRepository : IPartitionRepository
    {
        private ApplicationDbContext context { get; set; }
        private IFileStorageService fileStorageService;
        private string containerName = "partitions";

        public PartitionRepository(ApplicationDbContext context, IFileStorageService fileStorageService)
        {
            this.context = context;
            this.fileStorageService = fileStorageService;
        }

        public async Task<List<Partition>> GetAllPartitions()
        {
            return await context.Partitions.ToListAsync();
        }

        public async Task<IndexPartitionsDTO> GetIndexPartitionsDTO()
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

            var allPartitions = await context.Partitions
                .Include(x => x.Category)
                .Include(x => x.Movies)
                .ToListAsync();

            var response = new IndexPartitionsDTO();
            response.LastMovie = movieLast;
            response.LastMovieCountries = Countries;
            response.AllPartitions = allPartitions;

            return response;
        }

        public async Task<Partition> GetPartition(int Id)
        {
            var partition = await context.Partitions.FirstOrDefaultAsync(x => x.Id == Id);
            if (partition == null) { return null; }
            return partition;
        }

        public async Task CreatePartition(Partition partition)
        {
            context.Add(partition);
            await context.SaveChangesAsync();

            if (!string.IsNullOrWhiteSpace(partition.Picture))
            {
                var partitionPicture = Convert.FromBase64String(partition.Picture);
                var filePath = await fileStorageService.SaveFile(partitionPicture, partition.PictureUrl, containerName);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(filePath);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public async Task UpdatePartition(Partition partition)
        {
            if (!string.IsNullOrWhiteSpace(partition.Picture))
            {
                var partitionPicture = Convert.FromBase64String(partition.Picture);
                var filePath = await fileStorageService.SaveFile(partitionPicture, partition.PictureUrl, containerName);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(filePath);
                Console.ForegroundColor = ConsoleColor.White;
            }

            context.Attach(partition).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task DeletePartition(int Id)
        {
            var partition = await context.Partitions.FirstOrDefaultAsync(x => x.Id == Id);
            if (partition == null)
            {
                return;
            }

            context.Remove(partition);
            await context.SaveChangesAsync();
        }
    }
}
