using HotelCollection.Data.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HotelCollection.Repository.Interface;

namespace HotelCollection.Repository.ApprovalRepo
{
   public  interface IApprovalConfigRepository
    {
        Task<bool> CreateApprovalConfigAsync(ApprovalConfig config);
        Task<IEnumerable<ApprovalConfig>> GetApprovalConfigAsync();
        Task<ApprovalConfig> GetApprovalConfigByIdAsync(int Id);
        Task<bool> UpdateApprovalConfigAsync(ApprovalConfig approvalConfig);
        Task<bool> DeleteApprovalConfigAsync(int Id);
        Task<IEnumerable<ApprovalConfig>> GetFinalApprovalAsync();
        Task<IEnumerable<ApprovalConfig>> GetFinalApprovalAsync(string department);
        Task<IEnumerable<ApprovalConfig>> CheckUserApprovalAsync(int UserId);
        Task<IEnumerable<ApprovalConfig>> CheckUserApprovalAsync(int UserId, string department);
        Task<int> GetApprovalLevelByUserAsync(string email);
        Task<int> GetApprovalRoleByDepartmentAsync(string department);
        Task<string> GetApprovalEmailByUnitAsync(string department, string unit);
        Task<string> GetApprovalEmailByDepartmentAsync(string department, string approvalLevel);
        Task<string> GetNextApprovalsAsync(int requestId);
        Task<int> GetNextApprovalLevelAsync(int level, int roleId);
        Task<string> GetApprovalByDepartmentAsync(string department, string approvalLevel);
        Task<string> GetApprovalByDepartmentAsync(string department);
        Task<string> GetUsersEmailByRoleAsync(int roleId);
        Task<string> GetUsersEmailByRoleAsync(string role);

    }
}
