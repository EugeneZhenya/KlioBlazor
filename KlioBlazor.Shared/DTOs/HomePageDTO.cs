﻿using KlioBlazor.Shared.Entities;
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
        public List<Country> LastMovieCountries { get; set; }
        public List<Movie> MoviesPopular { get; set; }
        public List<Partition> PartitionsPopular { get; set; }
        public Movie RecomendMovie { get; set; }
        public List<Country> RecomendMovieCountries { get; set; }
        public List<Movie> LastAdded { get; set; }
        public List<Movie> TodaysFilms { get; set; }
        public List<Person> Jubilees { get; set; }
        public List<Person> Memorials { get; set; }
    }
}
