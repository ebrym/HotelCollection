using HotelCollection.Data;
using HotelCollection.Data.Entity;
using HotelCollection.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelCollection.Repository.Repos
{
    public class HotelCategoryFeeRepository : IHotelCategoryFeeRepository
    {
        private readonly HotelCollectionContext _context;
        public HotelCategoryFeeRepository(HotelCollectionContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateHotelCategoryFeeAsync(HotelCategoryFee HotelCategoryFee)
        {
            if (HotelCategoryFee != null)
            {

                await _context.AddAsync(HotelCategoryFee);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
        public async Task<IEnumerable<HotelCategoryFee>> GetHotelCategoryFeeAsync()
        {
            return await _context.HotelCategoryFees
                .Include(x => x.Category)
                .ToListAsync();
        }
        public async Task<HotelCategoryFee> GetHotelCategoryFeeByIdAsync(int Id)
        {
            return await _context.HotelCategoryFees
                .FindAsync(Id);
        }
        public async Task<bool> UpdateHotelCategoryFeeAsync(HotelCategoryFee HotelCategoryFee)
        {
            if (HotelCategoryFee != null)
            {
                _context.Update(HotelCategoryFee);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
        public async Task<bool> DeleteHotelCategoryFeeAsync(int Id)
        {
            var category = await _context.HotelCategoryFees.Where(x => x.Id == Id).FirstAsync();
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
