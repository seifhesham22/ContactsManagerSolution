using Entities;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
namespace Services
{
    public class CountriesServies : ICountriesService
    {
        private readonly ICountriesRepository _countriesRepository;
        public CountriesServies(ICountriesRepository countriesRepository)
        {
            _countriesRepository = countriesRepository;
        }
        public async Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest)
        {
            if (countryAddRequest == null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest));
            }
            if(countryAddRequest.CountryName == null)
            {
                throw new ArgumentException(nameof(countryAddRequest.CountryName));
            }
            if (await _countriesRepository.GetCountryByName(countryAddRequest.CountryName) != null);
            {
                throw new ArgumentException("Country name already exists");
            }
            Country country = countryAddRequest.ToCountry();
            country.CountryId = Guid.NewGuid(); 
            await _countriesRepository.AddCountry(country);
            return country.ToCountryResponse();

        }

        public async Task<CountryResponse?> GetCountryByCountryId(Guid? countryId)
        {
            if (countryId == null)
            {
               return null;
            }
            Country? country_from_list = await _countriesRepository.GetCountryById(countryId.Value);
            if (country_from_list == null) { return null; }
            return country_from_list.ToCountryResponse();
        }

        public async Task<List<CountryResponse>> GetCountryList()
        {
            return (await _countriesRepository.GetAllCountries()).Select(country => country.ToCountryResponse()).ToList();

        }

        public async Task<int> UploadCountriesFromExcelFile(IFormFile formFile)
        {
            MemoryStream stream = new MemoryStream();
            await formFile.CopyToAsync(stream);
            int countriesInserted = 0;
            using(ExcelPackage package = new ExcelPackage(stream))
            {
                ExcelWorksheet excelWorksheet =  package.Workbook.Worksheets["Countries"];
                int rowCount = excelWorksheet.Dimension.Rows;
                for(int i = 2; i <= rowCount; i++)
                {
                    string? cellValue = Convert.ToString(excelWorksheet.Cells[i, 1].Value);
                    if(!string.IsNullOrEmpty(cellValue))
                    {
                        string? countryName = cellValue;
                        if(await _countriesRepository.GetCountryByName(countryName) == null)
                        {
                            Country country = new Country() { CountryName = countryName};
                            await _countriesRepository.AddCountry(country);
                            
                            countriesInserted++;
                        }
                    }
                }
                return countriesInserted;
            }
        }
    }
}
