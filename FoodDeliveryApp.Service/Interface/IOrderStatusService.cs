using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Service.Interface
{
    public interface IOrderStatusService
    {
        public void ViewOrderStatus(Guid orderId);
    }
}
