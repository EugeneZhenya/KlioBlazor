using KlioBlazor.Shared.Entities;

namespace KlioBlazor.Repository
{
    public interface IGenreRepository
    {
        Task CreateGenre(Genre genre);
    }
}
