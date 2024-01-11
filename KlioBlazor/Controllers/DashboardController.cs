using KlioBlazor.Components.Shared;
using KlioBlazor.Data;
using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KlioBlazor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public DashboardController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<DashboardDTO>> Get()
        {
            var response = new DashboardDTO();

            response.CategoriesCount = await context.Categories.CountAsync();
            response.PartitionsCount = await context.Partitions.CountAsync();
            response.KeywordsCount = await context.Keywords.CountAsync();
            response.GenresCount = await context.Genres.CountAsync();
            response.CountriesCount = await context.Countries.CountAsync();
            response.CreatorsCount = await context.Creators.CountAsync();
            response.PeopleCount = await context.People.CountAsync();
            response.MoviesCount = await context.Movies.CountAsync();

            return response;
        }
    }
}
