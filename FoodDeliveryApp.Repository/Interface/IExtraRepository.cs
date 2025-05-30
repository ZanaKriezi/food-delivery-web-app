using FoodDeliveryApp.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Repository.Interface
{
    public interface IExtraRepository
    {
        Extra Get(Guid extraId);
        Extra Insert(Extra extra);
        void Remove(Extra e);
        void Update(Extra extra);
    }
}
