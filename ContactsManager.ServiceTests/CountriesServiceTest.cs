using System;
using System.Collections.Generic;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;

namespace CRUDTests
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;

        public CountriesServiceTest()
        {
            //var dbContextOptions = new PersonsDbContext(new DbContextOptionsBuilder<PersonsDbContext>().Options);
            //_countriesService = new CountriesServies(dbContextOptions);
            _countriesService = new CountriesServies(null);
        }
        #region add country
        [Fact]
        public async Task AddCountry_NullCountry()
        {
            CountryAddRequest? request = null;
            await Assert.ThrowsAsync<ArgumentNullException>(async() => await _countriesService.AddCountry(request));

        }
        [Fact]
        public async Task AddCountry_CountryNameIsNull()
        {
            CountryAddRequest request = new CountryAddRequest() { CountryName = null };
            await Assert.ThrowsAsync<ArgumentException>(async() => await _countriesService.AddCountry(request));

        }
        [Fact]
        public async Task AddCountry_DuplicateCountryName()
        {
            CountryAddRequest request1 = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest request2 = new CountryAddRequest() { CountryName = "USA" };
            await Assert.ThrowsAsync<ArgumentException>(async() =>
            {
                await _countriesService.AddCountry(request1);
                await _countriesService.AddCountry(request2);
            });
        }
        [Fact]
        public async Task AddCountry_ProperCountryRrequest()
        {
            CountryAddRequest request = new CountryAddRequest() { CountryName = "japan" };
            CountryResponse response = await _countriesService.AddCountry(request);
            List<CountryResponse> contries_From_GetAllCountries = await _countriesService.GetCountryList();
       
            Assert.True(response.Id != Guid.Empty);
            Assert.Contains(response, contries_From_GetAllCountries);

        }
        #endregion

        #region Get all countries
        [Fact]
        public async Task GetAllCountries_EmptyList()
        {
            List <CountryResponse> actual_countries_responce_list = await _countriesService.GetCountryList();
            Assert.Empty(actual_countries_responce_list);

        }
        [Fact]
        public async Task GetAllCountries_AddFewCountries()
        {
            List<CountryResponse> countries_list_from_add_country = new List<CountryResponse>();
            List<CountryAddRequest> country_request_list = new List<CountryAddRequest>()
            {
                new CountryAddRequest() { CountryName = "USA"},
                new CountryAddRequest() {CountryName ="UK" }
            }; 
            foreach(CountryAddRequest country_request in country_request_list)
            {
                countries_list_from_add_country.Add(await _countriesService.AddCountry(country_request));
            }
            List<CountryResponse> actualCountryResponceList = await _countriesService.GetCountryList();
            foreach(CountryResponse expected in countries_list_from_add_country)
            {
                Assert.Contains(expected , actualCountryResponceList);
            }

        }
        #endregion

        #region Get Country By Country Id
        [Fact]
        //if we supply null it should return null.
        public async Task GetCountryByCountryId_NullCountryId()
        {
            Guid? CountryId = null;
            CountryResponse? country_responce_from_get_method = await _countriesService.GetCountryByCountryId(CountryId);
            Assert.Null(country_responce_from_get_method);
        }
        [Fact]
        public async Task GetCountryByCountryId_ValidCountryId()
        {
            CountryAddRequest countryAddRequest = new CountryAddRequest() { CountryName = "China"};
            CountryResponse Country_responce_from_add = await _countriesService.AddCountry(countryAddRequest);
            CountryResponse? country_responce_from_get = await _countriesService.GetCountryByCountryId(Country_responce_from_add.Id);
            Assert.Equal(Country_responce_from_add,country_responce_from_get);


        }

        #endregion
    }
}
