using FoodDeliveryApp.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Service.Interface
{
    public  interface IOrderService 
    {
        Order GetDetailsForOrder(BaseEntity id);
        List<Order> GetAllOrders();

        List<Order> GetOrdersForCustomer(string customerId);

    }
}
