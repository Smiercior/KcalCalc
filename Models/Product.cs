using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace KcalCalc.Models
{
    public class Product
    {
        [Key]
        public int ID {get; set;}

        [Required]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Product name should be longer than 4 characters and shorter than 100 characters")]
        public string Name {get; set;}

        [Required]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Product brand should be longer than 4 characters and shorter than 100 characters")]
        public string Brand {get; set;}

        [Required]
        [Range(0, 5000)]
        public float Kcal {get; set;}

        [Required]
        [Range(0, 5000)]
        public float Protein {get; set;}

        [Required]
        [Range(0, 5000)]
        public float Carbohydrate {get; set;}

        [Required]
        [Range(0, 5000)]
        public float Fat {get; set;}

        [Required]
        [Range(0, 5000)]
        public int BasicAmountGram {get; set;}

        [BindNever]
        public virtual ICollection<ProductEntry>? ProductEntries {get; set;}
    }
}