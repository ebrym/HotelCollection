using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using HotelCollection.Repository.AccountRepo;
using HotelCollection.Repository.ApprovalRepo;
using HotelCollection.Data.Entity;
using static HotelCollection.Web.Enums.Enums;
using HotelCollection.Web.Models;

namespace HotelCollection.Web.Controllers
{
   // [Authorize]
    public class ApprovalConfigController : BaseController
    {
        private readonly IApprovalConfigRepository _ApprovalConfigRepository;
        private readonly IMapper _mapper;
        private readonly IAccountManager _accountManager;

        public ApprovalConfigController(IApprovalConfigRepository ApprovalConfigRepository, IMapper mapper, IAccountManager accountManager)
        {
            _ApprovalConfigRepository = ApprovalConfigRepository;
            _accountManager = accountManager;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var ApprovalConfigList = await _ApprovalConfigRepository.GetApprovalConfigAsync();

            List<ApprovalConfigModel> ApprovalConfigs = _mapper.Map<List<ApprovalConfigModel>>(ApprovalConfigList);
            foreach (var item in ApprovalConfigs)
            {
                var roleId = ApprovalConfigList.Where(u => u.Id == item.Id).FirstOrDefault();
                var role = await _accountManager.GetRoleByIdAsync(roleId.RoleId);
                item.Role = string.Join("| ", role.Name);

            }


            return View(ApprovalConfigs);
        }

        public async Task<IActionResult> Create()
        {
    
          
            var role = await _accountManager.GetRoles();

            var roleList = role.Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.Name 
            }).ToList();


            // var department = await _authService.DepartmentFromAD();
            // var departmentList = department.Select(a => new SelectListItem()
            // {
            //     Value = a.ToString(),
            //     Text = a.ToString()
            // }).Distinct()
            // .OrderBy(a=>a.Value)
            // .ToList();


            ViewBag.roles = roleList;
            //ViewBag.department = "";//departmentList;
            return View();


        }

        [HttpPost]
        public async Task<IActionResult> Create(ApprovalConfigModel ApprovalConfig)
        {
            if (ModelState.IsValid)
            {
                // to reload the list
                var role = await _accountManager.GetRoles();

                var roleList = role.Select(a => new SelectListItem()
                {
                    Value = a.Id.ToString(),
                    Text = a.Name
                }).ToList();
                ViewBag.roles = roleList;
                // var department = await _authService.DepartmentFromAD();
                // var departmentList = department.Select(a => new SelectListItem()
                // {
                //     Value = a.ToString(),
                //     Text = a.ToString()
                // }).Distinct()
                // .OrderBy(a => a.Value)
                // .ToList();

                //ViewBag.department = "";//departmentList;


                var userApprovalCheck = await _ApprovalConfigRepository.CheckUserApprovalAsync(ApprovalConfig.RoleId, ApprovalConfig.Department);
                if (userApprovalCheck.Count() > 0)
                {
                    Alert("Can not create multiple approval level for the selected  role!! Please try again.", Enums.Enums.NotificationType.error);
                    return View(ApprovalConfig);
                }

                var finalApprovalCheck = await _ApprovalConfigRepository.GetFinalApprovalAsync(ApprovalConfig.Department);
                if (finalApprovalCheck.Count() > 0)
                {
                    Alert("Cannot create new approval level because Final Approval has already been assigned for the role!! Please try again.", Enums.Enums.NotificationType.error);
                    return View(ApprovalConfig);
                }

                var mappedApprovalConfig = _mapper.Map<ApprovalConfig>(ApprovalConfig);
                var result = await _ApprovalConfigRepository.CreateApprovalConfigAsync(mappedApprovalConfig);

              
                if (result)
                {
                    Alert("Approval config created successfully.", Enums.Enums.NotificationType.success);
                    return RedirectToAction("Index");
                }
                else
                {
                    Alert("Some problems were encountered while trying to perform operation. </br> Please try again.", Enums.Enums.NotificationType.error);
                }

            }
            Alert("Some entry fields were are not validated. </br> Kindly, review all entry field.", Enums.Enums.NotificationType.error);
            return View(ApprovalConfig);
        }
        public async Task<IActionResult> Edit(int Id)
        {
            try
            {
                // to reload the list
                var role = await _accountManager.GetRoles();

                var roleList = role.Select(a => new SelectListItem()
                {
                    Value = a.Id.ToString(),
                    Text = a.Name
                }).ToList();
                ViewBag.roles = roleList;

                // var department = await _authService.DepartmentFromAD();
                // var departmentList = department.Select(a => new SelectListItem()
                // {
                //     Value = a.ToString(),
                //     Text = a.ToString()
                // }).Distinct()
                // .OrderBy(a => a.Value)
                // .ToList();

                ViewBag.department = "";//departmentList;

                var ApprovalConfig = await _ApprovalConfigRepository.GetApprovalConfigByIdAsync(Id);

                ApprovalConfigModel per = _mapper.Map<ApprovalConfigModel>(ApprovalConfig);
                return View(per);
            }
            catch (Exception ex)
            {
                Alert("Some problems were encountered while trying to perform operation. </br> Please try again.", Enums.Enums.NotificationType.error);
                return RedirectToAction("Index");
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ApprovalConfigModel ApprovalConfig)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    Alert("Some problems were encountered while trying to perform operation. Please try again.", Enums.Enums.NotificationType.error);
                    return View(ApprovalConfig);
                }
                if (ModelState.IsValid)
                {
                    // to reload the list
                    var role = await _accountManager.GetRoles();

                    var roleList = role.Select(a => new SelectListItem()
                    {
                        Value = a.Id.ToString(),
                        Text = a.Name
                    }).ToList();
                    ViewBag.roles = roleList;

                    // var department = await _authService.DepartmentFromAD();
                    // var departmentList = department.Select(a => new SelectListItem()
                    // {
                    //     Value = a.ToString(),
                    //     Text = a.ToString()
                    // }).Distinct()
                    // .OrderBy(a => a.Value)
                    // .ToList();

                    ViewBag.department = "";//departmentList;

                    var getApprovalConfig = await _ApprovalConfigRepository.GetApprovalConfigByIdAsync(ApprovalConfig.Id);
                    if (getApprovalConfig == null)
                    {
                        Alert("Invalid approval config. Please try again.", Enums.Enums.NotificationType.error);
                        return View(getApprovalConfig);
                    }

                    var finalApprovalCheck = await _ApprovalConfigRepository.GetFinalApprovalAsync(ApprovalConfig.Department);
                    if(ApprovalConfig.IsFinalLevel)
                    {
                        // check if final approval <> selected final
                        if (finalApprovalCheck.FirstOrDefault().Id != getApprovalConfig.Id)
                        {
                            Alert("Cannot create new Final Approval level for the selected role!! Please try again.", Enums.Enums.NotificationType.error);
                            return View(ApprovalConfig);
                        }

                    }
               

                    getApprovalConfig.RoleId = ApprovalConfig.RoleId;
                    getApprovalConfig.ApprovalLevel = ApprovalConfig.ApprovalLevel;
                    getApprovalConfig.ApprovalType = ApprovalConfig.ApprovalType;
                    getApprovalConfig.IsFinalLevel = ApprovalConfig.IsFinalLevel;

                    var result = await _ApprovalConfigRepository.UpdateApprovalConfigAsync(getApprovalConfig);

                    if (result)
                    {
                        Alert("Approval config updated successfully.", Enums.Enums.NotificationType.success);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        Alert("Some problems were encountered while trying to perform operation. Please try again.", Enums.Enums.NotificationType.error);
                        return View(ApprovalConfig);
                    }
                }
            }
            catch (Exception ex)
            {
                Alert("Some problems were encountered while trying to perform operation. </br> Please try again.", Enums.Enums.NotificationType.error);
               
            }
            return View(ApprovalConfig);
        }

        public async Task<IActionResult> Delete(int Id)
        {
            try
            { 
                var result = await _ApprovalConfigRepository.DeleteApprovalConfigAsync(Id);
            
                if (result)
                {
                    Alert("Approval Config deleted successfully.", Enums.Enums.NotificationType.success);
                    return RedirectToAction("Index");
                }
                else
                {
                    Alert("Some problems were encountered while trying to perform operation. Please try again.", Enums.Enums.NotificationType.error);
                    return View("Index");
                }
            }
            catch (Exception ex)
            {
                Alert("Some problems were encountered while trying to perform operation. Please try again.", Enums.Enums.NotificationType.error);
            }
            return View("Index");
        }

        
    }
}