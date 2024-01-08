using KlioBlazor.Data;
using KlioBlazor.Shared.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KlioBlazor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KeywordsController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public KeywordsController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Keyword>>> Get()
        {
            return await context.Keywords.ToListAsync();
        }

        [HttpGet("search/{searchText}")]
        public async Task<ActionResult<List<Keyword>>> FilterByWord(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText)) { return new List<Keyword>(); }
            return await context.Keywords.Where(x => x.Word.Contains(searchText)).Take(5).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(Keyword keyword)
        {
            context.Add(keyword);
            await context.SaveChangesAsync();
            return keyword.Id;
        }
    }
}
