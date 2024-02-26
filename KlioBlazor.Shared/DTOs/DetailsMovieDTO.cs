using KlioBlazor.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlioBlazor.Shared.DTOs
{
    public class DetailsMovieDTO
    {
        public Movie Movie { get; set; }
        public List<Genre> Genres { get; set; }
        public List<Country> Countries { get; set; }
        public List<Person> Staff { get; set; }
        public List<Person> Actors { get; set; }
        public List<Person> TranslateStaff { get; set; }
        public List<Person> TranslateActors { get; set; }
        public List<Creator> Creators { get; set; }
        public List<Keyword> Keywords { get; set; }
        public List<MovieInfo> MovieInfos { get; set; }
        public List<Partition> Partitions { get; set; }
        public List<Movie> OtherMovies { get; set; }
    }
}
