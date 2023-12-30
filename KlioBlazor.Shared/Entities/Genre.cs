using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlioBlazor.Shared.Entities
{
    public class Genre
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле \"Назва\" не може бути порожнім.")]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public List<MoviesGenres> MoviesGenres { get; set; } = new List<MoviesGenres>();
    }
}
