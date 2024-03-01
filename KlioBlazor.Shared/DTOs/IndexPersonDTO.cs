using KlioBlazor.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlioBlazor.Shared.DTOs
{
    public class IndexPersonDTO
    {
        public int Page { get; set; } = 1;
        public int RecordsPerPage { get; set; } = 14;
        public PaginationDTO Pagination
        {
            get { return new PaginationDTO() { Page = Page, RecordsPerPage = RecordsPerPage }; }
        }
        public List<Person> PeopleList { get; set; }
        public double TotalAmountPages { get; set; }
        public double TotalRecords {  get; set; }
        public List<Person> Jubilees { get; set; }
        public List<Person> Memorials { get; set; }
    }
}
