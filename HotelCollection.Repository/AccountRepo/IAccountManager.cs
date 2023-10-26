using System.Collections.Generic;
using System.Threading.Tasks;
using HotelCollection.Data.Entity;
using HotelCollection.Repository.Interface;

namespace HotelCollection.Repository.AccountRepo
{
    public interface IAccountManager
    {
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
        Task<bool> CreateRoleAsync(Role role);
        Task<bool> UpdateRoleAsync(Role role);
        Task<(bool Succeeded, string[] Error)> CreatePasswordlessUserAsync(ApplicationUser user, IEnumerable<string> roles);
        Task<bool> CreateUserAsync(ApplicationUser user, string password, string Role);
        Task<bool> CreateUserAsync(ApplicationUser user,  string Role);
        Task<bool> CreateUserAsync(ApplicationUser user);
        Task<(bool Succeeded, string[] Error)> DeleteRoleAsync(Role role);
        Task<(bool Succeeded, string[] Error)> DeleteRoleAsync(string roleName);
        Task<(bool Succeeded, string[] Error)> DeleteUserAsync(long userId);
        Task<(bool Succeeded, string[] Error)> DeleteUserAsync(ApplicationUser user);
        Task<IEnumerable<ApplicationUser>> GetUsers();
        Task<IEnumerable<ApplicationUser>> GetUsers(string user);
        Task<Role> GetRoleByIdAsync(long roleId);
        Task<Role> GetRoleByNameAsync(string roleName);
        Task<Role> GetRoleLoadRelatedAsync(string roleName);
        Task<List<Role>> GetRolesLoadRelatedAsync(int page, int pageSize);
        Task<(ApplicationUser User, string[] Role)> GetUserAndRolesAsync(long userId);        
        Task<ApplicationUser> GetUserByEmailAsync(string email);
        Task<ApplicationUser> GetUserByIdAsync(int userId);
        Task<ApplicationUser> GetUserByUserNameAsync(string userName);
        Task<IList<string>> GetUserRolesAsync(ApplicationUser user);
        Task<(bool Succeeded, string[] Error)> ResetPasswordAsync(ApplicationUser user, string newPassword);
        Task<IEnumerable<Role>> GetRoles();
        Task<bool> TestCanDeleteRoleAsync(long roleId);
        Task<bool> IsEmailExistAsync(string email);
        Task<bool> IsUserInRoleExistAsync(string email, string role);
        Task<(bool Succeeded, string[] Error)> UpdatePasswordAsync(ApplicationUser user, string currentPassword, string newPassword);
       // Task<(bool Succeeded, string[] Error)> UpdateRoleAsync(Role role, IEnumerable<string> claims);
        Task<(bool Succeeded, string[] Error)> UpdateUserAsync(ApplicationUser user);
        Task<(bool Succeeded, string[] Error)> UpdateUserAsync(ApplicationUser user, IEnumerable<string> roles);
        Task<bool> AssignUserRoleAsync(string Id, List<string> roles);
        Task<bool> AddUserToRoleAsync(string Id, string roles);
        //Task<List<(ApplicationUser User, string[] Roles)>> GetUsersAndRolesAsync();
        Task<List<(ApplicationUser User, string[])>> GetUsersAndRolesAsync();

        Task<string> GetUsersEmailByRoleAsync(int roleId);
        Task<string> GetUsersEmailByRoleAsync(int roleId, string Department);
        Task<(int,bool, string)> GetApprovalLevelByUserAsync(string email);
        Task<IList<string>> GetRolesAsync(ApplicationUser user);
        Task<ApplicationUser> FindByEmailAsync(string email);
        Task<ApplicationUser> FindByIdAsync(string userId);
        //Task<List<UserViewModel>> GetUsersAndRolesAsync(int page, int pageSize, string sortOrder, string sortValue);

    }
}