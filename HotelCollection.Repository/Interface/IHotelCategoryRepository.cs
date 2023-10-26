using HotelCollection.Data.Entity;
using HotelCollection.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HotelCollection.Repository.Interface
{
   public  interface IHotelCategoryRepository
    {
        Task<bool> CreateHotelCategoryAsync(HotelCategory category);
        Task<IEnumerable<HotelCategory>> GetHotelCategoryAsync();
        Task<HotelCategory> GetHotelCategoryByIdAsync(int Id);
        Task<bool> UpdateHotelCategoryAsync(HotelCategory category);
        Task<bool> DeleteHotelCategoryAsync(int Id);
    }
}
