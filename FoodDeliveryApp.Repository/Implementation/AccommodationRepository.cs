using Dapper;
using FoodDeliveryApp.Domain.DTO;
using FoodDeliveryApp.Repository.Interface;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace FoodDeliveryApp.Repository.Implementation;

public class AccommodationRepository : IAccommodationRepository
{
    private readonly string _connectionString;

    public AccommodationRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("AzureMySQL");
    }

    public async Task<List<Accommodation>> GetAccommodationsAsync()
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            string query = "SELECT Name, Type, Address, City, Country, Stars, PricePerNight, ContactNumber, Website, ImageUrl FROM Accommodations";

            var accommodations = await connection.QueryAsync<Accommodation>(query);
            return accommodations.AsList();
        }
    }
}