using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryApp.Domain.DomainModels
{
    public class ExtraInFoodItem : BaseEntity
    {
        public Guid ExtraId { get; set; }
        public virtual Extra? Extra { get; set; }
        public Guid FoodItemId { get; set; }
        public virtual FoodItem? FoodItem { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
    }
}
