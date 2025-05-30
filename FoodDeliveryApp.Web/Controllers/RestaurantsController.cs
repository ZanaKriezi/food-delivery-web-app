using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FoodDeliveryApp.Domain.DomainModels;
using FoodDeliveryApp.Repository;
using FoodDeliveryApp.Service.Interface;
using FoodDeliveryApp.Domain.DTO;
using System.Security.Claims;
using FoodDeliveryApp.Service.Implementation;
using MailKit.Search;

namespace FoodDeliveryApp.Web.Controllers
{
    public class RestaurantsController : Controller
    {
        private readonly IRestaurantService _restaurantService;

       public RestaurantsController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        // GET: Restaurants
        public IActionResult Index(string searchTerm)
        {
            List<Restaurant> restaurants = _restaurantService.GetAllRestaurants();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                restaurants = restaurants.Where(r => r.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            ViewData["CurrentFilter"] = searchTerm;

            return View(restaurants);
        }

        // GET: Restaurants/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurant = _restaurantService.GetDetails(id);
            if (restaurant == null)
            {
                return NotFound();
            }

            return View(restaurant);
        }
        public IActionResult GetMenu(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurant = _restaurantService.GetDetails(id);
            if (restaurant == null)
            {
                return NotFound();
            }

            return View(restaurant);
        }

        // GET: Restaurants/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Restaurants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,Address,IsOpen,AverageRating,ImageUrl,PhoneNumber,BaseDeliveryFee,Id")] Restaurant restaurant)
        {
            if (ModelState.IsValid)
            {
                restaurant.Id = Guid.NewGuid();
                _restaurantService.CreateNewRestaurant(restaurant);
                return RedirectToAction(nameof(Index));
            }
            return View(restaurant);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddReview([Bind("Rating,Comment,RestaurantId")] AddReviewViewModel reviewDto)
        {
            if (!User.Identity.IsAuthenticated)
            {
               
                var returnUrl = Url.Action("Details", "Restaurants", new { id = reviewDto.RestaurantId });
                return RedirectToPage("/Account/Login", new { area = "Identity", returnUrl });
            }

            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _restaurantService.AddReviewToRestaurant(reviewDto, userId);
                return RedirectToAction("Details", "Restaurants", new { id = reviewDto.RestaurantId });
            }
            return RedirectToAction("Details", "Restaurants", new { id = reviewDto.RestaurantId });
        }

        // GET: Restaurants/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurant = _restaurantService.GetDetails(id);
            if (restaurant == null)
            {
                return NotFound();
            }
            return View(restaurant);
        }

        // POST: Restaurants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Name,Address,IsOpen,AverageRating,ImageUrl,PhoneNumber,BaseDeliveryFee,Id")] Restaurant restaurant)
        {
            if (id != restaurant.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _restaurantService.UpdateExistingRestaurant(restaurant);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RestaurantExists(restaurant.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(restaurant);
        }

        // GET: Restaurants/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurant = _restaurantService.GetDetails(id);
            if (restaurant == null)
            {
                return NotFound();
            }

            return View(restaurant);
        }

        // POST: Restaurants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var restaurant = _restaurantService.GetDetails(id);
            if (restaurant != null)
            {
                _restaurantService.DeleteRestaurant(id);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool RestaurantExists(Guid id)
        {
            return _restaurantService.GetDetails(id) != null;
        }
       

    }
}
