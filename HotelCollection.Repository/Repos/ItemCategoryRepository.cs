
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using HotelCollection.Data;
using HotelCollection.Data.Entity;
using HotelCollection.Repository.Interface;

namespace HotelCollection.Repository.PermissionRepo
{
   public class ItemCategoryRepository : IItemCategoryRepository
    {
        private readonly HotelCollectionContext _context;

        public ItemCategoryRepository(HotelCollectionContext context )
        {
            _context = context;
        }

        public async Task<bool> CreateItemCategoryAsync(ItemCategory category)
        {
            if (category != null)
            {

                await _context.AddAsync(category);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<IEnumerable<ItemCategory>> GetItemCategoryAsync()
        {
            return await _context.ItemCategories.ToListAsync();
        }

        public async Task<ItemCategory> GetItemCategoryByIdAsync(int Id)
        {
            return await _context.ItemCategories.FindAsync(Id);
        }

        public async Task<bool> UpdateItemCategoryAsync(ItemCategory category)
        {
            if (category != null)
            {
                _context.Update(category);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> DeleteItemCategoryAsync(int Id)
        {
            var category = await _context.ItemCategories.Where(x=>x.Id==Id).FirstAsync();
            if (category != null)
            {
                 _context.Remove(category);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
