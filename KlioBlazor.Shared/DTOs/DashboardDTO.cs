using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlioBlazor.Shared.DTOs
{
    public class DashboardDTO
    {
        public int CategoriesCount { get; set; }
        public int PartitionsCount { get; set; }
        public int KeywordsCount { get; set; }
        public int GenresCount { get; set; }
        public int CountriesCount { get; set; }
        public int CreatorsCount { get; set; }
        public int PeopleCount { get; set; }
        public int MoviesCount { get; set; }
    }
}
