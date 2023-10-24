using HotelCollection.Data.Entity;
using HotelCollection.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HotelCollection.Repository.Interface
{
   public  interface IAuditLogRepository
    {
        Task<bool> CreateAuditLogAsync(AuditLog category);
       bool CreateAudit(AuditLog category);
        Task<IEnumerable<AuditLog>> GetAuditLogAsync();
        Task<AuditLog> GetAuditLogByIdAsync(int Id);
        Task<IEnumerable<AuditLog>> GetAuditLogyByUserAsync(string useremail);
    }
}
