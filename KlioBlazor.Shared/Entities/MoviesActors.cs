using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlioBlazor.Shared.Entities
{
    public class MoviesActors
    {
        public int PersonId { get; set; }
        public int MovieId { get; set; }
        public Person? Person { get; set; }
        public Movie? Movie { get; set; }
        public string Character { get; set; }
        public int Order { get; set; }
        public bool IsActor { get; set; }
        public bool IsTranslator { get; set; }
    }
}
