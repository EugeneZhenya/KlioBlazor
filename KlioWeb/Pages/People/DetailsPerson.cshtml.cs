using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;
using KlioWeb.Pages.Partitions;
using KlioWeb.Pages.Shared;
using KlioWeb.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KlioWeb.Pages.People
{
    public class DetailsPersonModel : PageModel
    {
        public AppState AppState = new AppState();
        [BindProperty(SupportsGet = true)]
        public int PersonId { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? PersonName { get; set; }

        private readonly ILogger<DetailsPersonModel> _logger;
        private readonly IPersonRepository _personRepository;

        public DetailsPersonDTO model;
        public List<Movie> moviesByPerson;
        public string nameOfPerson;
        public List<Movie> LastAdded { get; set; }

        public MoviesArea PersonMovies = new MoviesArea();
        public MoviesArea MoviesArea = new MoviesArea();

        public DetailsPersonModel(ILogger<DetailsPersonModel> logger, IPersonRepository personRepository)
        {
            _logger = logger;
            _personRepository = personRepository;
        }

        public async Task OnGetAsync()
        {
            model = await _personRepository.GetDetailsPersonDTO(PersonId);

            moviesByPerson = model.PersonMovies;
            nameOfPerson = model.Person.Name;
            LastAdded = model.LastAdded;

            PersonMovies = new MoviesArea() { Movies = moviesByPerson, Title = nameOfPerson, Subtitle = "Фільми за участю", ShowCategoryName = true, WatchAll = false, ShowCharacter = true };
            MoviesArea = new MoviesArea() { Movies = LastAdded, Title = "Останні додані", Subtitle = "Не проґавте", CenterHeader = true, CarouselClass = "bottom-carousel" };
        }
    }
}
