using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using FoodDeliveryApp.Domain.Identity;

namespace FoodDeliveryApp.Domain.DomainModels
{
    public class Order : BaseEntity
    {
        

        [Column(TypeName = "nvarchar(20)")]
        public OrderStatus Status { get; set; }

        [Range(0, double.MaxValue)]
        public decimal TotalAmount { get; set; }

        public DateTime OrderDate { get; set; }

        public string? AdditionalInfo { get; set; }
        public string CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }
        public Guid RestaurantId { get; set; }
        public virtual Restaurant? Restaurant { get; set; }
        public virtual ICollection<FoodItemInOrder>? FoodItemsInOrder { get; set; }
    }
}
