using KlioBlazor.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlioBlazor.Shared.DTOs
{
    public class HomePageDTO
    {
        public Movie LastMovie { get; set; }
        public List<Movie> MoviesPopular { get; set; }
    }
}
