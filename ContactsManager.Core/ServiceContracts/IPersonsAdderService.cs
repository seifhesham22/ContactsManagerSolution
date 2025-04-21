using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
namespace ServiceContracts
{
    public interface IPersonsAdderService
    {
        /// <summary>
        /// Adds a new person to the list of persons
        /// </summary>
        /// <param name="personAddRequest">person to add</param>
        /// <returns>Returns the same person details along with newly generated person id</returns>
        Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest);
    }
}
