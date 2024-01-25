using KlioBlazor.Shared.Entities;

namespace KlioBlazor.SSR.Repository
{
    public interface ICountryRepository
    {
        Task CreateCountry(Country country);
        Task DeleteCountry(int Id);
        Task<List<Country>> GetAllCountries();
        Task<Country> GetCountry(int Id);
        Task UpdateCountry(Country country);
    }
}
