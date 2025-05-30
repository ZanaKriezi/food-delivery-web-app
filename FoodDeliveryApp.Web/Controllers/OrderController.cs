using FoodDeliveryApp.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using FoodDeliveryApp.Domain.Identity;
using FoodDeliveryApp.Repository.Interface;
using FoodDeliveryApp.Service.Interface;
using ClosedXML.Excel;
using System.IO;
using System.Linq;
using System.Text;
using FoodDeliveryApp.Domain.DomainModels;
using GemBox.Document;

namespace FoodDeliveryApp.Web.Controllers
{
    public class OrderController : Controller
    {
      
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");          
            _orderService = orderService;
         
        }


        [Route("Orders/OrderHistory/{userId?}")]
        public IActionResult OrderHistory(string userId)
        {

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity", returnUrl = Url.Action("Index", "Restaurants") });
            }


            var userOrders = _orderService.GetOrdersForCustomer(userId);
            return View("History", userOrders);
        }

        [HttpGet]
        public FileContentResult CreateInvoice(Guid id)
        {
            var order = _orderService.GetDetailsForOrder(new BaseEntity { Id = id });

            string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.docx");
            var document = DocumentModel.Load(templatePath);

            // Replace basic order information
            document.Content.Replace("{{OrderID}}", order.Id.ToString());
            document.Content.Replace("{{CustomerEmail}}", order.Customer.Email);

            // Combine restaurant name and delivery fee into one line
            string restaurantInfo = $"{order.Restaurant.Name} (Delivery Fee: {order.Restaurant.BaseDeliveryFee:F2} den.)";
            document.Content.Replace("{{RestaurantInfo}}", restaurantInfo);

            StringBuilder productList = new StringBuilder();
            decimal totalPrice = 0;

            if (order.FoodItemsInOrder != null && order.FoodItemsInOrder.Any())
            {
                foreach (var foodItem in order.FoodItemsInOrder)
                {
                    decimal itemPrice = foodItem.FoodItem.Price * foodItem.Quantity;

                    // Add food item information
                    productList.AppendLine($"{foodItem.FoodItem.Name} x{foodItem.Quantity}");
                    productList.AppendLine($"Price per item: {foodItem.FoodItem.Price:F2} den.");

                    // Handle extras if they exist
                    if (foodItem.FoodItem.Extras != null && foodItem.FoodItem.Extras.Any())
                    {
                        productList.AppendLine("Extras:");
                        foreach (var extra in foodItem.FoodItem.Extras)
                        {
                            if (extra.Extra != null)
                            {
                                productList.AppendLine($"- {extra.Extra.Name} ({extra.Price:F2} den.)");
                                itemPrice += extra.Price;
                            }
                        }
                    }

                    totalPrice += itemPrice;
                }
            }
            else
            {
                productList.AppendLine("No items in order");
            }

            // Add delivery fee to total price
            totalPrice += order.Restaurant.BaseDeliveryFee;

            document.Content.Replace("{{ProductList}}", productList.ToString());
            document.Content.Replace("{{TotalPrice}}", $"{totalPrice:F2} den.");

            var stream = new MemoryStream();
            document.Save(stream, new PdfSaveOptions());

            return File(stream.ToArray(), new PdfSaveOptions().ContentType, $"Invoice_{order.Id}.pdf");
        }

        [HttpGet]
        public FileContentResult ExportAllOrders()
        {
            string fileName = "Orders.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("Orders");

                // Set headers
                worksheet.Cell(1, 1).Value = "Order ID";
                worksheet.Cell(1, 2).Value = "Customer";
                worksheet.Cell(1, 3).Value = "Total Price";

                var orders = _orderService.GetAllOrders();
                int row = 2;

                // Find maximum number of food items for header setup
                int maxFoodItems = orders.Max(o => o.FoodItemsInOrder?.Count ?? 0);

                // Setup headers for all possible food items
                for (int i = 1; i <= maxFoodItems; i++)
                {
                    int foodCol = 4 + (i - 1) * 2;  // Each food item takes 2 columns (item + extras)
                    worksheet.Cell(1, foodCol).Value = $"Food Item {i}";
                    worksheet.Cell(1, foodCol + 1).Value = $"Extras {i}";
                }

                foreach (var order in orders)
                {
                    worksheet.Cell(row, 1).Value = order.Id.ToString();
                    worksheet.Cell(row, 2).Value = order.Customer?.Email ?? "N/A";

                    decimal totalPrice = 0;

                    if (order.FoodItemsInOrder != null)
                    {
                        int foodItemIndex = 1;
                        foreach (var foodItem in order.FoodItemsInOrder)
                        {
                            int foodCol = 4 + (foodItemIndex - 1) * 2;  // Calculate column for current food item

                            // Add food item
                            decimal itemPrice = foodItem.FoodItem.Price * foodItem.Quantity;
                            worksheet.Cell(row, foodCol).Value = $"{foodItem.FoodItem.Name} x{foodItem.Quantity}";

                            // Handle extras if they exist
                            if (foodItem.FoodItem.Extras != null && foodItem.FoodItem.Extras.Any())
                            {
                                StringBuilder extraList = new StringBuilder();
                                foreach (var extra in foodItem.FoodItem.Extras)
                                {
                                    if (extra.Extra != null)
                                    {
                                        if (extraList.Length > 0)
                                            extraList.Append(", ");
                                        extraList.Append($"{extra.Extra.Name} (${extra.Price:F2})");
                                        itemPrice += extra.Price;
                                    }
                                }

                                if (extraList.Length > 0)
                                {
                                    worksheet.Cell(row, foodCol + 1).Value = extraList.ToString();
                                }
                            }

                            totalPrice += itemPrice;
                            foodItemIndex++;
                        }
                    }

                    worksheet.Cell(row, 3).Value = totalPrice + "den.";
                    row++;
                }

                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
        }
    }


}