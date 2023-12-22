using KlioBlazor.Shared.Entities;

namespace KlioBlazor.Helpers
{
    public class RepositoryInMemory : IRepository
    {
        public List<Movie> GetMovies()
        {
            return new List<Movie>()
            {
                new Movie() {Title = "Тарас Шевченко", ReleaseDate = new DateTime(2015, 3, 08), Poster = "content/movies/1/cover.jpg"},
                new Movie() {Title = "Емма Андієвська: Зустріч у Львові 13 вересня", ReleaseDate = new DateTime(2013, 9, 13), Poster = "content/movies/2/cover.jpg"},
                new Movie() {Title = "Battles and Brotherhood", ReleaseDate = new DateTime(2009, 8, 31), Poster = "content/movies/3/cover.jpg"}
            };
        }
    }
}
