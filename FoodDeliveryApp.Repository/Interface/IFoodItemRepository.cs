using FoodDeliveryApp.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FoodDeliveryApp.Repository.Interface
{
    public interface IFoodItemRepository
    {
        void Delete(FoodItem foodItem);
       
        FoodItem GetById(Guid id);
        void Save(FoodItem foodItem);
        void Update(FoodItem foodItem);
    }
}
