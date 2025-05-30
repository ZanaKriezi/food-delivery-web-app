using FoodDeliveryApp.Domain.DomainModels;
using FoodDeliveryApp.Repository.Interface;
using FoodDeliveryApp.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Service.Implementation
{
    public class ExtraService : IExtraService
    {
        private readonly IExtraRepository extraRepository;

        public ExtraService(IExtraRepository _extraRepository)
        {
            extraRepository = _extraRepository;
        }

        public Extra CreateExtra(Extra extra)
        {
            return extraRepository.Insert(extra);
        }

        public void RemoveExtraFromFoodItem(ExtraInFoodItem extraToRemove)
        {
            Extra e = extraRepository.Get(extraToRemove.ExtraId);
            extraRepository.Remove(e);
        }

        public void Update(ExtraInFoodItem existingExtra)
        {
            extraRepository.Update(existingExtra.Extra);
        }
    }
}
