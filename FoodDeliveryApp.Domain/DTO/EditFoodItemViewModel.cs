using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Domain.DTO
{
    public class EditFoodItemViewModel
    {
        public Guid FoodItemId { get; set; }

     
        [MaxLength(100)]
        public string? Name { get; set; }

        public string? Description { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        public Guid RestaurantId { get; set; }

        public List<ExtraInFoodItemViewModel>? ExtraInFoodItems { get; set; } = new List<ExtraInFoodItemViewModel>();

        [MaxLength(50)]
        public string? NewExtraName { get; set; }

        [Range(0, double.MaxValue)]
        public decimal NewExtraPrice { get; set; }
    }
}
