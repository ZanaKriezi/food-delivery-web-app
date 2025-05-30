using FoodDeliveryApp.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Domain.DTO
{
    public class AddToCartViewModel
    {
        public Guid FoodItemId { get; set; }
        public virtual FoodItem? FoodItem { get; set; }
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
        public IEnumerable<Guid>? SelectedExtras { get; set; }
    }
}
