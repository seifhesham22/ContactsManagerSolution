using Entities;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// represents the DTO class that returns contains the person details to update.
    /// </summary>
    public class PersonUpdateRequest
    {
        [Required(ErrorMessage ="person id cannot be blank")]
        public Guid Id { get; set; }    
        [Required(ErrorMessage = "the person name cannot be empty")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Email cannot be blank")]
        [EmailAddress(ErrorMessage = "Email should be valid")]
        public string? Email { get; set; }
        public Guid? CountryId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public GenderOptions? Gender { get; set; }
        public string? Address { get; set; }
        public bool? RecieveNewsLetter { get; set; }
        /// <summary>
        /// Converts From PersonRequest to Person
        /// </summary>
        /// <returns>Person object</returns>
        public Person ToPerson()
        {
            return new Person()
            {
                Id = Id,
                Address = Address,
                Name = Name,
                Email = Email,
                CountryId = CountryId,
                DateOfBirth = DateOfBirth,
                Gender = Gender.ToString(),
                RecieveNewsLetter = RecieveNewsLetter
            };
        }
    }
}
