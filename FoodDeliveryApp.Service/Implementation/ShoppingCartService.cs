using FoodDeliveryApp.Domain.DomainModels;
using FoodDeliveryApp.Domain.DTO;
using FoodDeliveryApp.Repository;
using FoodDeliveryApp.Repository.Implementation;
using FoodDeliveryApp.Repository.Interface;
using FoodDeliveryApp.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using FoodDeliveryApp.Domain.Email;

namespace FoodDeliveryApp.Service.Implementation
{
    public class ShoppingCartService : IShoppingCartService
    {

        private readonly ILogger<ShoppingCartService> _logger;
        private readonly IRepository<ShoppingCart> _shoppingCartRepository;
        private readonly ApplicationDbContext _context;
        private readonly  IRepository<FoodItemInCart> _foodItemInCartRepository;
        private readonly ICustomerRepository _userRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IEmailService _emailService;
        private readonly IExtraRepository _extraInFoodItemRepository;
        
        public ShoppingCartService(IRepository<ShoppingCart> shoppingCartRepository,
            IRepository<FoodItemInCart> foodItemInCartRepository,
            ApplicationDbContext context, IRepository<Order> orderRepository,
            ICustomerRepository userRepository, ILogger<ShoppingCartService> logger,
            IEmailService emailService,
            IExtraRepository extraInFoodItemRepository)
        {
            _foodItemInCartRepository = foodItemInCartRepository;
            _userRepository = userRepository;
            _shoppingCartRepository=shoppingCartRepository;
            _logger = logger;
            _context = context;
            _orderRepository = orderRepository;
            _emailService = emailService;
            _extraInFoodItemRepository = extraInFoodItemRepository;
            
        }

        public bool AddToShoppingConfirmed(FoodItemInCart model, string userId, IEnumerable<Guid> selectedExtras = null)
        {
            try
            {
                var user = _userRepository.GetCustomer(userId);
                if (user == null)
                {
                    _logger.LogError($"User with ID {userId} not found");
                    return false;
                }

                var shoppingCart = user.ShoppingCart;

                // Load the food item with its extras
                var foodItem = _context.FoodItems
                    .Include(f => f.Extras)
                        .ThenInclude(e => e.Extra)
                    .FirstOrDefault(f => f.Id == model.FoodItemId);

                if (foodItem == null)
                {
                    _logger.LogError($"Food item with ID {model.FoodItemId} not found");
                    return false;
                }

                // Filter the extras to only include selected ones
                if (selectedExtras != null && selectedExtras.Any())
                {
                    foodItem.Extras = foodItem.Extras
                        .Where(e => selectedExtras.Contains(e.Id))
                        .ToList();
                }
                else
                {
                    foodItem.Extras = new List<ExtraInFoodItem>();
                }

                // Create the cart item
                var itemToAdd = new FoodItemInCart
                {
                    Id = Guid.NewGuid(),
                    FoodItemId = model.FoodItemId,
                    ShoppingCart = shoppingCart,
                    ShoppingCartId = shoppingCart.Id,
                    Quantity = model.Quantity,
                    FoodItem = foodItem
                };

                _foodItemInCartRepository.Insert(itemToAdd);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding item to cart: {ex.Message}", ex);
                return false;
            }
        }


        public bool DeleteFromShoppingCart(string userId, Guid FoodId)
        {
            if (!string.IsNullOrEmpty(userId) && FoodId != null)
            {
                var loggedInUser = this._userRepository.GetCustomer(userId);

                var userShoppingCart = loggedInUser.ShoppingCart;

                var itemToDelete = userShoppingCart.FoodItemsInCart.Where(z => z.FoodItemId.Equals(FoodId)).FirstOrDefault();

                userShoppingCart.FoodItemsInCart.Remove(itemToDelete);

                this._shoppingCartRepository.Update(userShoppingCart);

                return true;
            }
            return false;
        }


        public ShoppingCartDTO GetInfoShoppingCart(string userId)
        {
            var user = this._userRepository.GetCustomer(userId);
            var shoppingCart = _shoppingCartRepository.GetAll()
                .FirstOrDefault(c => c.Id == user.ShoppingCart.Id);

            if (shoppingCart == null)
            {
                return new ShoppingCartDTO()
                {
                    FoodItemsInCarts = new List<FoodItemInCart>(),
                    TotalPrice = 0
                };
            }

            // Manually load related entities with their extras
            foreach (var foodItemInCart in shoppingCart.FoodItemsInCart)
            {
                _context.Entry(foodItemInCart)
                    .Reference(fic => fic.FoodItem)
                    .Load();

                _context.Entry(foodItemInCart.FoodItem)
                    .Reference(fi => fi.Restaurant)
                    .Load();

                _context.Entry(foodItemInCart.FoodItem)
                    .Collection(fi => fi.Extras)
                    .Query()
                    .Include(e => e.Extra)
                    .Load();
            }

            decimal totalPrice = shoppingCart.FoodItemsInCart
                .GroupBy(item => item.FoodItem.RestaurantId)
                .Sum(restaurantGroup =>
                {
                    // Calculate items total for this restaurant
                    var itemsTotal = restaurantGroup.Sum(cartItem =>
                    {
                        var itemPrice = cartItem.FoodItem.Price; // Base price
                        var extrasPrice = cartItem.FoodItem.Extras != null ?
                            cartItem.FoodItem.Extras.Sum(extra => extra.Price) : 0m;

                        return (itemPrice + extrasPrice) * cartItem.Quantity;
                    });

                    // Add one delivery fee per restaurant
                    return itemsTotal + restaurantGroup.First().FoodItem.Restaurant.BaseDeliveryFee;
                });

            return new ShoppingCartDTO()
            {
                FoodItemsInCarts = shoppingCart.FoodItemsInCart?.ToList() ?? new List<FoodItemInCart>(),
                TotalPrice = (double)totalPrice
            };
        }
        public async Task<bool> Order(string userID)
        {
            if (!string.IsNullOrEmpty(userID))
            {
                try
                {
                    var loggedInUser = this._userRepository.GetCustomer(userID);
                    var userCart = loggedInUser.ShoppingCart;

                    var order = new Order
                    {
                        Id = Guid.NewGuid(),
                        CustomerId = userID,
                        OrderDate = DateTime.Now,
                        Status = 0,
                        TotalAmount = userCart.FoodItemsInCart.Sum(f =>
                            (f.FoodItem.Price * f.Quantity) +
                            f.FoodItem.Restaurant.BaseDeliveryFee),
                        RestaurantId = userCart.FoodItemsInCart.First().FoodItem.RestaurantId,
                    };

                    order.FoodItemsInOrder = userCart.FoodItemsInCart.Select(f => new FoodItemInOrder
                    {
                        Id = Guid.NewGuid(),
                        OrderId = order.Id,
                        FoodItemId = f.FoodItemId,
                        FoodItem = f.FoodItem,
                        Quantity = f.Quantity,
                    }).ToList();

                    // Create and send order confirmation email
                    var emailMessage = new EmailMessage
                    {
                        MailTo = loggedInUser.Email,
                        Subject = "Order Confirmation - Foodie Deliveries",
                        Content = BuildOrderConfirmationEmail(order)
                    };

                    // Send email first to ensure customer gets notification
                    await _emailService.SendEmailMessage(emailMessage);

                    // Save order to database
                    await _orderRepository.InsertAsync(order);

                    // Clear the shopping cart
                    userCart.FoodItemsInCart.Clear();
                    await _userRepository.UpdateAsync(loggedInUser);

                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error processing order for user {userID}: {ex.Message}", ex);
                    return false;
                }
            }
            return false;
        }

        private string BuildOrderConfirmationEmail(Order order)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Your order has been successfully placed!");
            sb.AppendLine();
            sb.AppendLine("Order Details:");
            sb.AppendLine($"Order ID: {order.Id}");
            sb.AppendLine($"Order Date: {order.OrderDate:g}");
            sb.AppendLine();
            sb.AppendLine("Items ordered:");

            foreach (var item in order.FoodItemsInOrder)
            {
                var itemTotal = item.FoodItem.Price * item.Quantity;
                sb.AppendLine($"- {item.FoodItem.Name} x {item.Quantity} @ {item.FoodItem.Price:C} = {itemTotal:C}");

                if (item.FoodItem.Extras != null && item.FoodItem.Extras.Any())
                {
                    sb.AppendLine("  Extras:");
                    foreach (var extra in item.FoodItem.Extras)
                    {
                        sb.AppendLine($"    + {extra.Extra.Name} ({extra.Price:C})");
                    }
                }
            }

            sb.AppendLine();
            sb.AppendLine($"Delivery Fee: {order.FoodItemsInOrder.First().FoodItem.Restaurant.BaseDeliveryFee:C}");
            sb.AppendLine($"Total Amount: {order.TotalAmount:C}");
            sb.AppendLine();
            sb.AppendLine("Thank you for ordering with Foodie Deliveries!");
            sb.AppendLine("We'll notify you when your order is ready for delivery.");

            return sb.ToString();
        }
    }
}
