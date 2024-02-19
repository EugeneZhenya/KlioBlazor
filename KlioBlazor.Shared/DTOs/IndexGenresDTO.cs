using KlioBlazor.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlioBlazor.Shared.DTOs
{
    public class IndexGenresDTO
    {
        public Movie LastMovie { get; set; }
        public List<Country> LastMovieCountries { get; set; }
        public List<Genre> AllGenres { get; set; }
        public List<Movie> LastAdded { get; set; }
    }
}
