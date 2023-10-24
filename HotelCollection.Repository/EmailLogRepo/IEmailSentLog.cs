using HotelCollection.Data.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HotelCollection.Repository.Interface;

namespace HotelCollection.Repository.EmailLogRepo
{
   public interface IEmailSentLog
    {
        Task<bool> LogEmailAsync(EmailSentLog emailLog);
        Task<bool> LogEmailTransactionAsync(EmailSentLog emailLog);

        Task<bool> UpdateEmailAsync(EmailSentLog emailLog);

        Task<EmailSentLog> GetEmailSentByIdAsync(int Id);

        Task<IEnumerable<EmailSentLog>> GetEmailSentLogAsync();
    }
}
