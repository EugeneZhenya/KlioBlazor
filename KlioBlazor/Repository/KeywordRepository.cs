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
            return await httpService.GetHelper<List<Keyword>>($"{url}/all");
        }

        public async Task<List<Keyword>> GetKeywordByWord(string word)
        {
            return await httpService.GetHelper<List<Keyword>>($"{url}/search/{word}");
        }

        public async Task<IndexKeywordsDTO> GetIndexKeywordsDTO()
        {
            return await httpService.GetHelper<IndexKeywordsDTO>(url);
        }

        public async Task<Keyword> GetKeyword(int Id)
        {
            return await httpService.GetHelper<Keyword>($"{url}/{Id}");
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
