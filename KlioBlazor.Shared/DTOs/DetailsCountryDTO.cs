using KlioBlazor.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlioBlazor.Shared.DTOs
{
    public class DetailsCountryDTO
    {
        public Country Country { get; set; }
        public List<Movie> CountryMovies { get; set; }
    }
}
