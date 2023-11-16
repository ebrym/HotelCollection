
using HotelCollection.Data.Entity;
using HotelCollection.Repository.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HotelCollection.Repository.Interface;

namespace HotelCollection.Repository.ApprovalRepo
{
   public  interface IApprovalRepository
    {
        //Task<IEnumerable<Requisition>> GetApprovalDueAsync();
        
        Task<IEnumerable<PaymentSetup>> GetPendingApprovalAsync();
        Task<bool> CreateApprovalAsync(PaymentSetup approval, string remarks, bool isApproved);
        Task<IEnumerable<Approval>> GetApprovalCommentsAsync(int requestId);
        Task<PaymentSetup> GetPaymentApprovalDetailsAsync(int Id);

        Task<IEnumerable<PaymentSetup>> GetApprovedRequestsAsync();
        Task<PaymentSetup> GetPaymentDetailsAsync(int Id);
        Task<bool> DisburseRequest(string Status,int requestId, int[] Id, decimal[] QuantityIssued, bool[] IsProcurement);
    }
}
