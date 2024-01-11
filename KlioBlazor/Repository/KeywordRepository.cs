using KlioBlazor.Helpers;
using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;
using Microsoft.AspNetCore.Components;

namespace KlioBlazor.Repository
{
    public class KeywordRepository : IKeywordRepository
    {
        private NavigationManager navigationManager { get; set; }
        private readonly IHttpService httpService;
        private string url = "api/keywords";

        public KeywordRepository(IHttpService httpService, NavigationManager navigationManager)
        {
            this.httpService = httpService;
            this.navigationManager = navigationManager;
            this.url = this.navigationManager.BaseUri + this.url;
        }

        public async Task<List<Keyword>> GetAllKeywords()
        {
            return await Get<List<Keyword>>($"{url}/all");
        }

        public async Task<List<Keyword>> GetKeywordByWord(string word)
        {
            return await Get<List<Keyword>>($"{url}/search/{word}");
        }

        public async Task<IndexKeywordsDTO> GetIndexKeywordsDTO()
        {
            return await Get<IndexKeywordsDTO>(url);
        }

        public async Task<Keyword> GetKeyword(int Id)
        {
            return await Get<Keyword>($"{url}/{Id}");
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

        public async Task CreateKeyword(Keyword keyword)
        {
            var response = await httpService.Post(url, keyword);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }

        public async Task UpdateKeyword(Keyword keyword)
        {
            var response = await httpService.Put(url, keyword);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }
    }
}
