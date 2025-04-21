using ContactsManager.Core.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser , ApplicationRole , Guid>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        public virtual DbSet<Person> Persons { get; set; }
        public virtual DbSet<Country> Countries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().ToTable("countries");
            modelBuilder.Entity<Person>().ToTable("persons");
            //seed countries
            string countriesJson = System.IO.File.ReadAllText("countries.json");
            List<Country>? countries = System.Text.Json.JsonSerializer.Deserialize<List<Country>>(countriesJson);
            foreach(Country country in countries)
            {
                modelBuilder.Entity<Country>().HasData(country);
            }

            string personsJson = System.IO.File.ReadAllText("persons.json");
            List<Person>? persons = System.Text.Json.JsonSerializer.Deserialize<List<Person>>(personsJson);
            foreach(Person person in persons)
            {
                modelBuilder.Entity<Person>().HasData(person);
            }

            modelBuilder.Entity<Person>().Property(x => x.TIN).HasColumnName("TaxNumber").HasColumnType("varchar(8)").HasDefaultValue("ABC12345");
            modelBuilder.Entity<Person>().HasCheckConstraint("CHK_TIN", "len([TaxNumber]) = 8");

            //Table Relations
            //modelBuilder.Entity<Person>(x =>
            //{
            //    x.HasOne<Country>(c=>c.Country).WithMany(x=>x.Persons).HasForeignKey(x=>x.CountryId);
            //});
        }
        public List<Person> sp_GetAllPersons()
        {
            return Persons.FromSqlRaw("EXECUTE [dbo].[GetAllPersons]").ToList();
        }
        public int sp_InsertPerson(Person person)
        {
            SqlParameter[] parameter = new SqlParameter[]
            {
                new SqlParameter("@Id",person.Id),
                new SqlParameter("@Name",person.Name),
                new SqlParameter("@Email",person.Email),
                new SqlParameter("@CountryId",person.CountryId),
                new SqlParameter("@DateOfBirth",person.DateOfBirth),
                new SqlParameter("@Gender" , person.Gender),
                new SqlParameter("@Address",person.Address),
                new SqlParameter("@RecieveNewsLetter" , person.RecieveNewsLetter)
            };

            return Database.ExecuteSqlRaw("EXECUTE [dbo].[InsertPerson]@Id,@Name,@Email,@CountryId,@DateOfBirth,@Gender,@Address,@RecieveNewsLetter", parameter);
        }
    }
}
