using KlioBlazor.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlioBlazor.Shared.DTOs
{
    public class DetailsKeywordDTO
    {
        public Keyword Keyword { get; set; }
        public Movie LastMovie { get; set; }
        public List<Movie> KeywordMovies { get; set; }
        public PaginatedResponse<List<Movie>> KeywordMoviesPage { get; set; }
        public List<Movie> LastAdded { get; set; }
    }
}
