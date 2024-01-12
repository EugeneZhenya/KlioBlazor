using KlioBlazor.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlioBlazor.Shared.DTOs
{
    public class MovieUpdateDTO
    {
        public Movie Movie { get; set; }
        public List<Person> Actors { get; set; }
        public List<Genre> SelectedGenres { get; set; }
        public List<Genre> NotSelectedGenres { get; set; }
        public List<Partition> AllPartitions { get; set; }
        public List<Category> AllCategories { get; set; }
        public List<MovieInfo> MovieInfos { get; set; }
        public List<Country> SelectedCountries { get; set; }
        public List<Country> NotSelectedCountries { get; set; }
        public List<Creator> Creators { get; set; }
        public List<Keyword> Keywords { get; set; }
    }
}
