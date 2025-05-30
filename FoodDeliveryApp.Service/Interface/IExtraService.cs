using FoodDeliveryApp.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Service.Interface
{
    public interface IExtraService
    {
        Extra CreateExtra(Extra extra);
        void RemoveExtraFromFoodItem(ExtraInFoodItem extraToRemove);
        void Update(ExtraInFoodItem existingExtra);
    }
}
