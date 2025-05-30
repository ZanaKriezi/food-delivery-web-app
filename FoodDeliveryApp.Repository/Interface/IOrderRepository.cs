using FoodDeliveryApp.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Repository.Interface
{
  public interface IOrderRepository
    {
        Order GetDetailsForOrder(BaseEntity id);

       List <Order> GetAllOrders();
    }
}
