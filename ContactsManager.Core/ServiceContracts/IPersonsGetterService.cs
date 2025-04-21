using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
namespace ServiceContracts
{
    public interface IPersonsGetterService
    {
        /// <summary>
        /// returns all of persons
        /// </summary>
        /// <returns>returns list of PersonResponce Object</returns>
        Task<List<PersonResponse>> GetAllPersons();
        Task<PersonResponse> GetPersonById(Guid? id);
        /// <summary>
        /// returns a list of persons object that match with the search filed and the search string.
        /// </summary>
        /// <param name="searchBy">search field to search in</param>
        /// <param name="searchString">search string to search with</param>
        /// <returns>all matching person based on the search field and the search string</returns>
        Task<List<PersonResponse>> GetFilteredPersons(string? searchBy, string? searchString);
        /// <summary>
        /// Returns Persons As CSV
        /// </summary>
        /// <returns>Returns The Memort Stream With CSV Data</returns>
        Task<MemoryStream> GetPersonsCSV();
        /// <summary>
        /// Returns Persons As EXCEL
        /// </summary>
        /// <returns>Returns MemoryStream With EXCEL data</returns>
        Task<MemoryStream>GetPersonsExcel();
    }
}
