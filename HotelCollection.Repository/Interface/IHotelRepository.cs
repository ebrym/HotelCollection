using System.Collections.Generic;
using System.Threading.Tasks;
using HotelCollection.Data.Entity;

namespace HotelCollection.Repository.Interface;

public interface IHotelRepository
{
    Task<bool> CreateHotelAsync(Hotel agent);
    Task<IEnumerable<Hotel>> GetHotelAsync();
    Task<Hotel> GetHotelByIdAsync(int Id);
    Task<bool> UpdateHotelAsync(Hotel agent);
    Task<bool> DeleteHotelAsync(int Id);
}