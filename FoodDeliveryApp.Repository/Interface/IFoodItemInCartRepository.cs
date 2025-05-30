using FoodDeliveryApp.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Repository.Interface
{
    public interface IFoodItemInCartRepository
    {
        void Delete(FoodItemInCart foodItem);

        FoodItemInCart GetById(Guid id);
        void Save(FoodItemInCart foodItem);
        void Update(FoodItemInCart foodItem);
    }
}
