using FoodDeliveryApp.Domain.DomainModels;
using FoodDeliveryApp.Domain.DTO;
using FoodDeliveryApp.Repository.Interface;
using FoodDeliveryApp.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Service.Implementation
{
    public class FoodItemService : IFoodItemService
    {
        private readonly IFoodItemRepository _repository;
        private readonly IExtraService _extraService;
        public FoodItemService(IFoodItemRepository repository, IExtraService extraService)
        {
            _repository = repository;
            _extraService = extraService;
        }

        public void CreateNewFoodItem(CreateFoodItemViewModel viewModel)
        {
            var foodItem = new FoodItem
            {
                Name = viewModel.Name,
                Description = viewModel.Description,
                Price = viewModel.Price,
                ImageUrl = viewModel.ImageUrl,
                RestaurantId = viewModel.RestaurantId
            };

            _repository.Save(foodItem);
        }

        public void DeleteFoodItem(Guid id)
        {
            _repository.Delete(GetFoodItemById(id));
        }

        

        

        public FoodItem GetFoodItemById(Guid id)
        {
            return _repository.GetById(id);
        }

        public void UpdateFoodItem(EditFoodItemViewModel viewModel)
        {
            var foodItem = _repository.GetById(viewModel.FoodItemId);
            if (foodItem == null)
            {
                throw new ArgumentException("Food item not found");
            }

            // Update food item properties
            foodItem.Name = viewModel.Name;
            foodItem.Description = viewModel.Description;
            foodItem.Price = viewModel.Price;
            foodItem.ImageUrl = viewModel.ImageUrl;

            // Update extras
            var updatedExtras = new List<ExtraInFoodItem>();
            foreach (var extraViewModel in viewModel.ExtraInFoodItems)
            {
                if (!extraViewModel.IsDeleted)
                {
                    var existingExtra = foodItem?.Extras?.FirstOrDefault(e => e.Id == extraViewModel.Id);
                    if (existingExtra != null)
                    {
                        existingExtra.Price = extraViewModel.Price;
                        existingExtra.Extra.Name = extraViewModel.ExtraName;
                        updatedExtras.Add(existingExtra);
                    }
                    else
                    {
                        // This is a new extra being added to an existing food item
                        updatedExtras.Add(new ExtraInFoodItem
                        {
                            Id = new Guid(),
                            ExtraId = extraViewModel.ExtraId,
                            FoodItemId = foodItem.Id,
                            Price = extraViewModel.Price
                        });
                    }
                }
                else
                {
                    var extraToRemove = foodItem.Extras.FirstOrDefault(e => e.Id == extraViewModel.Id);
                    if (extraToRemove != null)
                    {
                        _extraService.RemoveExtraFromFoodItem(extraToRemove);
                    }
                }
            }

            // Add new extra if provided
            if (!string.IsNullOrEmpty(viewModel.NewExtraName))
            {
                var newExtra = _extraService.CreateExtra(new Extra { 
                    Id = new Guid(), 
                    Name = viewModel.NewExtraName });
                updatedExtras.Add(new ExtraInFoodItem
                {
                    Id= new Guid(),
                    ExtraId = newExtra.Id,
                    FoodItemId = foodItem.Id,
                    Price = viewModel.NewExtraPrice
                });
            }

            foodItem.Extras = updatedExtras;

            _repository.Update(foodItem);
        }

    }
}
