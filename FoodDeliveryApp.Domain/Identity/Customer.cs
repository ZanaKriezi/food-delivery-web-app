using FoodDeliveryApp.Domain.DomainModels;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryApp.Domain.Identity
{
    public class Customer : IdentityUser

    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string Surname { get; set; }

        public string? Address { get; set; }
        public int Age { get; set; }

        public virtual ICollection<Order>? Orders { get; set; }
        public virtual ICollection<Review>? Reviews { get; set; }
        public virtual ShoppingCart? ShoppingCart { get; set; }
    }
}
