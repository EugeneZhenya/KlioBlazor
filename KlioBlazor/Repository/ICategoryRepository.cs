using KlioBlazor.Shared.Entities;

namespace KlioBlazor.Repository
{
    public interface ICategoryRepository
    {
        Task CreateCategory(Category category);
        Task<List<Category>> GetCategories();
    }
}
