using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryApp.Domain.DomainModels
{
    public class FoodItemInCart : BaseEntity
    {
        public Guid ShoppingCartId { get; set; }
        public virtual ShoppingCart? ShoppingCart { get; set; }
        public Guid FoodItemId { get; set; }
        public virtual FoodItem? FoodItem { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
