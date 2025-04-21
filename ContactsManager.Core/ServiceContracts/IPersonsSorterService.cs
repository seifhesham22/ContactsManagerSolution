using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
namespace ServiceContracts
{
    public interface IPersonsSorterService
    {
        /// <summary>
        /// return sorted list of persons
        /// </summary>
        /// <param name="allPersons">list of persons to sort</param>
        /// <param name="sortBy">Name of the property (key) based on which the persons should be sorted</param>
        /// <param name="sortOrder">ASC or DESC</param>
        /// <returns>Returns the list of persons after sorting</returns>
        Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder);

    }
}
