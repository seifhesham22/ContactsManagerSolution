using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryContracts
{
    /// <summary>
    /// represents data access logic for person entity
    /// </summary>
    public interface IPersonsRepository
    {

        /// <summary>
        /// Adds a new person to the database
        /// </summary>
        /// <param name="person">person to add</param>
        /// <returns>Returns the same person afterr adding it to the database</returns>
        Task<Person> AddPerson(Person person);
        /// <summary>
        /// returns all of persons
        /// </summary>
        /// <returns>returns list of Person Objects</returns>
        Task<List<Person>> GetAllPersons();

        /// <summary>
        /// Returns person object based on the the given person Id
        /// </summary>
        /// <param name="id">The Id to search with</param>
        /// <returns>Person object or null</returns>
        Task<Person?> GetPersonById(Guid id);

        /// <summary>
        /// returns all persons based on the given expression.
        /// </summary>
        /// <param name="predicate">LINQ expression to check</param>
        /// <returns>all matching person with given condition</returns>
        Task<List<Person>> GetFilteredPersons(Expression<Func<Person, bool>> predicate);

        /// <summary>
        /// Deletes person based on the Guid
        /// </summary>
        /// <param name="id">the person Id</param>
        /// <returns>True if deletetion was sucess otherwise false</returns>
        Task<bool> DeletePersonByPersonId(Guid id);

        /// <summary>
        /// Updates person object the name and other properties
        /// </summary>
        /// <param name="person">the person object to update</param>
        /// <returns>Returns the updated person</returns>
        Task<Person> UpdatePerson(Person person);
     
    }
}
