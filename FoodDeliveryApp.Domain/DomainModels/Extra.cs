using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryApp.Domain.DomainModels
{
    public class Extra: BaseEntity
    {
      
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public virtual ICollection<ExtraInFoodItem>? ExtraInFoodItems { get; set; }
    }
}
