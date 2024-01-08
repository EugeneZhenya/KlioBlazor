using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlioBlazor.Shared.Entities
{
    public class MoviesCountries
    {
        public int MovieId { get; set; }
        public int CountryId { get; set; }
        public Movie? Movie { get; set; }
        public Country? Country { get; set; }
        public int Order { get; set; }
    }
}
