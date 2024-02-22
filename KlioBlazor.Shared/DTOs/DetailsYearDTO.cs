using KlioBlazor.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlioBlazor.Shared.DTOs
{
    public class DetailsYearDTO
    {
        public Movie LastMovie { get; set; }
        public PaginatedResponse<List<Movie>> YearMovies { get; set; }
        public List<Movie> LastAdded { get; set; }
    }
}
