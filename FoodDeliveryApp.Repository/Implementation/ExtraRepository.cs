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
    public class ExtraRepository : IExtraRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<Extra> entities;
        string errorMessage = string.Empty;

        public ExtraRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<Extra>();
        }

        public Extra Get(Guid extraId)
        {
            return entities.Where(e => e.Id.Equals(extraId))
                 .FirstOrDefault();
        }

        public Extra Insert(Extra extra)
        {
            if (extra == null)
            {
                throw new ArgumentNullException("extra");
            }
            entities.Add(extra);
            context.SaveChanges();

            return extra;
        }

        public void Remove(Extra e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("extra");
            }
            entities.Remove(e);
            context.SaveChanges();
        }

        public void Update(Extra extra)
        {
            if (extra == null)
            {
                throw new ArgumentNullException("extra");
            }
            entities.Update(extra);
            context.SaveChanges();
        }
    }
}
