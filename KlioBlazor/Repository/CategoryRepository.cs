using KlioBlazor.Helpers;
using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;
using Microsoft.AspNetCore.Components;
using System;

namespace KlioBlazor.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private NavigationManager navigationManager { get; set; }
        private readonly IHttpService httpService;
        private string url = "api/categories";

        public CategoryRepository(IHttpService httpService, NavigationManager navigationManager)
        {
            this.httpService = httpService;
            this.navigationManager = navigationManager;
            this.url = this.navigationManager.BaseUri + this.url;
        }

        public async Task<List<Category>> GetAllCategories()
        {
            return await httpService.GetHelper<List<Category>>($"{url}/all");
        }

        public async Task<IndexCategoriesDTO> GetIndexCategoriesDTO()
        {
            return await httpService.GetHelper<IndexCategoriesDTO>(url);
        }

        public async Task<Category> GetCategory(int Id)
        {
            return await httpService.GetHelper<Category>($"{url}/{Id}");
        }

        public async Task CreateCategory(Category category)
        {
            var response = await httpService.Post(url, category);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }

        public async Task UpdateCategory(Category category)
        {
            var response = await httpService.Put(url, category);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }
    }
}
