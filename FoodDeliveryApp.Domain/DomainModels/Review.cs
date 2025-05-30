using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using FoodDeliveryApp.Domain.Identity;

namespace FoodDeliveryApp.Domain.DomainModels
{
    public class Review : BaseEntity
    {
       
        public Guid RestaurantId { get; set; }
        public virtual Restaurant? Restaurant { get; set; }
        public string CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [MaxLength(2000)]
        public string? Comment { get; set; }

        public DateTime DatePosted { get; set; }
    }
}
