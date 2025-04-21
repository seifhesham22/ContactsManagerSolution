using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
namespace ServiceContracts
{
    public interface IPersonsUpdaterService
    {

        /// <summary>
        /// updates a specific person info based on person id
        /// </summary>
        /// <param name="personUpdateRequest">person details to u pdate including person id</param>
        /// <returns>returns person object after updation</returns>
        Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest);
    }
}
