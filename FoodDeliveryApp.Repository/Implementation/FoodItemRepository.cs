using FoodDeliveryApp.Domain.DomainModels;
using FoodDeliveryApp.Repository.Interface;
using Microsoft.EntityFrameworkCore;


namespace FoodDeliveryApp.Repository.Implementation
{
    public class FoodItemRepository : IFoodItemRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<FoodItem> entities;
        string errorMessage = string.Empty;

        public FoodItemRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<FoodItem>();
        }

        public void Delete(FoodItem foodItem)
        {
            if (foodItem == null)
            {
                throw new ArgumentNullException("foodItem");
            }
            entities.Remove(foodItem);
            context.SaveChanges();
        }

        public FoodItem GetById(Guid id)
        {
            return entities
                .Include(f => f.Extras)
                .Include("Extras.Extra")
                .Where(f => f.Id.Equals(id))
                .FirstOrDefault();
        }

        public void Save(FoodItem foodItem)
        {
            if (foodItem == null)
            {
                throw new ArgumentNullException("foodItem");
            }
            entities.Add(foodItem);
            context.SaveChanges();
        }

        public void Update(FoodItem foodItem)
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
