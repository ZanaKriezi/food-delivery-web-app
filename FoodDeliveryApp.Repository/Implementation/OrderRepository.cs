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
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<Order> entities;

        public OrderRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<Order>();
        }

        //public List<Order> GetAllOrders()
        //{
        //    return entities
        //        .Include(z => z.FoodItemsInOrder)
        //        .Include(z => z.Customer)
        //        .Include(z => z.Restaurant)
        //        .Include("FoodItemsInOrder.FoodItem")
        //        .ToList();
        //}

        public List<Order> GetAllOrders()
        {
            return entities
                .Select(o => new Order
                {
                    Id = o.Id,
                    Status = o.Status,
                    TotalAmount = o.TotalAmount,
                    OrderDate = o.OrderDate,
                    Customer = new Customer
                    {
                        Id = o.CustomerId,
                        Email = o.Customer.Email,
                        Name = o.Customer.Name,
                        Surname = o.Customer.Surname,
                        PhoneNumber = o.Customer.PhoneNumber,
                        Address = o.Customer.Address
                    },
                    Restaurant = new Restaurant
                    {
                        Id = o.RestaurantId,
                        Name = o.Restaurant.Name,
                        Address = o.Restaurant.Address,
                        BaseDeliveryFee = o.Restaurant.BaseDeliveryFee,
                        PhoneNumber = o.Restaurant.PhoneNumber,
                    },
                    FoodItemsInOrder = o.FoodItemsInOrder.Select(fi => new FoodItemInOrder
                    {
                        Quantity = fi.Quantity,
                        FoodItem = new FoodItem
                        {
                            Id = fi.FoodItem.Id,
                            Name = fi.FoodItem.Name,
                            Price = fi.FoodItem.Price,
                            Description = fi.FoodItem.Description,
                            Extras = fi.FoodItem.Extras.Select(e => new ExtraInFoodItem
                            {
                                Price = e.Price,
                                Extra = new Extra
                                {
                                    Name = e.Extra.Name,
                                    Id = e.Extra.Id
                                },
                                ExtraId = e.ExtraId,
                                FoodItemId = e.FoodItemId
                            }).ToList()
                        }
                    }).ToList()
                })
                .ToList();
        }

        public Order GetDetailsForOrder(BaseEntity id)
        {
            return entities
                .Select(o => new Order
                {
                    Id = o.Id,
                    Status = o.Status,
                    TotalAmount = o.TotalAmount,
                    OrderDate = o.OrderDate,
                    Customer = new Customer
                    {
                        Id =  o.CustomerId,
                        Email = o.Customer.Email,
                        Name = o.Customer.Name,
                        Surname = o.Customer.Surname,
                        PhoneNumber = o.Customer.PhoneNumber,
                        Address = o.Customer.Address
                    },
                    Restaurant = new Restaurant
                    {
                        Id = o.RestaurantId,
                        Name = o.Restaurant.Name,
                        Address = o.Restaurant.Address,
                        BaseDeliveryFee = o.Restaurant.BaseDeliveryFee,
                        PhoneNumber = o.Restaurant.PhoneNumber,
                        ImageUrl = o.Restaurant.ImageUrl,
                    },
                    FoodItemsInOrder = o.FoodItemsInOrder.Select(fi => new FoodItemInOrder
                    {
                        FoodItemId = fi.FoodItemId,
                        Quantity = fi.Quantity,
                        FoodItem = new FoodItem
                        {
                            Id = fi.FoodItemId,
                            Name = fi.FoodItem.Name,
                            Price = fi.FoodItem.Price,
                            Description = fi.FoodItem.Description,
                            ImageUrl = fi.FoodItem.ImageUrl,
                            Extras = fi.FoodItem.Extras.Select(e => new ExtraInFoodItem
                            {
                               
                                Price = e.Price,
                                Extra = new Extra
                                {                                 
                                    Name = e.Extra.Name,
                                    Id = e.Extra.Id
                                },
                                ExtraId = e.ExtraId,
                                FoodItemId = e.FoodItemId
                            }).ToList()
                        }
                    }).ToList()
                })
                .SingleOrDefaultAsync(z => z.Id == id.Id)
                .Result;
        }
    }
}
