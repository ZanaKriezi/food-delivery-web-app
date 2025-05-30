using FoodDeliveryApp.Domain.DomainModels;
using FoodDeliveryApp.Domain.DTO;
using FoodDeliveryApp.Domain.Identity;
using FoodDeliveryApp.Repository.Interface;
using FoodDeliveryApp.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Service.Implementation
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly ICustomerRepository _userRepository;
        private readonly IReviewRepository reviewRepository;
        public RestaurantService(IRestaurantRepository restaurantRepository, 
                                ICustomerRepository userRepository, 
                                IReviewRepository _reviewRepository)
        {
            _restaurantRepository = restaurantRepository;
            _userRepository = userRepository;
            reviewRepository = _reviewRepository;
        }

        public void AddReviewToRestaurant(AddReviewViewModel reviewDto, string userId)
        {
            Restaurant restaurant = _restaurantRepository.GetRestaurant(reviewDto.RestaurantId);
            Customer customer = _userRepository.GetCustomer(userId);
            if (restaurant != null && customer != null)
            {
                Review review = new Review()
                {
                    Id = new Guid(),
                    Restaurant = restaurant,
                    RestaurantId = reviewDto.RestaurantId,
                    Customer = customer,
                    CustomerId = userId,
                    Rating = reviewDto.Rating,
                    Comment = reviewDto.Comment,
                    DatePosted = DateTime.Now
                };

               reviewRepository.Insert(review);
                double newRatingSum = restaurant.Reviews.Select(r => r.Rating).Sum();
                double rating = newRatingSum / restaurant.Reviews.Count();
                restaurant.AverageRating = rating;
                _restaurantRepository.Update(restaurant);
            }
           
        }

        public void CreateNewRestaurant(Restaurant restaurant)
        {
            _restaurantRepository.Insert(restaurant);
        }

        public bool CreateRestaurant(Restaurant restaurant)
        {
            try
            {
                _restaurantRepository.Insert(restaurant);
                return true;
            }
            catch
            {
                return false;
            }
        }

            public void DeleteRestaurant(Guid id)
        {
            _restaurantRepository.Delete(GetDetails(id));
        }

        public Restaurant FindByName(string name)
        {
            return _restaurantRepository.GetAll().FirstOrDefault(r => r.Name == name);
        }

        public List<Restaurant> GetAllRestaurants()
        {
            return _restaurantRepository.GetAll().ToList();
        }

        public Restaurant GetDetails(Guid? id)
        {
            return _restaurantRepository.GetRestaurant(id);
        }

       
        public void UpdateExistingRestaurant(Restaurant restaurant)
        {
            _restaurantRepository.Update(restaurant);
        }
    }
}
