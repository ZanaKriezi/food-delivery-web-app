
using FoodDeliveryApp.Domain.DomainModels;
using FoodDeliveryApp.Domain.DTO;

namespace FoodDeliveryApp.Service.Interface
{
    public interface IFoodItemService
    {
        void CreateNewFoodItem(CreateFoodItemViewModel viewModel);
        void DeleteFoodItem(Guid id);
       
         

        FoodItem GetFoodItemById(Guid id);
        void UpdateFoodItem(EditFoodItemViewModel viewModel);
    }
}
