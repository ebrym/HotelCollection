using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelCollection.Data;
using HotelCollection.Data.Entity;
using HotelCollection.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace HotelCollection.Repository.Repos;

public class PaymentTransactionRepository :IPaymentTransaction
{
    private readonly HotelCollectionContext _context;
           public PaymentTransactionRepository(HotelCollectionContext context)
           {
               _context = context; 
           }
   
           public async Task<bool> CreatePaymentTransactionAsync(PaymentTransaction payment)
           {
               if (payment != null)
               {
   
                   await _context.AddAsync(payment);
                   await _context.SaveChangesAsync();
                   return true;
               }
   
               return false;
           }
           public async Task<IEnumerable<PaymentTransaction>> GetPaymentTransactionAsync()
           {
               return await _context.PaymentTransactions
                   .Include(y => y.PaymentSetup)
                   .ToListAsync();
           }
           public async Task<PaymentTransaction> GetPaymentTransactionByIdAsync(int Id)
           {
               return await _context.PaymentTransactions.FindAsync(Id);
           }
           public async Task<PaymentTransaction> GetTransactionByReference(string reference)
           {
               return await _context.PaymentTransactions.FirstOrDefaultAsync(x=>x.Reference == reference);
           }
           public async Task<bool> UpdatePaymentTransactionAsync(PaymentTransaction paymentSetup)
           {
               if (paymentSetup != null)
               {
                   _context.Update(paymentSetup);
                   await _context.SaveChangesAsync();
                   return true;
               }
   
               return false;
           }
           public async Task<bool> DeletePaymentTransactionAsync(int Id)
           {
               var category = await _context.PaymentTransactions.Where(x => x.Id == Id).FirstAsync();
               if (category != null)
               {
                   _context.Remove(category);
                   await _context.SaveChangesAsync();
                   return true;
               }
   
               return false;
           }
}