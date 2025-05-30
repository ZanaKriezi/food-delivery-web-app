using FoodDeliveryApp.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Domain.DTO
{
    public class ShoppingCartDTO
    {
        public List<FoodItemInCart>? FoodItemsInCarts { get; set; }
        public double TotalPrice { get; set; }
    }
}
