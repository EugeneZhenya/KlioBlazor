using KlioBlazor.Data;
using KlioBlazor.Helpers;
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

        public PartitionsController(ApplicationDbContext context, IFileStorageService fileStorageService)
        {
            this.context = context;
            this.fileStorageService = fileStorageService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Partition>>> Get()
        {
            return await context.Partitions.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(Partition partition)
        {
            context.Add(partition);
            await context.SaveChangesAsync();

            if (!string.IsNullOrWhiteSpace(partition.Picture))
            {
                var partitionPicture = Convert.FromBase64String(partition.Picture);
                var filePath = await fileStorageService.SaveFile(partitionPicture, partition.PictureUrl, "partitions");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(filePath);
                Console.ForegroundColor = ConsoleColor.White;
            }

            return partition.Id;
        }
    }
}
