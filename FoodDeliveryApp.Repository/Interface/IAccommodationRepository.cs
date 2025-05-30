using FoodDeliveryApp.Domain.DTO;

namespace FoodDeliveryApp.Repository.Interface;

public interface IAccommodationRepository
{
    Task<List<Accommodation>> GetAccommodationsAsync();
}