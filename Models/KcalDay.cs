
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KcalCalc.Models
{
    public class KcalDay
    {
        [Key]
        public int ID {get; set;}

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date {get; set;}
  
        [ForeignKey("Person")]
        public int PersonID {get; set;}
        public virtual Person Person {get; set;}

        public virtual ICollection<ProductEntry> ProductEntries {get; set;}
    }
}