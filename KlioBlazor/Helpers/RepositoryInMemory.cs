using KlioBlazor.Shared.Entities;

namespace KlioBlazor.Helpers
{
    public class RepositoryInMemory : IRepository
    {
        public List<Movie> GetMovies()
        {
            return new List<Movie>()
            {
                new Movie() {Title = "Spider-Man: Far From Home", ReleaseDate = new DateTime(2023, 12, 11)},
                new Movie() {Title = "Moana", ReleaseDate = new DateTime(2023, 12, 23)},
                new Movie() {Title = "Inception", ReleaseDate = new DateTime(2013, 7, 16)}
            };
        }
    }
}
