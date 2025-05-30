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
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<Restaurant> entities;
        string errorMessage = string.Empty;

        public RestaurantRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<Restaurant>();
        }

        public Restaurant GetRestaurant(Guid? id)
        {
            return entities
                .Include(r => r.FoodItems)
                .Include(r => r.Reviews)
                .Include("FoodItems.Extras")
                .Include("FoodItems.Extras.Extra")
                .Include("Reviews.Customer")
                .Where(r => r.Id.Equals(id))
                .FirstOrDefault();
        }


        public IEnumerable<Restaurant> GetAll()
        {
            return entities.AsEnumerable();
        }

       
        public void Insert(Restaurant restaurant)
        {
            if (restaurant == null)
            {
                throw new ArgumentNullException("restaurant");
            }
            entities.Add(restaurant);
            context.SaveChanges();
        }

        public void Update(Restaurant restaurant)
        {
            if (restaurant == null)
            {
                throw new ArgumentNullException("restaurant");
            }
            entities.Update(restaurant);
            context.SaveChanges();
        }

        public void Delete(Restaurant restaurant)
        {
            if (restaurant == null)
            {
                throw new ArgumentNullException("restaurant");
            }
            entities.Remove(restaurant);
            context.SaveChanges();
        }

       

    }

}
