using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlioBlazor.Shared.Entities
{
    public class Keyword
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле \"Слово\" не може бути порожнім.")]
        public string Word { get; set; }
        public string? Comment { get; set; }
        public string? Equivalent { get; set; }
        public List<MoviesKeywords> MoviesKeywords { get; set; } = new List<MoviesKeywords>();
    }
}
