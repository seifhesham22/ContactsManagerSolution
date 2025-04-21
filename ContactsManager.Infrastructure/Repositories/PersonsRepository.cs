using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class PersonsRepository : IPersonsRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<PersonsRepository> _logger;
        public PersonsRepository(ApplicationDbContext db , ILogger<PersonsRepository> logger) { _db = db; _logger = logger; }

        public async Task<Person> AddPerson(Person person)
        {
            _db.Persons.Add(person);
            await _db.SaveChangesAsync();
            return person;
        }

        public async Task<bool> DeletePersonByPersonId(Guid id)
        {
            _db.Persons.RemoveRange(_db.Persons.Where(x=>x.Id == id));
            int rows = await _db.SaveChangesAsync();
            return rows > 0;
        }

        public async Task<List<Person>> GetAllPersons()
        {
            _logger.LogInformation("getAllPersons PersonsRepository");
            return await _db.Persons.Include("Country").ToListAsync();
        }

        public async Task<List<Person>> GetFilteredPersons(Expression<Func<Person, bool>> predicate)
        {
            _logger.LogInformation("getFilteredPersons PersonReposotry");
            return  await _db.Persons.Include("Country").Where(predicate).ToListAsync();
        }

        public async Task<Person?> GetPersonById(Guid id)
        {
            return await _db.Persons.Include("Country").FirstOrDefaultAsync(x=>x.Id == id);
        }

        public async Task<Person> UpdatePerson(Person person)
        {
            Person? matchingPerson = await _db.Persons.FirstOrDefaultAsync(x => x.Id == person.Id);
            if (matchingPerson == null)
            {
                return person;
            }
            matchingPerson.Name = person.Name;
            matchingPerson.Email = person.Email;
            matchingPerson.Address = person.Address;
            matchingPerson.Gender = person.Gender;
            matchingPerson.DateOfBirth = person.DateOfBirth;
            matchingPerson.CountryId = person.CountryId;
            matchingPerson.RecieveNewsLetter = person.RecieveNewsLetter;
            await _db.SaveChangesAsync();
            
            return matchingPerson;
        }
    }
}
