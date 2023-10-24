using HotelCollection.Data.Entity;
using HotelCollection.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HotelCollection.Repository.Interface
{
   public  interface IRequisition
    {
        Task<bool> CreateRequisitionAsync(Requisition requisition);
        Task<IEnumerable<Requisition>> GetRequisitionsAsync();
        Task<IEnumerable<Requisition>> GetRequisitionsByUserAsync();
        Task<Requisition> GetRequisitionByIdAsync(int Id);
        Task<Requisition> GetRequisitionDetailsAsync(int Id);
        Task<IEnumerable<Requisition>> GetRequisitionsDashBoardByUserAsync();
        Task<bool> UpdateRequisitionAsync(Requisition requisition);
        Task<int> GetCurrentApprovalLevel(int Id);
        Task<bool> DeleteRequisitionAsync(int Id);
        Task<Requisition> GetRequisitionDetailsByUserAsync(int Id);
    }
}
