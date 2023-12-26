using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlioBlazor.Shared.Entities
{
    public class Person
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле \"Ім'я\" не може бути порожнім.")]
        public string Name { get; set; }
        public string Biography { get; set; }
        public string? Picture { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool DateOfBirthExact { get; set; }
        public DateTime? DateOfDeath { get; set; }
        public bool DateOfDeathExact { get; set; }
        public bool IsFemale { get; set; }
    }
}
