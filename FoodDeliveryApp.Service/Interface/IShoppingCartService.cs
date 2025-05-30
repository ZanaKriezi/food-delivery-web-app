using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodDeliveryApp.Domain.DomainModels;
using FoodDeliveryApp.Domain.DTO;

namespace FoodDeliveryApp.Service.Interface
{
    public interface IShoppingCartService
    {
        ShoppingCartDTO GetInfoShoppingCart(string userId);
        bool DeleteFromShoppingCart(string userId, Guid productId);
        bool AddToShoppingConfirmed(FoodItemInCart model, string userId, IEnumerable<Guid> selectedExtras = null);

        Task<bool> Order(string userId);
    }
}
