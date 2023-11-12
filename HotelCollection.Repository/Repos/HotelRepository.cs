
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
   public class HotelRepository : IHotelRepository
    {
        private readonly HotelCollectionContext _context;

        public HotelRepository(HotelCollectionContext context )
        {
            _context = context;
        }

        public async Task<bool> CreateHotelAsync(Hotel Hotel)
        {
            if (Hotel != null)
            {

                await _context.AddAsync(Hotel);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<IEnumerable<Hotel>> GetHotelAsync()
        {
            return await _context.Hotels
                .Include(x=>x.Category)
                .Include(x => x.LocalGovernmentArea)
                .ToListAsync();
        }

        public async Task<Hotel> GetHotelByIdAsync(int Id)
        {
            return await _context.Hotels
                .FindAsync(Id);
        }

        public async Task<bool> UpdateHotelAsync(Hotel Hotel)
        {
            if (Hotel != null)
            {
                _context.Update(Hotel);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> DeleteHotelAsync(int Id)
        {
            var category = await _context.Hotels.Where(x=>x.Id==Id).FirstAsync();
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
