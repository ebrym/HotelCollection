
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using HotelCollection.Data;
using HotelCollection.Data.Entity;
using HotelCollection.Repository.Interface;

namespace HotelCollection.Repository.Repo
{
   public class HotelCategoryRepository : IHotelCategoryRepository
    {
        private readonly HotelCollectionContext _context;

        public HotelCategoryRepository(HotelCollectionContext context )
        {
            _context = context;
        }

        public async Task<bool> CreateHotelCategoryAsync(HotelCategory category)
        {
            if (category != null)
            {

                await _context.AddAsync(category);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<IEnumerable<HotelCategory>> GetHotelCategoryAsync()
        {
            return await _context.ItemCategories.ToListAsync();
        }

        public async Task<HotelCategory> GetHotelCategoryByIdAsync(int Id)
        {
            return await _context.ItemCategories.FindAsync(Id);
        }

        public async Task<bool> UpdateHotelCategoryAsync(HotelCategory category)
        {
            if (category != null)
            {
                _context.Update(category);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> DeleteHotelCategoryAsync(int Id)
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
