using FoodDeliveryApp.Domain.DomainModels;
using FoodDeliveryApp.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Service.Interface
{
    public interface IRestaurantService
    {
        void AddReviewToRestaurant(AddReviewViewModel reviewDto, string userId);
        void CreateNewRestaurant(Restaurant restaurant);
        void DeleteRestaurant(Guid id);
        List<Restaurant> GetAllRestaurants();
        
        Restaurant GetDetails(Guid? id);
        void UpdateExistingRestaurant(Restaurant restaurant);

        Restaurant FindByName(string name);
        bool CreateRestaurant(Restaurant restaurant);


    }
}
