using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace Repositories
{
    public class CountriesRepository : ICountriesRepository
    {
        private readonly ApplicationDbContext _db;

        public CountriesRepository(ApplicationDbContext dbContext)
        {
           _db = dbContext;
        }


        public async Task<Country> AddCountry(Country country)
        {
            _db.Countries.Add(country);
            await _db.SaveChangesAsync();
            return country;
        }

        public async Task<List<Country>> GetAllCountries()
        {
           return await _db.Countries.ToListAsync();
        }

        public async Task<Country?> GetCountryById(Guid id)
        {
            return await _db.Countries.FirstOrDefaultAsync(x => x.CountryId == id);
        }

        public async Task<Country?> GetCountryByName(string name)
        {
            return await _db.Countries.FirstOrDefaultAsync(x => x.CountryName == name);
        }
    }
}
