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
    public class PaymentSetupRepository: IPaymentSetupRepository
    {
        private readonly HotelCollectionContext _context;
        public PaymentSetupRepository(HotelCollectionContext context)
        {
            _context = context; 
        }

        public async Task<bool> CreatePaymentSetupAsync(PaymentSetup paymentSetup)
        {
            if (paymentSetup != null)
            {

                await _context.AddAsync(paymentSetup);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
        public async Task<IEnumerable<PaymentSetup>> GetPaymentSetupAsync()
        {
            return await _context.PaymentSetups.Include(x => x.Hotel)
                .Include(y => y.PaymentType)
                .ToListAsync();
        }
        public async Task<PaymentSetup> GetPaymentSetupByIdAsync(int Id)
        {
            return await _context.PaymentSetups.FindAsync(Id);
        }
        public async Task<bool> UpdatePaymentSetupAsync(PaymentSetup paymentSetup)
        {
            if (paymentSetup != null)
            {
                _context.Update(paymentSetup);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
        public async Task<bool> DeletePaymentSetupAsync(int Id)
        {
            var category = await _context.PaymentSetups.Where(x => x.Id == Id).FirstAsync();
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
