using KlioBlazor.Shared.Entities;

namespace KlioBlazor.Repository
{
    public interface ICountryRepository
    {
        Task CreateCountry(Country country);
    }
}
