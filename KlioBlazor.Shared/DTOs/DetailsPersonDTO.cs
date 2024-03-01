using KlioBlazor.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlioBlazor.Shared.DTOs
{
    public class DetailsPersonDTO
    {
        public Person Person { get; set; }
        public Movie LastMovie { get; set; }
        public List<Movie> PersonMovies { get; set; }
        public List<Movie> LastAdded { get; set; }
    }
}
