using FoodDeliveryApp.Domain.DomainModels;
using FoodDeliveryApp.Repository.Interface;
using FoodDeliveryApp.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Service.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository orderRepository;
        public OrderService(IOrderRepository orderRepository)
        {
           this. orderRepository = orderRepository;

        }

        public List<Order> GetAllOrders()
        {
            return orderRepository.GetAllOrders();
        }

        public Order GetDetailsForOrder(BaseEntity id)
            {
            return orderRepository.GetDetailsForOrder(id);

            }

        public List<Order> GetOrdersForCustomer(string customerId)
        {
            return orderRepository.GetAllOrders().Where(o => o.Customer.Id == customerId).ToList();

        }
    }
    }