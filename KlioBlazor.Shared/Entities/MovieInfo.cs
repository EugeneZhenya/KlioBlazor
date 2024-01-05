using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlioBlazor.Shared.Entities
{
    public class MovieInfo
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле \"Інформація\" не може бути порожнім.")]
        public string Info { get; set; }
        [Required(ErrorMessage = "Поле \"Текст\" не може бути порожнім.")]
        public string Text { get; set; }
        public string? Remark { get; set; }
        public int MovieId { get; set; }
        public Movie? Movie { get; set; }
    }
}
