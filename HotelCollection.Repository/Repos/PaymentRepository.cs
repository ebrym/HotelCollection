using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HotelCollection.Data;
using HotelCollection.Data.Entity;
using HotelCollection.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace HotelCollection.Repository.Repos;

public class PaymentRepository : IPaymentRepository
{
    private readonly HotelCollectionContext _context;

    public PaymentRepository(HotelCollectionContext context )
    {
        _context = context;
    }
        public async Task<PaymentSetup> GetPaymentDetailsByReferenceAsync(string reference)
        {
             var userReq = await _context.PaymentSetups.Where(x => x.ReferenceNo == reference  && 
                                                         x.ApprovalStatus == "Completed" && 
                                                         x.IsDeleted == false)
                 .Include(x=> x.Hotel)
                 .ThenInclude(x=>x.LocalGovernmentArea)
                 .Include(x=> x.Hotel)
                 .ThenInclude(x=>x.Category)
                 .Include(x=> x.PaymentType)
                                                     .FirstOrDefaultAsync();
            return userReq;
        }
        
}