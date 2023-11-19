using System.Collections.Generic;
using System.Threading.Tasks;
using HotelCollection.Data.Entity;

namespace HotelCollection.Repository.Interface;

public interface IPaymentTransaction
{
    Task<PaymentTransaction> GetTransactionByReference(string paymentReference);
    
    Task<bool> CreatePaymentTransactionAsync(PaymentTransaction paymentSetup);
    Task<IEnumerable<PaymentTransaction>> GetPaymentTransactionAsync();
    Task<PaymentTransaction> GetPaymentTransactionByIdAsync(int Id);
    Task<bool> UpdatePaymentTransactionAsync(PaymentTransaction paymentSetup);
    Task<bool> DeletePaymentTransactionAsync(int Id);
}