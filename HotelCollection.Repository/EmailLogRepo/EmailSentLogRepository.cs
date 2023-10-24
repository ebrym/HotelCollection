using HotelCollection.Data;
using HotelCollection.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HotelCollection.Repository.EmailLogRepo
{
   public class EmailSentLogRepository : IEmailSentLog
    {
        private readonly HotelCollectionContext _context;

        public EmailSentLogRepository(HotelCollectionContext context)
        {
            _context = context;
        }

        public async Task<bool> LogEmailAsync(EmailSentLog emailLog)
        {
            if (emailLog != null)
            {

                await _context.AddAsync(emailLog);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> LogEmailTransactionAsync(EmailSentLog emailLog)
        {
            if (emailLog != null)
            {

                await _context.AddAsync(emailLog);
                //await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async  Task<bool> UpdateEmailAsync(EmailSentLog emailLog)
        {
            if (emailLog != null)
            {
                _context.Update(emailLog);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<EmailSentLog> GetEmailSentByIdAsync(int Id)
        {
            return await _context.EmailSentLogs.FindAsync(Id);
        }
        public async Task<IEnumerable<EmailSentLog>> GetEmailSentLogAsync()
        {
            return await _context.EmailSentLogs.ToListAsync();
        }

    }
}
