using System.ComponentModel.DataAnnotations;

namespace KcalCalc.Models
{
    public class Person
    {
        [Key]
        public int ID {get; set;}

        [Required]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "First name should be longer than 4 characters and shorter than 100 characters")]
        public string Firstname {get; set;}

        [Required]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "Sur name should be longer than 4 characters and shorter than 100 characters")]
        public string Surname {get; set;}

        public virtual ICollection<KcalDay>  KcalDays {get; set;}
    }
}