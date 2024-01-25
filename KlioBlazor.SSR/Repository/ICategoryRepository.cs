using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;

namespace KlioBlazor.SSR.Repository
{
    public interface ICategoryRepository
    {
        Task CreateCategory(Category category);
        Task DeleteCategory(int Id);
        Task<List<Category>> GetAllCategories();
        Task<Category> GetCategory(int Id);
        Task<IndexCategoriesDTO> GetCategoryByName(string name);
        Task UpdateCategory(Category category);
    }
}
