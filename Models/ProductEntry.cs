using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KcalCalc.Models
{
    public class ProductEntry
    {
        [Key]
        public int ID {get; set;}

        [Required]
        public int ProductAmount {get; set;}

        [ForeignKey("KcalDay")]
        public int KcalDayID {get; set;}
        public virtual KcalDay KcalDay {get; set;}

        [ForeignKey("Product")]
        public int ProductID {get; set;}
        public virtual Product Product {get; set;}
    }
}