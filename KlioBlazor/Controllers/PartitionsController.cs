using KlioBlazor.Data;
using KlioBlazor.Helpers;
using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KlioBlazor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartitionsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IFileStorageService fileStorageService;
        private string containerName = "partitions";

        public PartitionsController(ApplicationDbContext context, IFileStorageService fileStorageService)
        {
            this.context = context;
            this.fileStorageService = fileStorageService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<Partition>>> GetAllPartitions()
        {
            return await context.Partitions.ToListAsync();
        }

        [HttpGet]
        public async Task<ActionResult<IndexPartitionsDTO>> Get()
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

        [HttpGet("{id}")]
        public async Task<ActionResult<Partition>> Get(int id)
        {
            var partition = await context.Partitions.FirstOrDefaultAsync(x => x.Id == id);
            if (partition == null) { return NotFound(); }
            return partition;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(Partition partition)
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

            return partition.Id;
        }

        [HttpPut]
        public async Task<ActionResult> Put(Partition partition)
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
            return NoContent();
        }
    }
}
