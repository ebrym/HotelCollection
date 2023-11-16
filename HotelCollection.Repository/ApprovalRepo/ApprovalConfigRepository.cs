using HotelCollection.Data;
using HotelCollection.Data.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using HotelCollection.Repository.ApprovalRepo;
using Microsoft.AspNetCore.Identity;

namespace HotelCollection.Repository.ApprovalRepo
{
   public class ApprovalConfigRepository : IApprovalConfigRepository
    {
        private readonly HotelCollectionContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public ApprovalConfigRepository(HotelCollectionContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<Role> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<bool> CreateApprovalConfigAsync(ApprovalConfig rfqApproval)
        {
            if (rfqApproval != null)
            {
                rfqApproval.IsActive = true;
                await _context.AddAsync(rfqApproval);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<IEnumerable<ApprovalConfig>> GetApprovalConfigAsync()
        {
            return await _context.ApprovalConfigs.ToListAsync();
        }

        public async Task<IEnumerable<ApprovalConfig>> GetFinalApprovalAsync()
        {
            return await _context.ApprovalConfigs. Where(x=> x.IsFinalLevel == true).ToListAsync();
        }

        public async Task<IEnumerable<ApprovalConfig>> GetFinalApprovalAsync(string PaymentType)
        {
            return await _context.ApprovalConfigs.Where(x => x.IsFinalLevel == true).ToListAsync();
        }


        public async Task<IEnumerable<ApprovalConfig>> CheckUserApprovalAsync(int UserId)
        {
            return await _context.ApprovalConfigs.Where(x => x.RoleId == UserId ).ToListAsync();
        }
        public async Task<IEnumerable<ApprovalConfig>> CheckUserApprovalAsync(int UserId, string PaymentType)
        {
            return await _context.ApprovalConfigs.Where(x => x.RoleId == UserId && x.PaymentType == PaymentType).ToListAsync();
        }

        public async Task<ApprovalConfig> GetApprovalConfigByIdAsync(int Id)
        {
            return await _context.ApprovalConfigs.FindAsync(Id);
        }

        public async Task<bool> UpdateApprovalConfigAsync(ApprovalConfig ApprovalConfig)
        {
            if (ApprovalConfig != null)
            {

                _context.Update(ApprovalConfig);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> DeleteApprovalConfigAsync(int Id)
        {
            var rfqApprovalConfig = _context.ApprovalConfigs.Where(x=>x.Id == Id).FirstOrDefault();
            if (rfqApprovalConfig != null)
            {
                 _context.Remove(rfqApprovalConfig);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
        public async Task<int> GetApprovalLevelByUserAsync(string email)
        {
            var users = await _userManager.FindByEmailAsync(email);
            if (users == null) 
            return 0;

            var role = await _userManager.GetRolesAsync(users);
            if (role == null)
                return 0;

            var defaultRole = role.FirstOrDefault();

            // get approval level for the role
            var approvalLevel = await _context.ApprovalConfigs.Where(x => x.RoleId == Convert.ToInt32(defaultRole)).FirstOrDefaultAsync();
            if (approvalLevel == null)
                return 0;

            return approvalLevel.ApprovalLevel;
        }
        public async Task<int> GetNextApprovalLevelAsync(int level, int roleId)
        {          
            // get approval level for the role
            var approvalLevel = await _context.ApprovalConfigs.Where(x => x.RoleId == roleId && x.ApprovalLevel == (level + 1)).FirstOrDefaultAsync();
            if(approvalLevel != null)
            {
                return approvalLevel.ApprovalLevel;
            }
            return 0;
        }
        public async Task<int> GetApprovalRoleByPaymentTypeAsync(string PaymentType)
        {
            var roles = await _context.Roles.Where(x => x.Name.Contains(PaymentType)).ToListAsync();        
            int roleId = 0;
            foreach (var item in roles)
            {
                var approvalLevel = await _context.ApprovalConfigs.Where(x => x.RoleId == item.Id && x.ApprovalLevel == 1 && x.PaymentType == PaymentType).FirstOrDefaultAsync();
                if (approvalLevel != null)
                {
                    roleId = approvalLevel.RoleId;
                    break;
                }
            }

            return roleId;
        }

        public async Task<string> GetUsersEmailByRoleAsync(int roleId)
        {
            var role = await _roleManager.Roles.Where(r => r.Id == roleId).FirstOrDefaultAsync();
            var users = await _userManager.GetUsersInRoleAsync(role.Name);
            if (users == null)
                return "";

            var userEmail = users.Select(e => e.Email).ToArray();
            var usernames = string.Join(",", userEmail);
            return usernames;
        }

        public async Task<string> GetApprovalEmailByUnitAsync(string PaymentType, string unit)
        {
            var users =  await _context.Users.Where(x => x.Unit.ToLower() == unit.ToLower()).ToListAsync();
            if (users == null)
                return "";

            var userEmail = users.Select(e => e.Email).ToArray();
            var usernames = string.Join(",", userEmail);
            return usernames;            
        }

        public async Task<string> GetApprovalEmailByPaymentTypeAsync(string PaymentType, string approvalLevel)
        {
            // approval role from config.
            var approvalRole = await _context.ApprovalConfigs.Where(x => x.ApprovalLevel == Convert.ToInt16(approvalLevel) && x.PaymentType == PaymentType && x.IsActive == true)
                                        .Select(x=>x.RoleId)
                                        .FirstOrDefaultAsync();
            // get the role name
            var role = await _roleManager.Roles.Where(r => r.Id == approvalRole).FirstOrDefaultAsync();
            // get users in that role
            var users = await _userManager.GetUsersInRoleAsync(role.Name);
            if (users == null)  return "";


            //var userEmail =  users.Where(x => x.PaymentType.ToLower() == PaymentType.ToLower()).ToList();
            var approvals = users.Select(e => e.Email).ToArray();
            var approvalEmails = string.Join(",", approvals);
            return approvalEmails;
        }
        public async Task<string> GetNextApprovalsAsync(int requestId)
        {
            // approval role from config.
            var request = await _context.PaymentSetups.Where(x => x.Id == requestId).FirstOrDefaultAsync();
            var approvalRole = await _context.ApprovalConfigs.Where(x => x.ApprovalLevel == Convert.ToInt16(request.CurrentApprovalLevel) && x.PaymentType == request.PaymentType.Type && x.IsActive == true)
                                        .Select(x => x.RoleId)
                                        .FirstOrDefaultAsync();
            // get the role name
            var role = await _roleManager.Roles.Where(r => r.Id == approvalRole).FirstOrDefaultAsync();
            // get users in that role
            var users = await _userManager.GetUsersInRoleAsync(role.Name);
            if (users == null) return "";


            //var userEmail = users.Where(x => x.PaymentType.ToLower() == request.PaymentType.ToLower()).ToList();
            var approvals = users.Select(e => e.Email).ToArray();
            var approvalEmails = string.Join(",", approvals);
            return approvalEmails;
        }
        public async Task<string> GetApprovalByPaymentTypeAsync(string PaymentType, string approvalLevel)
        {
            // approval role from config.
            var approvalRole = await _context.ApprovalConfigs.Where(x => x.ApprovalLevel == Convert.ToInt16(approvalLevel) && x.PaymentType == PaymentType && x.IsActive == true)
                                        .Select(x => x.RoleId)
                                        .FirstOrDefaultAsync();
            // get the role name
            var role = await _roleManager.Roles.Where(r => r.Id == approvalRole).FirstOrDefaultAsync();
            // get users in that role
            var users = await _userManager.GetUsersInRoleAsync(role.Name);
            if (users == null) return "";


            //var userEmail = users.Where(x => x.PaymentType.ToLower() == PaymentType.ToLower()).ToList();
            var approvals = users.Select(e => e.FullName).ToArray();
            var approvalEmails = string.Join(",", approvals);
            return approvalEmails;
        }
        public async Task<string> GetApprovalByPaymentTypeAsync(string PaymentType)
        {
            // approval role from config.
            var approvalRole = await _context.ApprovalConfigs.Where(x => x.PaymentType == PaymentType && x.IsActive == true)
                                        .Select(x => x.RoleId)
                                        .FirstOrDefaultAsync();
            // get the role name
            var role = await _roleManager.Roles.Where(r => r.Id == approvalRole).FirstOrDefaultAsync();
            // get users in that role
            var users = await _userManager.GetUsersInRoleAsync(role.Name);
            if (users == null) return "";


            //var userEmail = users.Where(x => x.PaymentType.ToLower() == PaymentType.ToLower()).ToList();
            var approvals = users.Select(e => e.FullName).ToArray();
            var approvalEmails = string.Join(",", approvals);
            return approvalEmails;
        }

        public async Task<string> GetUsersEmailByRoleAsync(string role)
        {
            var users = await _userManager.GetUsersInRoleAsync(role);
            if (users == null)
                return "";

            var userEmail = users.Select(e => e.Email).ToArray();
            var usernames = string.Join(",", userEmail);
            return usernames;
        }
        
        public async Task<bool> IsFinalApprovalsAsync(int requestId)
        {
            // approval role from config.
            var request = await _context.PaymentSetups.Where(x => x.Id == requestId).FirstOrDefaultAsync();
            var approvalConfig = await _context.ApprovalConfigs.Where(x => x.ApprovalLevel == Convert.ToInt16(request.CurrentApprovalLevel) && x.PaymentType == request.PaymentType.Type && x.IsActive == true)
                .FirstOrDefaultAsync();
            return approvalConfig.IsFinalLevel;
        }
    }
}
