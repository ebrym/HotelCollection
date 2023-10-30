using HotelCollection.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelCollection.Repository.Interface
{
    public interface IHotelCategoryFeeRepository
    {
        Task<bool> CreateHotelCategoryFeeAsync(HotelCategoryFee hotelCategoryFee);
        Task<IEnumerable<HotelCategoryFee>> GetHotelCategoryFeeAsync();
        Task<HotelCategoryFee> GetHotelCategoryFeeByIdAsync(int Id);
        Task<bool> UpdateHotelCategoryFeeAsync(HotelCategoryFee hotelCategoryFee);
        Task<bool> DeleteHotelCategoryFeeAsync(int Id);
    }
}
