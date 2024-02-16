using KlioBlazor.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlioBlazor.Shared.DTOs
{
    public class DetailsPartitionDTO
    {
        public Partition Partition { get; set; }
        public Movie LastMovie { get; set; }
        public List<Movie> PartitionMovies { get; set; }
        public List<Movie> LastAdded { get; set; }
    }
}
