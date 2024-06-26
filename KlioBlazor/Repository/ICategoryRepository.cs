﻿using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;

namespace KlioBlazor.Repository
{
    public interface ICategoryRepository
    {
        Task CreateCategory(Category category);
        Task<List<Category>> GetAllCategories();
        Task<Category> GetCategory(int Id);
        Task UpdateCategory(Category category);
        Task DeleteCategory(int Id);
        Task<IndexCategoriesDTO> GetCategoryByName(string name);
    }
}
