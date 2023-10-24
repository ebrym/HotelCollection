using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using static HotelCollection.Web.Enums.Enums;
using AutoMapper;
using HotelCollection.Web.Models;
using Microsoft.AspNetCore.Identity;
using HotelCollection.Data.Entity;
using HotelCollection.Repository.AccountRepo;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HotelCollection.Web.Controllers
{
    public class AccountController : BaseController
    {
       
        private readonly IAccountManager _accountManager;
        private readonly IMapper _mapper;
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AccountController(
                                RoleManager<Role> roleManager,
                                UserManager<ApplicationUser> userManager,
                                SignInManager<ApplicationUser> signInManager,
                                IMapper mapper,
                                IAccountManager accountManager)
        {
            _accountManager = accountManager;
            _mapper = mapper;
           _roleManager = roleManager;
          _userManager = userManager;
          _signInManager = signInManager;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
       
 public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
      
                if (ModelState.IsValid)
                {
                    var user =  _userManager.Users.FirstOrDefault(m => m.Email.Trim() == loginViewModel.Username);
                
                    //var user = await _accountManager.GetUserByEmailAsync(loginViewModel.Email);
                    if (user == null)
                    {
                        ModelState.AddModelError("", "Email/password not found");
                        return View(loginViewModel);
                    }

                    var passwordIsCorrect = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);
                    if (passwordIsCorrect)
                    {
                        var roles = await _userManager.GetRolesAsync(user);

                        List<int> rolesId = new List<int>();
                        var claims = new List<Claim>();

                        foreach (var r in roles)
                        {
                            var roleId = _roleManager.Roles.Where(x => x.Name == r).First();
                            // claims.Add(new Claim("Roles", r));
                            rolesId.Add(roleId.Id);
                        }

                        claims.Add(new Claim("Email", user.Email));

                        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(user);
                        if ( claimsPrincipal?.Identity is ClaimsIdentity claimsIdentity)
                        {
                            claimsIdentity.AddClaims(claims);
                        }

                    var props = new AuthenticationProperties();
                    props.IsPersistent = true;

                    await _signInManager.Context.SignInAsync(
                       CookieAuthenticationDefaults.
                       AuthenticationScheme,
                       claimsPrincipal, props);

                    return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Email/password not found");
                        View(loginViewModel);
                    }
                }
            ModelState.AddModelError("", "Email/password not found");
            return  View(loginViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
           // return RedirectToAction("Login", "Account");

            try
            {
                // Removing Session
                //HttpContext.Session.Clear();

                // Removing Cookies
                CookieOptions option = new CookieOptions();
                if (Request.Cookies[".AspNetCore.Session"] != null)
                {
                    option.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Append(".AspNetCore.Session", "", option);
                }

                if (Request.Cookies["AuthenticationToken"] != null)
                {
                    option.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Append("AuthenticationToken", "", option);
                }

                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Login", "Account");
            }
            catch (Exception)
            {
                throw;
            }


        }


        #region"USER SECTION"
        public async Task<IActionResult> CreateUser()
        {
            var roles = await _accountManager.GetRoles();
            var roleList = roles.Select(a => new SelectListItem()
            {
                Value = a.Name,
                Text = a.Name
            }).ToList();
            ViewBag.roles = roleList;
            return View();
        }
        [HttpPost]
       
        public async Task<IActionResult> CreateUser(RegisterViewModel user)
        {
            if (ModelState.IsValid)
            {
                var roles = await _accountManager.GetRoles();
                var roleList = roles.Select(a => new SelectListItem()
                {
                    Value = a.Name,
                    Text = a.Name
                }).ToList();
                ViewBag.roles = roleList;

                if (await _accountManager.IsEmailExistAsync(user.Email))
                {
                    Alert("User already exist with the supplied email.", Enums.Enums.NotificationType.error);
                    return View(user);
                }

                var mappedUser = _mapper.Map<ApplicationUser>(user);
                mappedUser.UserName = user.Email;
                var result = await _accountManager.CreateUserAsync(mappedUser, user.Password, user.Role);

                if (result)
                {
                    Alert("Account created sucsessfully.", Enums.Enums.NotificationType.success);
                    return RedirectToAction("Users");
                }
                else
                {
                    Alert("User account could not be created. Please try again later.", Enums.Enums.NotificationType.error);

                    return View(user);
                }
            }
            Alert("Entries could not be validate. Please try again.", Enums.Enums.NotificationType.error);
            return View(user);
        }

        public async Task<IActionResult> Users()
        {

            var users = await _accountManager.GetUsers();

            List<UserViewModel> userList = _mapper.Map<List<UserViewModel>>(users);
            return View(userList);

        }

        public async Task<IActionResult> EditUser(int id)
        {
            try
            {
                var users = await _accountManager.GetUserByIdAsync(id);

                RegisterViewModel user = _mapper.Map<RegisterViewModel>(users);
                return View(user);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpPost]
        public async Task<IActionResult> EditUser(RegisterViewModel user)
        {

            if (ModelState.IsValid)
            {
                var getUser = await _accountManager.GetUserByIdAsync(user.Id);
                if (getUser == null)
                    return View(user);
                getUser.FirstName = user.FirstName;
                getUser.LastName = user.LastName;
                getUser.Department = user.Department;

                // var mappedUser = _mapper.Map<User>(user);
                getUser.SecurityStamp = Guid.NewGuid().ToString();
                var result = await _accountManager.UpdateUserAsync(getUser);

                if (result.Succeeded)
                {
                    Alert("Account updated sucsessfully.", Enums.Enums.NotificationType.success);
                    return RedirectToAction("Users");
                }
                else
                {
                    Alert("User account could not be updated. Please try again later.", Enums.Enums.NotificationType.error);
                    return View(user);
                }
            }
            Alert("Entries could not be validate. Please try again.", Enums.Enums.NotificationType.error);
            return View(user);
        }

        //[PermissionValidation("can_create_user", "can_assign_permission")]
        public async Task<IActionResult> AssignUserRole()
        {
            try
            {
                var roles = await _accountManager.GetRoles();
                //var users = await _accountManager.GetUsers();
                //var users = await _authService.FindUserByNameOrUsernameFromAD();  
                var userRoleVM = roles.Select(a => new SelectListItem()
                {
                    Value = a.Name,
                    Text = a.Name
                }).ToList();

               // List<UserRoleViewModel> userRoleVM = new List<UserRoleViewModel>();
                //foreach (var item in roles)
                //{
                //    userRoleVM.Add(new UserRoleViewModel { Role = item.Name });
                //}

                ViewBag.roles = userRoleVM;
                //ViewBag.users = userList;

                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpPost]
        //[PermissionValidation("can_create_user", "can_assign_permission")]
        public async Task<IActionResult> AssignUserRole(RegisterViewModel register)
        {
            try
            {

                if (!string.IsNullOrEmpty(register.Email))
                {
                    var roles = await _accountManager.GetRoles();
                    //var currentRoles = await _accountManager.GetUserAndRolesAsync(Convert.ToInt16(Id));

                  
                   
                    var userRoleVM = roles.Select(a => new SelectListItem()
                    {
                        Value = a.Name,
                        Text = a.Name
                    }).ToList();
                    ViewBag.roles = userRoleVM;
                    //check if user aready exist.
                    var user = await _accountManager.GetUserByEmailAsync(register.Email);
                    if(user == null)
                    {
                        // create user
                        ApplicationUser newUser = new ApplicationUser
                        {
                            DisplayName= register.FirstName + " " + register.LastName,
                            FirstName = register.FirstName,
                            LastName = register.LastName,
                            Email = register.Email,
                            Department = register.Department,
                            JobTitle=register.JobTitle,
                            //UserName=register.Username.Replace(".",""),
                            UserName = register.Username,
                            Unit = register.Unit,

                        };
                        // create user and add to role
                        var createUser = await _accountManager.CreateUserAsync(newUser, register.Role);
                        if(createUser)
                        {
                            Alert("Role assigned successfully.", Enums.Enums.NotificationType.success);
                            return RedirectToAction("UserRoles");
                        }
                        else
                        {
                            Alert("Could not assing role to user.", Enums.Enums.NotificationType.success);
                        }
                    }

                    // user already exist
                   else
                    {
                        //check if user is in role
                        var checkuserrole = await _accountManager.IsUserInRoleExistAsync(user.Email, register.Role);
                        if(!checkuserrole)
                        {
                            Alert("User aready belongs to the selected role. Please, select a different role.", Enums.Enums.NotificationType.error);

                            return View(register);
                        }
                        else
                        {
                            // add user to role
                            var addRole = await _accountManager.AddUserToRoleAsync(register.Email, register.Role);
                            if(addRole)
                            {
                                Alert("Role assigned successfully.", Enums.Enums.NotificationType.success);
                                return RedirectToAction("UserRoles");
                            }
                        }
                        //return View(register);
                    }    

                    
                }
                return View(register);
            }
            catch (Exception ex)
            {
               // throw ex;
                Alert("Some error occured. Please try again.", Enums.Enums.NotificationType.error);
                return View(register);
            }
            
        }

        [HttpPost]  
        public async Task<IActionResult> CreateUserRole(UserViewModel assignUser, List<UserRoleViewModel> UserRole)
        {

            if (ModelState.IsValid)
            {
                List<UserRoleViewModel> roles = new List<UserRoleViewModel>();
                if (UserRole != null)
                {
                    roles = UserRole.Where(x => x.SelectedRole == true).ToList();
                }
           

                List<string> selectecRoles = new List<string>();

                foreach (var role in roles)
                {
           
                    selectecRoles.Add(role.Role);
                }


                var result = await _accountManager.AssignUserRoleAsync(assignUser.Id.ToString(), selectecRoles);

                if (result)
                {
                    //await _userManager.AddToRoleAsync(user, registerViewModel.UserRoles);
                    Alert("Role(s) assigned to user succesfully.", Enums.Enums.NotificationType.success);
                    return RedirectToAction("UserRoles");
                }
                else
                {
                    Alert("Error assigning role(s). Please try again later.", Enums.Enums.NotificationType.error);
                    return View("EditAssignUserRole");
                }
            }
            Alert("Invalid entries. Please, try again.", Enums.Enums.NotificationType.info);
            return View("EditAssignUserRole");
        }


        public IActionResult GetUsers(string term)
        {
            if (!string.IsNullOrEmpty(term))
            {
                // var list = _authService.FindUserByNameOrUsernameFromAD(term)
                //     .ToList();

                return Json("");
            }
            else
            {
                return Json("");
            }
            
        }

        public async Task<IActionResult> UserRoles()
        {

            var users = await _accountManager.GetUsers();
           // var roles = await _accountManager.GetRoles();
           
            List<UserViewModel> userList = _mapper.Map<List<UserViewModel>>(users);


          


            foreach (var item in userList)
            {
                var user = users.Where(u=>u.Id==item.Id).FirstOrDefault();
                var role  = await _accountManager.GetRolesAsync(user);
                item.Role = string.Join("| ", role);

            }

            return View(userList);

        }


     
        //[PermissionValidation("can_create_user", "can_assign_permission")]
        public async Task<IActionResult> EditAssignUserRole(string Id)
        {
            try
            {

                if (!string.IsNullOrEmpty(Id))
                {
                    var roles = await _accountManager.GetRoles();
                    var users = await _accountManager.GetUsers();
                    //var currentRoles = await _accountManager.GetUserAndRolesAsync(Convert.ToInt16(Id));

                    var user = await _accountManager.FindByIdAsync(Id.ToString());
                    var currentRoles = await _accountManager.GetRolesAsync(user);

                    var userList = users.Select(a => new SelectListItem()
                    {
                        Value = a.Id.ToString(),
                        Text = a.FullName
                    }).ToList();

                    List<UserRoleViewModel> userRoleVM = new List<UserRoleViewModel>();
                    foreach (var item in roles)
                    {
                        var isRoleAssigned = currentRoles.Any(x => x.Contains(item.Name));
                        if (isRoleAssigned)
                        {
                            userRoleVM.Add(new UserRoleViewModel { SelectedRole = true, Role = item.Name });
                        }
                        else
                        {
                            userRoleVM.Add(new UserRoleViewModel { SelectedRole = false, Role = item.Name });
                        }
                        //userRoleVM.Add(new UserRoleViewModel { Role = item.Name });
                    }

                    ViewBag.roles = userRoleVM;
                    ViewBag.users = userList;

                    return View();



                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            Alert("Please select user.", Enums.Enums.NotificationType.error);
            return View();
        }
        #endregion


        #region"ROLE SECTION"
        [AllowAnonymous]
     
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
     
        public async Task<IActionResult> CreateRole(RoleViewModel role)
        {
            if (ModelState.IsValid)
            {
                var mappedRole = _mapper.Map<Role>(role);

                var result = await _accountManager.CreateRoleAsync(mappedRole);

                if (result)
                {
                    Alert("Role created successfully.", Enums.Enums.NotificationType.success);
                    return RedirectToAction("Roles");
                }
                else
                {
                    Alert("Error updating role. Please try again later.", Enums.Enums.NotificationType.error);
                    return View("AssignUserRole");
                }

            }
            Alert("Invalid entries. Please, try again.", Enums.Enums.NotificationType.info);
            return View(role);
        }

        public async Task<IActionResult> Roles()
        {

            var roles = await _accountManager.GetRoles();

            List<RoleViewModel> roleList = _mapper.Map<List<RoleViewModel>>(roles);
            return View(roleList);

        }

    
        public async Task<IActionResult> EditRole(int id)
        {
            try
            {
                var role = await _accountManager.GetRoleByIdAsync(id);

                RoleViewModel appRole = _mapper.Map<RoleViewModel>(role);
                return View(appRole);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpPost]
      
        public async Task<IActionResult> EditRole(RoleViewModel role)
        {

            if (ModelState.IsValid)
            {
                var getRole = await _accountManager.GetRoleByIdAsync(role.Id);
                if (getRole == null)
                    return View(role);
                getRole.Name = role.Name;

                var result = await _accountManager.UpdateRoleAsync(getRole);

                if (result)
                {
                    Alert("Role updated successfully.", Enums.Enums.NotificationType.success);
                    return RedirectToAction("Roles");
                }
                else
                {
                    Alert("Role could not be updated. Please try again later.", Enums.Enums.NotificationType.error);
                    return View();
                }
            }
            Alert("Invalid entries. Please, try again.", Enums.Enums.NotificationType.info);
            return View(role);
        }
        #endregion
    }
}
