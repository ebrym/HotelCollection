using HotelCollection.Data.Entity;
using HotelCollection.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HotelCollection.Repository.Interface
{
   public  interface IItemCategoryRepository
    {
        Task<bool> CreateItemCategoryAsync(ItemCategory category);
        Task<IEnumerable<ItemCategory>> GetItemCategoryAsync();
        Task<ItemCategory> GetItemCategoryByIdAsync(int Id);
        Task<bool> UpdateItemCategoryAsync(ItemCategory category);
        Task<bool> DeleteItemCategoryAsync(int Id);
    }
}
