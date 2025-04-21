using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
namespace ServiceContracts
{
    public interface IPersonsDeleterService
    {

        /// <summary>
        /// Deletes a person based on person Id
        /// </summary>
        /// <param name="id">represents the person Id which should delete</param>
        /// <returns>returns true, if deletition is done successfully and false , if not</returns>
        Task<bool> DeletePerson(Guid? id);
    }
}
