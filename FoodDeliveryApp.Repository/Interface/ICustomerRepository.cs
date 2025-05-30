using FoodDeliveryApp.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Repository.Interface
{
    public interface ICustomerRepository
    {
        Customer GetCustomer(string? Id);
         void Update(Customer customer);
        Task UpdateAsync(Customer customer);
    }
}
