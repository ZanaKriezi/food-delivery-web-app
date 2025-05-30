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
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext _context;
        private DbSet<Review> _entities;
        string errorMessage = string.Empty;

        public ReviewRepository(ApplicationDbContext context)
        {
            _context = context;
            _entities = context.Set<Review>();
        }

        public void Insert(Review review)
        {

            if (review == null)
            {
                throw new ArgumentNullException("review");
            }
            _entities.Add(review);
            _context.SaveChanges();
        }
    }
}
