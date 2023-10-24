using HotelCollection.Data.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HotelCollection.Repository.Interface
{
    public interface IReportRepo
    {
        Task<IEnumerable<Requisition>> GetAll();
        Task<IEnumerable<Requisition>> GetByDepartmentRequisitionTypeAndDate(string department, string requisitionType, DateTime startDate, DateTime endDate);
        Task<IEnumerable<Requisition>> GetByProject(int projectId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<Requisition>> GetByDisbursedStatus(string disburseStatus, DateTime startDate, DateTime endDate);
        Task<IEnumerable<Requisition>> GetByApprovalStatus(string approvalStatus, DateTime startDate, DateTime endDate);
    }
}
