﻿using KlioBlazor.Helpers;
using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;
using Microsoft.AspNetCore.Components;

namespace KlioBlazor.Repository
{
    public class CreatorRepository : ICreatorRepository
    {
        private NavigationManager navigationManager { get; set; }
        private readonly IHttpService httpService;
        private string url = "api/creators";

        public CreatorRepository(IHttpService httpService, NavigationManager navigationManager)
        {
            this.httpService = httpService;
            this.navigationManager = navigationManager;
            this.url = this.navigationManager.BaseUri + this.url;
        }

        public async Task<List<Creator>> GetAllCreators()
        {
            return await httpService.GetHelper<List<Creator>>($"{url}/all");
        }

        public async Task<IndexCreatorsDTO> GetIndexCreatorsDTO()
        {
            return await httpService.GetHelper<IndexCreatorsDTO>(url);
        }

        public async Task<Creator> GetCreator(int Id)
        {
            return await httpService.GetHelper<Creator>($"{url}/{Id}");
        }

        public async Task<List<Creator>> GetCreatorByTitle(string name)
        {
            return await httpService.GetHelper<List<Creator>>($"{url}/search/{name}");
        }

        public async Task<DetailsCreatorDTO> GetDetailsCreatorDTO(int Id)
        {
            return await httpService.GetHelper<DetailsCreatorDTO>($"{url}/details/{Id}");
        }

        public async Task CreateCreator(Creator creator)
        {
            var response = await httpService.Post(url, creator);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }

        public async Task UpdateCreator(Creator creator)
        {
            var response = await httpService.Put(url, creator);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }

        public async Task DeleteCreator(int Id)
        {
            var response = await httpService.Delete($"{url}/{Id}");
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }
    }
}
