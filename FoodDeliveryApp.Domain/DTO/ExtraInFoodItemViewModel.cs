using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Domain.DTO
{
    public class ExtraInFoodItemViewModel
    {
        public Guid Id { get; set; }
        public Guid ExtraId { get; set; }
        public Guid FoodItemId { get; set; }

      
        public string? ExtraName { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        public bool IsDeleted { get; set; }
    }
}
