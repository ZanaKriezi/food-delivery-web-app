using FoodDeliveryApp.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace FoodDeliveryApp.Web.Controllers.API;

public class PartnerData : Controller
{
        private readonly IAccommodationRepository _accommodationRepository;

        public PartnerData(IAccommodationRepository accommodationRepository)
        {
            _accommodationRepository = accommodationRepository;
        }

        public async Task<IActionResult> Index()
        {
            var accommodations = await _accommodationRepository.GetAccommodationsAsync();
            return View(accommodations);
        }
    }
