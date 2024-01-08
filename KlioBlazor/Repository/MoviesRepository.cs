using KlioBlazor.Helpers;
using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;
using Microsoft.AspNetCore.Components;

namespace KlioBlazor.Repository
{
    public class MoviesRepository : IMoviesRepository
    {
        private NavigationManager navigationManager { get; set; }
        private readonly IHttpService httpService;
        private string url = "api/movies";

        public MoviesRepository(IHttpService httpService, NavigationManager navigationManager)
        {
            this.httpService = httpService;
            this.navigationManager = navigationManager;
            this.url = this.navigationManager.BaseUri + this.url;
        }

        public async Task<HomePageDTO> GetHomePageDTO()
        {
            return await Get<HomePageDTO>(url);
        }

        public async Task<DetailsMovieDTO> GetDetailsMovieDTO(int id)
        {
            return await Get<DetailsMovieDTO>($"{url}/{id}");
        }

        private async Task<T> Get<T>(string url)
        {
            var response = await httpService.Get<T>(url);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }

        public async Task<int> CreateMovie(Movie movie)
        {
            var response = await httpService.Post<Movie, int>(url, movie);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }

            return response.Response;
        }
    }
}
