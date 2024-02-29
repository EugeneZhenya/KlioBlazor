using KlioBlazor.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlioBlazor.Shared.DTOs
{
    public class IndexKeywordsDTO
    {
        public Movie LastMovie { get; set; }
        public List<Country> LastMovieCountries { get; set; }
        public List<Keyword> AllKeywords { get; set; }
        public List<Movie> LastAdded { get; set; }
    }
}
