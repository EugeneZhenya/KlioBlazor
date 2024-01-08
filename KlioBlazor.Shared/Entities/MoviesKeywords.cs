using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlioBlazor.Shared.Entities
{
    public class MoviesKeywords
    {
        public int MovieId { get; set; }
        public int KeywordId { get; set; }
        public Movie? Movie { get; set; }
        public Keyword? Keyword { get; set; }
        public int Order { get; set; }
    }
}
