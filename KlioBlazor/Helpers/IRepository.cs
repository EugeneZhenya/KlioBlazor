using KlioBlazor.Shared.Entities;

namespace KlioBlazor.Helpers
{
    public interface IRepository
    {
        List<Movie> GetMovies();
    }
}
