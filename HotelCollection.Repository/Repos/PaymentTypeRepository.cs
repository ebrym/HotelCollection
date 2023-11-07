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
    public class PaymentTypeRepository: IPaymentTypeRepository
    {
        private readonly HotelCollectionContext _context;
        public PaymentTypeRepository(HotelCollectionContext context)
        {
            _context = context;
        }
        public async Task<bool> CreatePaymentTypeAsync(PaymentType PaymentType)
        {
            if (PaymentType != null)
            {

                await _context.AddAsync(PaymentType);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<IEnumerable<PaymentType>> GetPaymentTypeAsync()
        {
            return await _context.PaymentTypes
                .ToListAsync();
        }
        public async Task<PaymentType> GetPaymentTypeByIdAsync(int Id)
        {
            return await _context.PaymentTypes
                .FindAsync(Id);
        }
        public async Task<bool> UpdatePaymentTypeAsync(PaymentType PaymentType)
        {
            if (PaymentType != null)
            {
                _context.Update(PaymentType);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
        public async Task<bool> DeletePaymentTypeAsync(int Id)
        {
            var paymentType = await _context.PaymentTypes.Where(x => x.Id == Id).FirstAsync();
            if (paymentType != null)
            {
                _context.Remove(paymentType);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
