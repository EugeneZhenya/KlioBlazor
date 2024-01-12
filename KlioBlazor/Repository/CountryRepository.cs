using KlioBlazor.Helpers;
using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;
using Microsoft.AspNetCore.Components;

namespace KlioBlazor.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private NavigationManager navigationManager { get; set; }
        private readonly IHttpService httpService;
        private string url = "api/countries";

        public CountryRepository(IHttpService httpService, NavigationManager navigationManager)
        {
            this.httpService = httpService;
            this.navigationManager = navigationManager;
            this.url = this.navigationManager.BaseUri + this.url;
        }

        public async Task<List<Country>> GetAllCountries()
        {
            return await httpService.GetHelper<List<Country>>($"{url}/all");
        }

        public async Task<IndexCountriesDTO> GetIndexCountriesDTO()
        {
            return await httpService.GetHelper<IndexCountriesDTO>(url);
        }

        public async Task<Country> GetCountry(int Id)
        {
            return await httpService.GetHelper<Country>($"{url}/{Id}");
        }

        public async Task CreateCountry(Country country)
        {
            var response = await httpService.Post(url, country);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }

        public async Task UpdateCountry(Country country)
        {
            var response = await httpService.Put(url, country);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }
    }
}
