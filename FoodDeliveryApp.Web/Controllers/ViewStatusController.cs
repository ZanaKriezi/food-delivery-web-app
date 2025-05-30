using FoodDeliveryApp.Repository.Interface;
using FoodDeliveryApp.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FoodDeliveryApp.Web.Controllers
{
    public class ViewStatusController : Controller
    {
        private readonly IOrderStatusService _orderStatus;
        private readonly ICustomerRepository _customerRepository;
        private readonly IOrderService _orderServie;


        public ViewStatusController(IOrderStatusService orderStatus, ICustomerRepository customerRepository, IOrderService orderService)
        {
            this._orderStatus = orderStatus;
            this._customerRepository = customerRepository;
            this._orderServie = orderService;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage("/Account/Login", new { area = "Identity", returnUrl = Url.Action("Index", "ViewStatus") });
            }

            var customer = _customerRepository.GetCustomer(userId);

            if (customer == null || customer.Orders == null || !customer.Orders.Any())
            {
                return RedirectToAction("Index", "Home");
            }

            var customerOrder = customer.Orders
                .OrderByDescending(o => o.OrderDate)
                .FirstOrDefault();

            var orderId = new Domain.DomainModels.BaseEntity
            {
                Id = customerOrder.Id
            };

            var order = _orderServie.GetDetailsForOrder(orderId);

            if (order == null)
            {
                return RedirectToAction("Index", "Home");
            }

            _orderStatus.ViewOrderStatus(order.Id);
            return PartialView("_ViewStatusModal", order);
        }
    }
}
