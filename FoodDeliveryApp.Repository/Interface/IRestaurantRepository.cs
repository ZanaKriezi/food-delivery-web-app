using FoodDeliveryApp.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Repository.Interface
{
    public interface IRestaurantRepository
    {
        public Restaurant GetRestaurant(Guid? id);

        public IEnumerable<Restaurant> GetAll();

        public void Insert(Restaurant restaurant);
        public void Update(Restaurant restaurant);

        public void Delete(Restaurant restaurant);
    }
}
