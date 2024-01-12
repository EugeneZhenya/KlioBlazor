using KlioBlazor.Shared.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace KlioBlazor.Shared.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле \"Назва\" не може бути порожнім.")]
        public string Title { get; set; }
        public string Summary { get; set; }
        [NotMapped]
        public string Poster { get; set; }
        [NotMapped]
        public string Background { get; set; }
        [Required(ErrorMessage = "Поле \"Дата виходу\" не може бути порожнім.")]
        public DateTime? ReleaseDate { get; set; }
        public bool ReleaseDateExact { get; set; }
        [Required]
        public DateTime PublicDate { get; set; }
        public bool HasTrailer { get; set; }
        public int AgeLimit { get; set; }
        public int Duration { get; set; }
        public int ViewCounter { get; set; }
        public int PatitionId { get; set; }
        public Partition? Partition { get; set; } = null!;
        public List<MoviesGenres> MoviesGenres { get; set; } = new List<MoviesGenres>();
        public List<MoviesActors> MoviesActors { get; set; } = new List<MoviesActors>();
        public List<MoviesCountries> MoviesCountries { get; set; } = new List<MoviesCountries>();
        public List<MoviesCreators> MoviesCreators { get; set; } = new List<MoviesCreators>();
        public List<MoviesKeywords> MoviesKeywords { get; set; } = new List<MoviesKeywords>();
        public List<MovieInfo> MovieInfos { get; set; } = new List<MovieInfo>();
        public string TitleBrief
        {
            get
            {
                if (string.IsNullOrEmpty(Title))
                {
                    return null;
                }

                if (Title.Length > 60)
                {
                    return Title.Substring(0, 60) + "…";
                }
                else
                {
                    return Title;
                }
            }
        }
    }
}
