using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using HotelCollection.Data;
using HotelCollection.Data.Entity;
using HotelCollection.Repository.Interface;

namespace HotelCollection.Repository.Repos
{
    public class LocalGovernmentAreaRepository : ILocalGovernmentAreaRepository
    {
        private readonly HotelCollectionContext _context;
        public LocalGovernmentAreaRepository(HotelCollectionContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateLocalGovernmentAreaAsync(LocalGovernmentArea lga)
        {
            if (lga != null)
            {

                await _context.AddAsync(lga);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<IEnumerable<LocalGovernmentArea>> GetLocalGovernmentAreaAsync()
        {
            return await _context.LocalGovernmentAreas.ToListAsync();
        }

        public async Task<LocalGovernmentArea> GetLocalGovernmentAreaByIdAsync(int Id)
        {
            return await _context.LocalGovernmentAreas.FindAsync(Id);
        }

        public async Task<bool> UpdateLocalGovernmentAreaAsync(LocalGovernmentArea lga)
        {
            if (lga != null)
            {
                _context.Update(lga);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> DeleteLocalGovernmentAreaAsync(int Id)
        {
            var category = await _context.LocalGovernmentAreas.Where(x => x.Id == Id).FirstAsync();
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
