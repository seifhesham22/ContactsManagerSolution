using System.Text;
using System.Threading.Tasks;
using ServiceContracts.Enums;
using Entities;
using System.ComponentModel.DataAnnotations;
namespace ServiceContracts.DTO
{
    public class PersonAddRequest
    {
        /// <summary>
        /// acts a a DTO for inserting a new perosn
        /// </summary>
        [Required(ErrorMessage ="the person name cannot be empty")]
        public string? Name { get; set; }
        [Required(ErrorMessage ="Email cannot be blank")]
        [EmailAddress(ErrorMessage ="Email should be valid")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [Required(ErrorMessage ="Please Choose a Country")]
        public Guid? CountryId { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }
        [Required(ErrorMessage ="Please choose gender")]
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
