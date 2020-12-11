using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Taste.Models
{
    public class MenuItem
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Menu Item")]
        public string Name { get; set; }

        public string Description { get; set; }
        public string Image { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Price must be 1$ or greater!")]
        public double Price { get; set; }

        [Display(Name = "Category")] public int CategoryId { get; set; }

        [ForeignKey("CategoryId")] public virtual Category Category { get; set; }
        [Display(Name = "Food Type")] public int FoodTypeId { get; set; }

        [ForeignKey("FoodTypeId")] public virtual FoodType FoodType { get; set; }
    }
}