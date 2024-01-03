using KlioBlazor.Shared.Helpers;
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
        public DateTime? IndependenceDay { get; set; }
        [NotMapped]
        public string Flag { get; set; }
        [NotMapped]
        public string Emblem { get; set; }
        [NotMapped]
        public string Background { get; set; }
        public List<MoviesCountries> MoviesCountries { get; set; } = new List<MoviesCountries>();
        public string FlagUrl
        {
            get
            {
                return Id + "-Flag_" + StringUtilities.Translit(Name) + ".svg"; ;
            }
        }
        public string EmblemUrl
        {
            get
            {
                return Id + "-Emblem_" + StringUtilities.Translit(Name) + ".svg"; ;
            }
        }
        public string BackgroundUrl
        {
            get
            {
                return Id + "-Background_" + StringUtilities.Translit(Name) + ".jpg"; ;
            }
        }
    }
}
