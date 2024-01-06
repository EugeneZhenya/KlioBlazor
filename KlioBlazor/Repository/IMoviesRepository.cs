using KlioBlazor.Shared.Entities;

namespace KlioBlazor.Repository
{
    public interface IMoviesRepository
    {
        Task<int> CreateMovie(Movie movie);
    }
}
