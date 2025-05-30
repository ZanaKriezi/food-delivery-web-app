using System.Text.Json;
using FoodDeliveryApp.Domain.DomainModels;
using FoodDeliveryApp.Domain.DTO;
using FoodDeliveryApp.Repository.Interface;
using FoodDeliveryApp.Service.Implementation;
using FoodDeliveryApp.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodDeliveryApp.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {

        private readonly IOrderService _orderService;
        private readonly IRestaurantService _restaurantService;

        public AdminController(IOrderService orderService, IRestaurantService restaurantService)
        {
            _orderService = orderService;
            _restaurantService = restaurantService;
        }

        [HttpGet("[action]")]
        public IActionResult GetAllOrders()
        {

            var orders = _orderService.GetAllOrders();
            return Ok(orders);
        }

        [HttpPost("[action]")]
        public Order GetDetails(BaseEntity id)
        {
            return this._orderService.GetDetailsForOrder(id);
        }

        [HttpPost("[action]")]
        public IActionResult ImportRestaurants([FromBody] List<RestaurantImportDto> model)
        {
            if (model == null || model.Count == 0)
                return BadRequest(new { message = "Invalid restaurant data." });

            bool status = true;

            foreach (var item in model)
            {
                var existingRestaurant = _restaurantService.FindByName(item.Name);

                if (existingRestaurant == null)
                {
                    var newRestaurant = new Restaurant
                    {
                        Name = item.Name,
                        Address = item.Address,
                        IsOpen = item.IsOpen,
                        AverageRating = item.AverageRating,
                        ImageUrl = item.ImageUrl,
                        PhoneNumber = item.PhoneNumber,
                        BaseDeliveryFee = item.BaseDeliveryFee
                    };

                    var result = _restaurantService.CreateRestaurant(newRestaurant);
                    status = status && result;
                }
            }

            return status ? Ok(new { message = "Restaurants imported successfully." }) :
                            StatusCode(500, new { message = "Error importing restaurants." });
        }

        

    
        
        
        
    }
}