using KlioBlazor.Shared.Entities;
using KlioBlazor.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlioBlazor.Shared.DTOs
{
    public class IndexAgeLimitsDTO
    {
        public Movie LastMovie { get; set; }
        public List<Country> LastMovieCountries { get; set; }
        public List<AgeCategory> AllAges { get; set; }
        public List<int> AgeCounters { get; set; }
        public List<Movie> LastAdded { get; set; }
    }
}
