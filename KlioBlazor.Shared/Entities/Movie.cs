using System.ComponentModel.DataAnnotations;

namespace KlioBlazor.Shared.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле \"Назва\" не може бути порожнім.")]
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Poster { get; set; }
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
        public List<MoviesGenres> MoviesGenres { get; set; } = new List<MoviesGenres>();
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
                    return Title.Substring(0, 60) + "...";
                }
                else
                {
                    return Title;
                }
            }
        }
    }
}
