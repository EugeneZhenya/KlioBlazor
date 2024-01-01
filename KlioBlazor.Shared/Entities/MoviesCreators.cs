using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlioBlazor.Shared.Entities
{
    public class MoviesCreators
    {
        public int MovieId { get; set; }
        public int CreatorID { get; set; }
        public Movie Movie { get; set; }
        public Creator Creator { get; set; }
        public int Order { get; set; }
    }
}
