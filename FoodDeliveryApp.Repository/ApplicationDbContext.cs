using FoodDeliveryApp.Domain.DomainModels;
using FoodDeliveryApp.Domain.Email;
using FoodDeliveryApp.Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace FoodDeliveryApp.Repository
{
    public class ApplicationDbContext : IdentityDbContext<Customer>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<FoodItem> FoodItems { get; set; }
        public DbSet<Extra> Extras { get; set; }
        public DbSet<ExtraInFoodItem> ExtraInFoodItems { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<FoodItemInCart> FoodItemsInCart { get; set; }
        public DbSet<EmailMessage> EmailMessages { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<FoodItemInOrder> FoodItemsInOrder { get; set; }
        public DbSet<Review> Reviews { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure FoodItemInOrder
            modelBuilder.Entity<FoodItemInOrder>()
                .HasOne(fio => fio.Order)
                .WithMany(o => o.FoodItemsInOrder)
                .HasForeignKey(fio => fio.OrderId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

            // Configure Order
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Review
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Customer)
                .WithMany(c => c.Reviews)
                .HasForeignKey(r => r.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure ShoppingCart
            modelBuilder.Entity<ShoppingCart>()
                .HasOne(sc => sc.Customer)
                .WithOne(c => c.ShoppingCart)
                .HasForeignKey<ShoppingCart>(sc => sc.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Additional configurations can be added here as needed
        }

    }
}
