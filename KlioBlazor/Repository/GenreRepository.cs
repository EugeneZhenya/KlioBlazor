using KlioBlazor.Helpers;
using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;
using Microsoft.AspNetCore.Components;

namespace KlioBlazor.Repository
{
    public class GenreRepository : IGenreRepository
    {
        private NavigationManager navigationManager { get; set; }
        private readonly IHttpService httpService;
        private string url = "api/genres";

        public GenreRepository(IHttpService httpService, NavigationManager navigationManager)
        {
            this.httpService = httpService;
            this.navigationManager = navigationManager;
            this.url = this.navigationManager.BaseUri + this.url;
        }

        public async Task<List<Genre>> GetAllGenres()
        {
            return await httpService.GetHelper<List<Genre>>($"{url}/all");
        }

        public async Task<IndexGenresDTO> GetIndexGenresDTO()
        {
            return await httpService.GetHelper<IndexGenresDTO>(url);
        }

        public async Task<Genre> GetGenre(int Id)
        {
            return await httpService.GetHelper<Genre>($"{url}/{Id}");
        }

        public async Task<DetailsGenreDTO> GetDetailsGenreDTO(int Id)
        {
            return await httpService.GetHelper<DetailsGenreDTO>($"{url}/details/{Id}");
        }

        public async Task CreateGenre(Genre genre)
        {
            var response = await httpService.Post(url, genre);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }

        public async Task UpdateGenre(Genre genre)
        {
            var response = await httpService.Put(url, genre);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }

        public async Task DeleteGenre(int Id)
        {
            var response = await httpService.Delete($"{url}/{Id}");
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }
    }
}
