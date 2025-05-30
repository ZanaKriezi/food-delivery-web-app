using FoodDeliveryApp.Repository;
using FoodDeliveryApp.Repository.Interface;
using FoodDeliveryApp.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodDeliveryApp.Domain;
using FoodDeliveryApp.Domain.DomainModels;
using Microsoft.Extensions.DependencyInjection;
using FoodDeliveryApp.Domain.Email;

namespace FoodDeliveryApp.Service.Implementation
{
    public class OrderStatusService : IOrderStatusService
    {

        //scope here is used for Timer since it allows DBcontext to be used in the backround
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IRepository<Order> _orderRepository;
        private readonly IEmailService _emailService;
        private readonly ICustomerRepository _customerRepository;


        


        public OrderStatusService(IServiceScopeFactory scopeFactory, IRepository<Order> order
            ,IEmailService emailService,ICustomerRepository customerRepository)
        {
            _scopeFactory = scopeFactory;
            _orderRepository = order;
            _emailService = emailService;
            _customerRepository = customerRepository;
        }

        /*this is a timer for changing Status of order 
        the status of the order changes every three minutes*/
        


        public void ViewOrderStatus(Guid orderId)
        {

            TimerCallback timerCallback = (state) =>
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    var order = context.Orders.SingleOrDefault(o => o.Id == orderId);
                    if (order != null)
                    {
                        this.UpdateOrderStatus(order, context);
                    }
                }

            };
            //for testing it is set to 10 seconds on deploment it should be set to 60 000
            Timer timer = new Timer(timerCallback,
                                    null,
                                    0,
                                    60000);
           var checkOrder= this._orderRepository.Get(orderId);
            var loggedInUserId = checkOrder.CustomerId;
            var loggedInUser = this._customerRepository.GetCustomer(loggedInUserId);

            if (checkOrder.Status == OrderStatus.Delivered)
            {
                EmailMessage deliveredEmail= new EmailMessage();
                deliveredEmail.Subject = "Order delivered succesfully!";
                deliveredEmail.MailTo = loggedInUser.Email;

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Your order has been delivered to Address: " + loggedInUser.Address);
                sb.AppendLine("");
                sb.AppendLine("Thank you for ordering with us!");
                deliveredEmail.Content=sb.ToString();
                this._emailService.SendEmailMessage(deliveredEmail);
                
            }


        }

        private void UpdateOrderStatus(Order order, ApplicationDbContext context)
        {
            switch (order.Status)
            {
                case OrderStatus.Created:
                    order.Status = OrderStatus.Ready;
                    break;

                case OrderStatus.Ready:
                    order.Status = OrderStatus.BeingDelivered;
                    break;

                case OrderStatus.BeingDelivered:
                    order.Status = OrderStatus.Delivered;
                    break;

                case OrderStatus.Delivered:
                    // Order is already delivered 
                    //Email should be sent
                    return;
            }

            context.Orders.Update(order);
            context.SaveChanges();
        }
        



    }
}

