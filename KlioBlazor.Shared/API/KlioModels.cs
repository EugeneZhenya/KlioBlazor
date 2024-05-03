using KlioBlazor.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlioBlazor.Shared.API
{
    public class VideoSearchResult
    {
        public int NextPageNumber { get; set; }
        public PageInfo PageInfo { get; set; }
        public List<Movie> Items { get; set; }
    }

    public class PageInfo
    {
        public int TotalResults { get; set; }

        public int ResultsPerPage { get; set; }
    }
}
