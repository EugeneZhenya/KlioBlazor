namespace KlioBlazor.Shared.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Poster { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime ReleaseDateExact { get; set; }
        public DateTime PublicDate { get; set; }
        public bool HasTrailer { get; set; }
        public int AgeLimit { get; set; }
        public int Duration { get; set; }
        public int ViewCounter { get; set; }
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
