using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;

namespace KlioBlazor.Repository
{
    public interface ICountryRepository
    {
        Task CreateCountry(Country country);
        Task<List<Country>> GetAllCountries();
        Task<Country> GetCountry(int Id);
        Task<IndexCountriesDTO> GetIndexCountriesDTO();
        Task UpdateCountry(Country country);
    }
}
