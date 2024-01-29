using KlioBlazor.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlioBlazor.Shared.DTOs
{
    public class DetailsCreatorDTO
    {
        public Creator Creator { get; set; }
        public Movie LastMovie { get; set; }
        public List<Movie> CreatorMovies { get; set; }
    }
}
