
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
        
        Task<IEnumerable<Requisition>> GetPendingApprovalAsync();
        Task<bool> CreateApprovalAsync(Requisition approval, bool isApproved);
        Task<IEnumerable<Approval>> GetApprovalCommentsAsync(int requestId);
        Task<Requisition> GetRequisitionApprovalDetailsAsync(int Id);

        Task<IEnumerable<Requisition>> GetApprovedRequestsAsync();
        Task<Requisition> GetRequisitionDetailsAsync(int Id);
        Task<bool> DisburseRequest(string Status,int requestId, int[] Id, decimal[] QuantityIssued, bool[] IsProcurement);
    }
}
