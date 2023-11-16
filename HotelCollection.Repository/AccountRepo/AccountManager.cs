using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HotelCollection.Data;
using HotelCollection.Data.Entity;
using HotelCollection.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace HotelCollection.Repository.AccountRepo
{
    public class AccountManager : IAccountManager
    {
        private readonly HotelCollectionContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<Role> _roleManager;
       // private readonly ISMTPService _emailSender;
        private IConfiguration _config;



        public AccountManager(
            HotelCollectionContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<Role> roleManager,
           //  ISMTPService emailSender,
             IConfiguration config,
            IHttpContextAccessor httpAccessor)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
           // _emailSender = emailSender;
            _config = config;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ApplicationUser> GetUserByIdAsync(int userId)
        {
            return await _userManager.FindByIdAsync(userId.ToString());
        }

        public async Task<ApplicationUser> GetUserByUserNameAsync(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<IList<string>> GetUserRolesAsync(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsers()
        {
            return await _userManager.Users.ToListAsync();
        }
        public async Task<IEnumerable<ApplicationUser>> GetUsers(string user )
        {
            return await _userManager.Users
                .Where(x  => x.UserName == user || x.FirstName == user)
                .ToListAsync();
        }

        public async Task<IEnumerable<Role>> GetRoles()
        {
            return await _roleManager.Roles.ToListAsync();
        }



        public async Task<(ApplicationUser User, string[] Role)> GetUserAndRolesAsync(long userId)
        {
            var user = await _context.Users
                .Include(u => u.Roles)
                .Where(u => u.Id == userId)
                .SingleOrDefaultAsync();

            if (user == null)
                return (null, null);

            var userRoleIds = user.Roles.Select(r => r.RoleId).ToList();

            var roles = await _context.Roles
                .Where(r => userRoleIds.Contains(r.Id))
                .Select(r => r.Name)
                .ToArrayAsync();



            return (user, roles);
        }



        public async Task<List<(ApplicationUser User, string[])>> GetUsersAndRolesAsync()
        {
            IQueryable<ApplicationUser> usersQuery = _context.Users
                .Include(u => u.Roles)
                .OrderBy(u => u.UserName);

            //if (page != -1)
            //    usersQuery = usersQuery.Skip((page - 1) * pageSize);

            //if (pageSize != -1)
            //    usersQuery = usersQuery.Take(pageSize);

            var users = await usersQuery.ToListAsync();

            var userRoleIds = users.SelectMany(u => u.Roles.Select(r => r.RoleId)).ToList();

            var roles = await _context.Roles
                .Where(r => userRoleIds.Contains(r.Id))
                .ToArrayAsync();

            return users.Select(u => (u,
                roles.Where(r => u.Roles.Select(ur => ur.RoleId).Contains(r.Id)).Select(r => r.Name).ToArray()))
                .ToList();
        }


        public async Task<(bool Succeeded, string[] Error)> CreatePasswordlessUserAsync(ApplicationUser user, IEnumerable<string> roles)
        {
            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded)
                return (false, result.Errors.Select(e => e.Description).ToArray());


            user = await _userManager.FindByNameAsync(user.Email);

            try
            {
                result = await _userManager.AddToRolesAsync(user, roles.Distinct());
            }
            catch
            {
                await DeleteUserAsync(user);
                throw;
            }

            if (!result.Succeeded)
            {
                await DeleteUserAsync(user);
                return (false, result.Errors.Select(e => e.Description).ToArray());
            }

            return (true, new string[] { });
        }
        public async Task<bool> CreateUserAsync(ApplicationUser user, string password, string Role)
        {
            var result = await _userManager.CreateAsync(user, password);
            //if (!result.Succeeded)
            //    return (false);

            if (!result.Succeeded)
            {
                await DeleteUserAsync(user);
                return false;
            }


            user = await _userManager.FindByEmailAsync(user.Email);

            try
            {
                result = await _userManager.AddToRoleAsync(user, Role.ToUpper());



                //_context.SaveChanges();

            }
            catch
            {
                await DeleteUserAsync(user);
                throw;
            }

            if (!result.Succeeded)
            {
                await DeleteUserAsync(user);
                return false;
            }

            return (true);
        }
        public async Task<bool> CreateUserAsync(ApplicationUser user,  string Role)
        {
            var result = await _userManager.CreateAsync(user);
            //if (!result.Succeeded)
            //    return (false);

            if (!result.Succeeded)
            {
                await DeleteUserAsync(user);
                return false;
            }


           var newUser = await _userManager.FindByEmailAsync(user.Email);

            try
            {
                result = await _userManager.AddToRoleAsync(user, Role.ToUpper());
                
            }
            catch
            {
                await DeleteUserAsync(user);
                throw;
            }

            if (!result.Succeeded)
            {
                await DeleteUserAsync(user);
                return false;
            }

            return (true);
        }
        public async Task<bool> CreateUserAsync(ApplicationUser user)
        {
            


            user = await _userManager.FindByEmailAsync(user.Email);

            try
            {
                var result = await _userManager.CreateAsync(user);
                //if (!result.Succeeded)
                //    return (false);

                if (!result.Succeeded)
                {
                    await DeleteUserAsync(user);
                    return false;
                }

            }
            catch
            {
                await DeleteUserAsync(user);
                throw;
            }

          

            return (true);
        }


        public async Task<(bool Succeeded, string[] Error)> UpdateUserAsync(ApplicationUser user)
        {
            return await UpdateUserAsync(user, null);
        }


        public async Task<(bool Succeeded, string[] Error)> UpdateUserAsync(ApplicationUser user, IEnumerable<string> roles)
        {
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return (false, result.Errors.Select(e => e.Description).ToArray());


            if (roles != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var enumerable = roles as string[] ?? roles.ToArray();
                var rolesToRemove = userRoles.Except(enumerable).ToArray();
                var rolesToAdd = enumerable.Except(userRoles).Distinct().ToArray();

                if (rolesToRemove.Any())
                {
                    result = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                    if (!result.Succeeded)
                        return (false, result.Errors.Select(e => e.Description).ToArray());
                }

                if (rolesToAdd.Any())
                {
                    result = await _userManager.AddToRolesAsync(user, rolesToAdd);
                    if (!result.Succeeded)
                        return (false, result.Errors.Select(e => e.Description).ToArray());
                }
            }

            return (true, new string[] { });
        }


        public async Task<(bool Succeeded, string[] Error)> ResetPasswordAsync(ApplicationUser user, string newPassword)
        {
            string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
            if (!result.Succeeded)
                return (false, result.Errors.Select(e => e.Description).ToArray());

            return (true, new string[] { });
        }

        public async Task<(bool Succeeded, string[] Error)> UpdatePasswordAsync(ApplicationUser user, string currentPassword, string newPassword)
        {
            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            if (!result.Succeeded)
                return (false, result.Errors.Select(e => e.Description).ToArray());

            return (true, new string[] { });
        }

        public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        {
            if (!await _userManager.CheckPasswordAsync(user, password))
            {
                if (!_userManager.SupportsUserLockout)
                    await _userManager.AccessFailedAsync(user);

                return false;
            }

            return true;
        }

        public async Task<bool> IsEmailExistAsync(string email)
        {
            var checkEmail = await _userManager.FindByEmailAsync(email);
            if (checkEmail != null)
            {
                return true;
            }

            return false;
        }

        public async Task<(bool Succeeded, string[] Error)> DeleteUserAsync(long userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user != null)
                return await DeleteUserAsync(user);

            return (true, new string[] { });
        }


        public async Task<(bool Succeeded, string[] Error)> DeleteUserAsync(ApplicationUser user)
        {
            var result = await _userManager.DeleteAsync(user);
            return (result.Succeeded, result.Errors.Select(e => e.Description).ToArray());
        }
        
        public async Task<Role> GetRoleByIdAsync(long roleId)
        {
            return await _roleManager.FindByIdAsync(roleId.ToString());
        }


        public async Task<Role> GetRoleByNameAsync(string roleName)
        {
            return await _roleManager.FindByNameAsync(roleName);
        }


        public async Task<Role> GetRoleLoadRelatedAsync(string roleName)
        {
            var role = await _context.Roles
                .Include(r => r.Claims)
                .Include(r => r.Users)
                .Where(r => r.Name == roleName)
                .SingleOrDefaultAsync();

            return role;
        }


        public async Task<List<Role>> GetRolesLoadRelatedAsync(int page, int pageSize)
        {
            IQueryable<Role> rolesQuery = _context.Roles
                .Include(r => r.Claims)
                .Include(r => r.Users)
                .OrderBy(r => r.Name);

            if (page != -1)
                rolesQuery = rolesQuery.Skip((page - 1) * pageSize);

            if (pageSize != -1)
                rolesQuery = rolesQuery.Take(pageSize);

            var roles = await rolesQuery.ToListAsync();

            return roles;
        }


        public async Task<bool> CreateRoleAsync(Role role)
        {
            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
                return (false);

           

            return (true);
        }
        public async Task<bool> UpdateRoleAsync(Role role)
        {
            var result = await _roleManager.UpdateAsync(role);
            if (!result.Succeeded)
                return (false);



            return (true);
        }

        public async Task<bool> AssignUserRoleAsync(string Id, List<string> roles)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user == null)
                return false;


            // remove user from all roles
            var allroles = await GetRoles();
            //List<string> selectecRoles = new List<string>();

            foreach (var role in allroles)
            {
                var isUserinRole = await _userManager.IsInRoleAsync(user, role.Name);
                if (isUserinRole)
                {
                    await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
            }
            //var removeRoles = await _userManager.RemoveFromRolesAsync(user, allroles.Select(r => r.Name).ToArray());
            //if (!removeRoles.Succeeded)
            //    return false;

            var addRoleResult = await _userManager.AddToRolesAsync(user, roles.ToArray<string>());
            
            if (!addRoleResult.Succeeded)
                return (false);



            return (true);
        }
        
        public async Task<bool> TestCanDeleteRoleAsync(long roleId)
        {
            return !await _context.UserRoles.Where(r => r.RoleId == roleId).AnyAsync();
        }


        public async Task<(bool Succeeded, string[] Error)> DeleteRoleAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);

            if (role != null)
                return await DeleteRoleAsync(role);

            return (true, new string[] { });
        }


        public async Task<(bool Succeeded, string[] Error)> DeleteRoleAsync(Role role)
        {
            var result = await _roleManager.DeleteAsync(role);
            return (result.Succeeded, result.Errors.Select(e => e.Description).ToArray());
        }

        public async Task<string> GetUsersEmailByRoleAsync(int roleId)
        {
            var role = await _roleManager.Roles.Where(r => r.Id == roleId).FirstOrDefaultAsync();
            var users = await _userManager.GetUsersInRoleAsync(role.Name);
            if (users == null)
                return "";

            var userEmail = users.Select(e=>e.Email).ToArray();
            var usernames= string.Join(",", userEmail);
            return usernames;
        }
        public async Task<string> GetUsersEmailByRoleAsync(int roleId, string department)
        {
            var role = await _roleManager.Roles.Where(r => r.Id == roleId).FirstOrDefaultAsync();
            var users = await _userManager.GetUsersInRoleAsync(role.Name);
            if (users == null)
                return "";

            var userEmail = users.Where(x =>x.Department == department).Select(e => e.Email).ToArray();
            var usernames = string.Join(",", userEmail);
            return usernames;
        }
        public async Task<(int, bool, string)> GetApprovalLevelByUserAsync(string email)
        {
            int level = 0;
            bool isFinalApproval = false;
            string ApprovalLevelDepartment = "";
            try
            {
                //var users = await _userManager.FindByEmailAsync(email);
                var user = await _context.Users.Where(x => x.Email == email).FirstOrDefaultAsync();

                //var role = await _userManager.GetRolesAsync(user);
                var role = await _context.UserRoles.Where(x=>x.UserId==user.Id).Select(x=>x.RoleId).ToListAsync();
                // _roleManager.Roles.Where(r => r.Id == roleId).FirstOrDefaultAsync();

                //var defaultRole = role.FirstOrDefault();

                // get approval level for the role
                //string approvalLevel= "";
                foreach (var item in role)
                {
                 var apLevel = await _context.ApprovalConfigs.Where(x => x.RoleId == item).FirstOrDefaultAsync();
                
                    if(apLevel != null)
                    {
                        level = apLevel.ApprovalLevel;
                        isFinalApproval = apLevel.IsFinalLevel;
                        ApprovalLevelDepartment = apLevel.PaymentType;
                        break;
                    }
                }

                //// var approvalLevel = await _context.ApprovalConfigs.Where(x => x.RoleId.).FirstOrDefaultAsync();
                //level = approvalLevel.ApprovalLevel;
                //isFinalApproval = approvalLevel.IsFinalLevel;
                //ApprovalLevelDepartment = approvalLevel.Department;

            }
           catch (Exception ex)
            {
                return (level, isFinalApproval, ApprovalLevelDepartment);
            }
            return (level, isFinalApproval, ApprovalLevelDepartment);
        }

   

        public async Task<bool> IsUserInRoleExistAsync(string email, string role)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                var checkuserrole = await _userManager.IsInRoleAsync(user, role);

                if (!checkuserrole) return false;
            }
            catch
            {
                throw;
            }

            return true;
        }

        public async Task<bool> AddUserToRoleAsync(string email, string role)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return false;

         
                var isUserinRole = await _userManager.IsInRoleAsync(user, role);
                if (isUserinRole)
                    return (false);


            var addRoleResult = await _userManager.AddToRoleAsync(user, role);

            if (!addRoleResult.Succeeded)
                return (false);



            return (true);
        }

        public async Task<IList<string>> GetRolesAsync(ApplicationUser user)
        {
            var role = await _userManager.GetRolesAsync(user);
            return role;
        }

        public async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user;
        }

        public async Task<ApplicationUser> FindByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user;
        }
    }
}

//public async Task<(bool Succeeded, string[] Error)> UpdateRoleAsync(Role role, IEnumerable<string> claims)
//{
//    var enumerable = claims as string[] ?? claims.ToArray();

//        string[] invalidClaims = enumerable.Where(c => ApplicationPermissions.GetPermissionByValue(c) == null).ToArray();
//        if (invalidClaims.Any())
//            return (false, new[] { "The following claim types are invalid: " + string.Join(", ", invalidClaims) });



//    var result = await _roleManager.UpdateAsync(role);
//    if (!result.Succeeded)
//        return (false, result.Errors.Select(e => e.Description).ToArray());



//        var roleClaims = (await _roleManager.GetClaimsAsync(role)).Where(c => c.Type == CustomClaimTypes.Permission);
//        var roleClaims1 = roleClaims as Claim[] ?? roleClaims.ToArray();
//        var roleClaimValues = roleClaims1.Select(c => c.Value).ToArray();

//        var claimsToRemove = roleClaimValues.Except(enumerable).ToArray();
//        var claimsToAdd = enumerable.Except(roleClaimValues).Distinct().ToArray();

//        if (claimsToRemove.Any())
//        {
//            foreach (string claim in claimsToRemove)
//            {
//                result = await _roleManager.RemoveClaimAsync(role, roleClaims1.FirstOrDefault(c => c.Value == claim));
//                if (!result.Succeeded)
//                    return (false, result.Errors.Select(e => e.Description).ToArray());
//            }
//        }

//        if (claimsToAdd.Any())
//        {
//            foreach (string claim in claimsToAdd)
//            {
//                result = await _roleManager.AddClaimAsync(role, new Claim(CustomClaimTypes.Permission, ApplicationPermissions.GetPermissionByValue(claim)));
//                if (!result.Succeeded)
//                    return (false, result.Errors.Select(e => e.Description).ToArray());
//            }
//        }


//    return  (true, new string[] { });
//}