
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
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly HotelCollectionContext _context;

        public AuditLogRepository(HotelCollectionContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAuditLogAsync(AuditLog category)
        {
            if (category != null)
            {

                await _context.AddAsync(category);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

 

        public async Task<IEnumerable<AuditLog>> GetAuditLogAsync()
        {
            return await _context.AuditLogs.ToListAsync();
        }

        public async Task<AuditLog> GetAuditLogByIdAsync(int Id)
        {
            return await _context.AuditLogs.FindAsync(Id);
        }

        public async Task<bool> UpdateAuditLogAsync(AuditLog category)
        {
            if (category != null)
            {
                _context.Update(category);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> DeleteAuditLogAsync(int Id)
        {
            var category = await _context.ItemCategories.Where(x => x.Id == Id).FirstAsync();
            if (category != null)
            {
                _context.Remove(category);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }




        public async Task<IEnumerable<AuditLog>> GetAuditLogyByUserAsync(string userEmail)
        {

            var userLog = await _context.AuditLogs.Where(x => x.UserEmail == userEmail)
                                                .OrderByDescending(x => x.LoggedInAt)
                                                .ToListAsync();

            return userLog;
        }

        public bool CreateAudit(AuditLog category)
        {
            if (category != null)
            {

                _context.Add(category);
                _context.SaveChanges();
                return true;
            }

            return false;
        }
    }
}
