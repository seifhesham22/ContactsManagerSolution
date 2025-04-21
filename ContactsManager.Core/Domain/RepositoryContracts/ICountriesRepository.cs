using Entities;
namespace RepositoryContracts
{
    public interface ICountriesRepository
    {

        /// <summary>
        /// Adds a new Country to data store
        /// </summary>
        /// <param name="country">The country object to add</param>
        /// <returns>Returns the country object after addig it to the data store</returns>
        Task<Country> AddCountry(Country country);
        /// <summary>
        /// Returns All Countries In the data store
        /// </summary>
        /// <returns>All countries from the table</returns>
        Task<List<Country>> GetAllCountries();
        /// <summary>
        /// Returns a matching country object based on the id otherwise, returns null
        /// </summary>
        /// <param name="id">The Id to search</param>
        /// <returns>Matching Country Object or Null</returns>
        Task<Country?> GetCountryById(Guid id);
        /// <summary>
        /// Returns a matching country object based on the name otherwise , it returns null
        /// </summary>
        /// <param name="name">the naem to search</param>
        /// <returns>Matching country object or null</returns>
        Task<Country?> GetCountryByName(string name);
    }
}
