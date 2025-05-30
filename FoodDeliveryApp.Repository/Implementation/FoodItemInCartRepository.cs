using FoodDeliveryApp.Domain.DomainModels;
using FoodDeliveryApp.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Repository.Implementation
{
    public class FoodItemInCartRepository : IFoodItemInCartRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<FoodItemInCart> entities;
        string errorMessage = string.Empty;

        public FoodItemInCartRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<FoodItemInCart>();
        }
        public void Delete(FoodItemInCart foodItem)
        {
            if (foodItem == null)
            {
                throw new ArgumentNullException("foodItem");
            }
            entities.Remove(foodItem);
            context.SaveChanges();
        }

        public FoodItemInCart GetById(Guid id)
        {
            return entities
                .Include(f => f.FoodItem)
                .Include(f => f.ShoppingCart)
                .Where(f => f.Id.Equals(id))
                .FirstOrDefault();

        }

        public void Save(FoodItemInCart foodItem)
        {
            if (foodItem == null)
            {
                throw new ArgumentNullException("foodItem");
            }
            entities.Add(foodItem);
            context.SaveChanges();
        }

        public void Update(FoodItemInCart foodItem)
        {
            if (foodItem == null)
            {
                throw new ArgumentNullException("foodItem");
            }
            entities.Update(foodItem);
            context.SaveChanges();
        }
    }
}
