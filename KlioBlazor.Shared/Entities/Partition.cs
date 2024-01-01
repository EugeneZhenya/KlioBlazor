using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlioBlazor.Shared.Entities
{
    public class Partition
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле \"Назва\" не може бути порожнім.")]
        public string Name { get; set; }
        public string? Description { get; set; }
        [NotMapped]
        public string Picture { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public List<Movie> Movies { get; set; } = new List<Movie>();
    }
}
