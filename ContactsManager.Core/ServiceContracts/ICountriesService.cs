using Microsoft.AspNetCore.Http;
using ServiceContracts.DTO;
namespace ServiceContracts
{
    /// <summary>
    /// Business logic for country entity
    /// </summary>
    public interface ICountriesService
    {
        /// <summary>
        /// adds a country to list of countries 
        /// </summary>
        /// <param name="countryAddRequest">Country Object to add</param>
        /// <returns>Returns the same country object as country responce</returns>
        public Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest);
        public Task<List<CountryResponse>> GetCountryList();
        /// <summary>
        /// Returns A Country object based on the Country Id
        /// </summary>
        /// <param name="countryId">Country Guid(Which I wanna search for)</param>
        /// <returns>Matching Country Object</returns>
        public Task<CountryResponse> GetCountryByCountryId(Guid? countryId);
        /// <summary>
        /// Uploades Countries From excel file to data base
        /// </summary>
        /// <param name="formFile">Excel File With The Countries To Inscert</param>
        /// <returns>returns the number of countries added</returns>
        public Task<int> UploadCountriesFromExcelFile(IFormFile formFile);
        
    }
}
