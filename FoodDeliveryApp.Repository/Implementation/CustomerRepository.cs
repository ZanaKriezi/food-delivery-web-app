using FoodDeliveryApp.Domain.DomainModels;
using FoodDeliveryApp.Domain.Identity;
using FoodDeliveryApp.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Repository.Implementation
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<Customer> entities;
        string errorMessage = string.Empty;

        public CustomerRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<Customer>();
        }

        public Customer GetCustomer(string? Id)
        {
            return entities
                .Include(s => s.ShoppingCart)
                .Include("ShoppingCart.FoodItemsInCart")
                .Include("ShoppingCart.FoodItemsInCart.FoodItem")
                .Include("ShoppingCart.FoodItemsInCart.FoodItem.Extras")
                .Include("ShoppingCart.FoodItemsInCart.FoodItem.Extras.Extra")
                .Include(s => s.Orders)
                .Include("Orders.FoodItemsInOrder.FoodItem")
                .Include("Orders.FoodItemsInOrder.FoodItem.Extras")
                .Include("Orders.FoodItemsInOrder.FoodItem.Extras.Extra")
                .Include(s => s.Reviews)
                .Where(c => c.Id.Equals(Id))
                .FirstOrDefault();
        }

        public void Update(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException("custumer");
            }
            entities.Update(customer);
            context.SaveChanges();
        }

        public async Task UpdateAsync(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException("customer");
            }
            entities.Update(customer);
            await context.SaveChangesAsync();
        }
    }
}
