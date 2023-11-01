using HotelCollection.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelCollection.Repository.Interface
{
    public interface IPaymentTypeRepository
    {
        Task<bool> CreatePaymentTypeAsync(PaymentType paymentType);
        Task<IEnumerable<PaymentType>> GetPaymentTypeAsync();
        Task<PaymentType> GetPaymentTypeByIdAsync(int Id);
        Task<bool> UpdatePaymentTypeAsync(PaymentType paymentType);
        Task<bool> DeletePaymentTypeAsync(int Id);
    }
}
