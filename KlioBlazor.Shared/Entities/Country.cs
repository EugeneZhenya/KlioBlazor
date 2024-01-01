using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlioBlazor.Shared.Entities
{
    public class Country
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле \"Назва\" не може бути порожнім.")]
        public string Name { get; set; }
        public string? Description { get; set; }
        [NotMapped]
        public string Flag { get; set; }
        [NotMapped]
        public string Emblem { get; set; }
        public List<MoviesCountries> MoviesCountries { get; set; } = new List<MoviesCountries>();
    }
}
