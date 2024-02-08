using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;

namespace KlioWeb.Repository
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllCategories();
        Task<Category> GetCategory(int Id);
        Task<IndexCategoriesDTO> GetCategoryByName(string name);
    }
}
