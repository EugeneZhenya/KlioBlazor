using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;

namespace KlioWeb.Repository
{
    public interface ICountryRepository
    {
        Task<Country> GetCountry(int Id);
        Task<DetailsCountryDTO> GetDetailsCountryDTO(FilterMoviesDTO filterMoviesDTO);
        Task<IndexCountriesDTO> GetIndexCountriesDTO();
    }
}
