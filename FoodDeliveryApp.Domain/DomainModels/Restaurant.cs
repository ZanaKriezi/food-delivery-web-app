using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryApp.Domain.DomainModels
{
    public class Restaurant : BaseEntity
    {
      

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        public bool IsOpen { get; set; }

        [Range(0, 5)]
        public double AverageRating { get; set; }

        public string? ImageUrl { get; set; }
        public string? PhoneNumber { get; set; }

        [Range(0, double.MaxValue)]
        public decimal BaseDeliveryFee { get; set; }

        public virtual ICollection<FoodItem>? FoodItems { get; set; }
        public virtual ICollection<Review>? Reviews { get; set; }
        public virtual ICollection<Order>? Orders { get; set; }
    }
}
