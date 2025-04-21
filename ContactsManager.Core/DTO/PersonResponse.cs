using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Entities;
using ServiceContracts.Enums;

namespace ServiceContracts.DTO
{
    public class PersonResponse
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Country { get; set; }
        public Guid? CountryId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public bool? RecieveNewsLetter { get; set; }
        public double? Age { get; set; }
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(PersonResponse)) { return false; }
            PersonResponse other = (PersonResponse)obj;
            return Id == other.Id && Name == other.Name && Email == other.Email && Country
            == other.Country && CountryId == other.CountryId && DateOfBirth ==
            other.DateOfBirth && Gender == other.Gender && Address ==
            other.Address && RecieveNewsLetter ==
            other.RecieveNewsLetter && Age == other.Age;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override string ToString()
        {
            return $"Person Id: {Id} , Person Name: {Name} , Person Email: {Email} , Person Country: {Country} , Person Country Id: {CountryId}" +
                $"Person Date of birth: {DateOfBirth} , Person Gender {Gender} , Person Address {Address} , Recieved Letter: {RecieveNewsLetter}";
        }
        public PersonUpdateRequest ToPersonUpdateRequest()
        {
            return new PersonUpdateRequest()
            {
                Address = Address,
                CountryId = CountryId,
                DateOfBirth = DateOfBirth,
                Email = Email,
                Gender = (GenderOptions)Enum.Parse(typeof(GenderOptions),Gender,true),
                Id = Id,
                Name = Name,
                RecieveNewsLetter = RecieveNewsLetter,
            };
        }
    }
    public static class PersonExtentions
    {
        public static PersonResponse ToPersonResponse(this Person person) 
        {
            return new PersonResponse()
            {
                Id = person.Id,
                Name = person.Name,
                Email = person.Email,
                DateOfBirth = person.DateOfBirth,
                Gender = person.Gender,
                Address = person.Address,
                RecieveNewsLetter = person.RecieveNewsLetter,
                CountryId = person.CountryId,
                Age = (person.DateOfBirth != null) ? Math.Round((DateTime.Now - person.DateOfBirth.Value).TotalDays/365.25) : null,
                Country = person.Country?.CountryName 
                

            };
        }

    }
    
    
}
