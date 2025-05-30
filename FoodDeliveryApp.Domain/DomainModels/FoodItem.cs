using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryApp.Domain.DomainModels
{
    public class FoodItem : BaseEntity
    {
      

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public string? Description { get; set; }

        public int TimesOrdered { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        public Guid RestaurantId { get; set; }
        public virtual Restaurant? Restaurant { get; set; }

        public virtual ICollection<ExtraInFoodItem>? Extras { get; set; }
        public virtual ICollection<FoodItemInCart>? FoodItemInCarts { get; set; }
        public virtual ICollection<FoodItemInOrder>? FoodItemInOrders { get; set; }
    }
}
