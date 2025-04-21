
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Person
    {
        [Key]
        public Guid Id { get; set; }
        [StringLength(40)]
        public string? Name { get; set; }
        [StringLength(40)]

        public string? Email { get; set; }
        public Guid? CountryId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public bool? RecieveNewsLetter { get; set; }
        public string? TIN { get; set; }
        [ForeignKey("CountryId")]
        public virtual Country? Country { get; set; }
    }
}
