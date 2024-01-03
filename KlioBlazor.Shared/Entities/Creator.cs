using KlioBlazor.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace KlioBlazor.Shared.Entities
{
    public class Creator
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле \"Назва\" не може бути порожнім.")]
        public string Title { get; set; }
        public string? Description { get; set; }
        [NotMapped]
        public string? Logo { get; set; }
        public string LogoUrl
        {
            get
            {
                return Id + "-" + StringUtilities.Translit(Title) + ".png"; ;
            }
        }
        public string? Location { get; set; }
        public string? HomeUrl { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Поле \"Країна\" не може бути порожнім.")]
        public int CountryId { get; set; }
        public Country? Country { get; set; }
        public List<MoviesCreators> MoviesCreators { get; set; } = new List<MoviesCreators>();
    }
}
