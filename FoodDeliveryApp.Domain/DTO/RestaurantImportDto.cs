using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Domain.DTO
{
     public class RestaurantImportDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        public bool IsOpen { get; set; } = true;

        [Range(0, 5)]
        public double AverageRating { get; set; } = 0;

        public string? ImageUrl { get; set; }
        public string? PhoneNumber { get; set; }

        [Range(0, double.MaxValue)]
        public decimal BaseDeliveryFee { get; set; }
    }
}
