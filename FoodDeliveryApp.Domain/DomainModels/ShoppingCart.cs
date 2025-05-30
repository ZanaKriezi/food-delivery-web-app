using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using FoodDeliveryApp.Domain.Identity;

namespace FoodDeliveryApp.Domain.DomainModels
{
    public class ShoppingCart : BaseEntity
    {
        public string CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }

        [Range(0, double.MaxValue)]
        public decimal OrderAmount { get; set; }

        [Range(0, double.MaxValue)]
        public decimal DeliveryFee { get; set; }

        public string? AdditionalInfo { get; set; }

        public virtual ICollection<FoodItemInCart>? FoodItemsInCart { get; set; }
    }
}
