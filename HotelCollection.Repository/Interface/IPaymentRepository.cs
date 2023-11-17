using System.Threading.Tasks;
using HotelCollection.Data.Entity;

namespace HotelCollection.Repository.Interface;

public interface IPaymentRepository
{
   Task<PaymentSetup> GetPaymentDetailsByReferenceAsync(string reference);
}