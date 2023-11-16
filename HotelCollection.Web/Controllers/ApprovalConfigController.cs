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
using HotelCollection.Repository.Interface;
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
        private readonly IPaymentTypeRepository _paymentType;

        public ApprovalConfigController(IApprovalConfigRepository ApprovalConfigRepository, 
            IMapper mapper, IAccountManager accountManager,
            IPaymentTypeRepository paymentType)
        {
            _ApprovalConfigRepository = ApprovalConfigRepository;
            _accountManager = accountManager;
            _mapper = mapper;
            _paymentType = paymentType;
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


             var paymentType = await _paymentType.GetPaymentTypeAsync();
            var paymentTypeList = paymentType.Select(a => new SelectListItem()
            {
                Value = a.Type.ToString(),
                Text = a.Type.ToString()
            }).Distinct()
            .OrderBy(a=>a.Value)
            .ToList();


            ViewBag.roles = roleList;
            ViewBag.paymentType = paymentTypeList;
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
                var paymentType = await _paymentType.GetPaymentTypeAsync();
                var paymentTypeList = paymentType.Select(a => new SelectListItem()
                    {
                        Value = a.Type.ToString(),
                        Text = a.Type.ToString()
                    }).Distinct()
                    .OrderBy(a=>a.Value)
                    .ToList();


                
                ViewBag.paymentType = paymentTypeList;


                var userApprovalCheck = await _ApprovalConfigRepository.CheckUserApprovalAsync(ApprovalConfig.RoleId, ApprovalConfig.PaymentType);
                if (userApprovalCheck.Count() > 0)
                {
                    Alert("Can not create multiple approval level for the selected  role!! Please try again.", Enums.Enums.NotificationType.error);
                    return View(ApprovalConfig);
                }

                var finalApprovalCheck = await _ApprovalConfigRepository.GetFinalApprovalAsync(ApprovalConfig.PaymentType);
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

                var paymentType = await _paymentType.GetPaymentTypeAsync();
                var paymentTypeList = paymentType.Select(a => new SelectListItem()
                    {
                        Value = a.Type.ToString(),
                        Text = a.Type.ToString()
                    }).Distinct()
                    .OrderBy(a=>a.Value)
                    .ToList();

                
                ViewBag.paymentType = paymentTypeList;

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

                    var paymentType = await _paymentType.GetPaymentTypeAsync();
                    var paymentTypeList = paymentType.Select(a => new SelectListItem()
                        {
                            Value = a.Type.ToString(),
                            Text = a.Type.ToString()
                        }).Distinct()
                        .OrderBy(a=>a.Value)
                        .ToList();

                    
                    ViewBag.paymentType = paymentTypeList;

                    var getApprovalConfig = await _ApprovalConfigRepository.GetApprovalConfigByIdAsync(ApprovalConfig.Id);
                    if (getApprovalConfig == null)
                    {
                        Alert("Invalid approval config. Please try again.", Enums.Enums.NotificationType.error);
                        return View(ApprovalConfig);
                    }

                    var finalApprovalCheck = await _ApprovalConfigRepository.GetFinalApprovalAsync(ApprovalConfig.PaymentType);
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
                    getApprovalConfig.PaymentType = ApprovalConfig.PaymentType;
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