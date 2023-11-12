using HotelCollection.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelCollection.Repository.Interface
{
    public interface IPaymentSetupRepository
    {
        Task<bool> CreatePaymentSetupAsync(PaymentSetup paymentSetup);
        Task<IEnumerable<PaymentSetup>> GetPaymentSetupAsync();
        Task<PaymentSetup> GetPaymentSetupByIdAsync(int Id);
        Task<bool> UpdatePaymentSetupAsync(PaymentSetup paymentSetup);
        Task<bool> DeletePaymentSetupAsync(int Id);
    }
}
