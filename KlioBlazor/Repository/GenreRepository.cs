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
            return await Get<List<Genre>>($"{url}/all");
        }

        public async Task<IndexGenresDTO> GetIndexGenresDTO()
        {
            return await Get<IndexGenresDTO>(url);
        }

        public async Task<Genre> GetGenre(int Id)
        {
            return await Get<Genre>($"{url}/{Id}");
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
    }
}
