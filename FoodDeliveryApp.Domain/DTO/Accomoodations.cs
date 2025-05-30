using FoodDeliveryApp.Domain.DomainModels;

namespace FoodDeliveryApp.Domain.DTO;

public class Accommodation : BaseEntity
{
    public string? Name { get; set; }
    public AccommodationType? Type { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public int? Stars { get; set; }
    public decimal? PricePerNight { get; set; }
    public List<string>? Amenities { get; set; }
    public string? ContactNumber { get; set; }
    public string? Website { get; set; }

    public string? ImageUrl { get; set; }
} 
public enum AccommodationType

{
    Hotel,
    Airbnb,
    Resort,
    Hostel,
    Apartment,
    Villa
}