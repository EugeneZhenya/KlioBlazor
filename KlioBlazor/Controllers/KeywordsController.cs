using KlioBlazor.Data;
using KlioBlazor.Shared.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost]
        public async Task<ActionResult<int>> Post(Keyword keyword)
        {
            context.Add(keyword);
            await context.SaveChangesAsync();
            return keyword.Id;
        }
    }
}
