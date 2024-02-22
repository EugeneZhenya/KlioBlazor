using KlioBlazor.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlioBlazor.Shared.DTOs
{
    public class DetailsAgeLimitsDTO
    {
        public Movie LastMovie { get; set; }
        public PaginatedResponse<List<Movie>> AgesMovies { get; set; }
        public List<Movie> LastAdded { get; set; }
}
}
