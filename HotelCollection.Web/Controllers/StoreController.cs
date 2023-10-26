using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using HotelCollection.Data.Entity;
using HotelCollection.Infrastructure.Interface;
using HotelCollection.Infrastructure.Models;
using HotelCollection.Web.Models;
using HotelCollection.Repository.AccountRepo;
using HotelCollection.Repository.ApprovalRepo;
using HotelCollection.Repository.Interface;
using static HotelCollection.Web.Enums.Enums;

namespace HotelCollection.Web.Controllers
{
   // [Authorize]
    public class StoreController :  BaseController
    {
        private readonly IHotelCategoryRepository _HotelCategoryRepository;
        private readonly IMapper _mapper;
        private readonly IProjectService _projectService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IRequisition _requisitionRepository;
        private readonly IApprovalRepository _approvalRepository;
        private IHostingEnvironment _hostingEnv;
        private readonly IAccountManager _accountManager;
        private IViewRenderService _viewRender;
        private readonly ISMTPService _emailSender;
        private readonly IApprovalConfigRepository _ApprovalConfigRepository;
        private IConfiguration _config;

        public StoreController(IHotelCategoryRepository HotelCategoryRepository, 
                                     IHttpContextAccessor contextAccessor, 
                                     IRequisition requisitionRepository, 
                                     IProjectService projectService,
                                     IMapper mapper,
                                     IConfiguration config,
                                     IApprovalRepository approvalRepository,
                                     IApprovalConfigRepository ApprovalConfigRepository,
                                     IAccountManager accountManager,
                                     IHostingEnvironment hostingEnv,
                                     IViewRenderService viewRender, 
                                     ISMTPService emailSender)
        {
            _HotelCategoryRepository = HotelCategoryRepository;
            _projectService = projectService;
            _mapper = mapper;
            _contextAccessor = contextAccessor;
            _hostingEnv = hostingEnv;
            _accountManager = accountManager;
            _viewRender = viewRender;
            _ApprovalConfigRepository = ApprovalConfigRepository;
            _approvalRepository = approvalRepository;
            _emailSender = emailSender;
            _config = config;
            _requisitionRepository = requisitionRepository;
    }
        public async Task<IActionResult> Index()
        {
            var myRequestList = await _approvalRepository.GetApprovedRequestsAsync();


            List<RequisitionModel> myRequests = _mapper.Map<List<RequisitionModel>>(myRequestList);

            foreach (var request in myRequests)
            {
                List<ItemsModel> items = new List<ItemsModel>();

                var item = myRequestList.Where(x => x.Id == request.Id)                                        
                                        .FirstOrDefault();
                var newItems = item.RequisitionDetails.Select(u => new ItemsModel {
                    HotelCategoryId = u.HotelCategoryId,
                    ItemDescription = u.ItemDescription,
                    HotelCategory =  _HotelCategoryRepository.GetHotelCategoryByIdAsync(u.HotelCategoryId).Result.CategoryName,
                    Quantity=u.Quantity
                }).ToList();

                var ditem = _mapper.Map<List<ItemsModel>>(newItems);
                    items.AddRange(ditem);

                request.Items = items;   

            }

            return View(myRequests);
        }
        
        public async Task<IActionResult> Disburse(int Id)
        {


            var myRequestList = await _approvalRepository.GetRequisitionDetailsAsync(Id);
            if (myRequestList == null)
            {

                Alert("You are not authorise to approve the selected Request.", Enums.Enums.NotificationType.info);
                return RedirectToAction("Index");
            }
            var approvalComments = await _approvalRepository.GetApprovalCommentsAsync(Id);
            RequisitionModel myRequests = _mapper.Map<RequisitionModel>(myRequestList);

           
                List<ItemsModel> items = new List<ItemsModel>();

                //var item = myRequestList.Where(x => x.Id == request.Id)
                //                        .FirstOrDefault();
                var newItems = myRequestList.RequisitionDetails.Select(u => new ItemsModel
                {
                    HotelCategoryId = u.HotelCategoryId,
                    ItemDescription = u.ItemDescription,
                    HotelCategory = _HotelCategoryRepository.GetHotelCategoryByIdAsync(u.HotelCategoryId).Result.CategoryName,
                    Quantity = u.Quantity,
                    IsProcurement = u.IsProcurement,
                    QuantityIssued = u.QuantityIssued
                }).ToList();

                var ditem = _mapper.Map<List<ItemsModel>>(newItems);
                items.AddRange(ditem);

                myRequests.Items = items;

                List<ApprovalModel> approvals = new List<ApprovalModel>();

                var newApprovals = approvalComments.Select(u => new ApprovalModel
                {
                    ApprovalFullName = u.ApproverUserId.FullName,
                    Comment = u.Comment,
                    ApprovalDate = u.DateCreated,
                    RequisitionId = u.RequisitionId
                }).ToList();

                ViewBag.Comments = newApprovals;

                return View(myRequests);
        }

        [HttpPost]
        public async Task<IActionResult> Disburse(RequisitionModel requisition, int[] ItemId, decimal[] QuantityIssued)
        {
            
            // view return to be optimized.
            var myRequestList = await _approvalRepository.GetRequisitionDetailsAsync(requisition.Id);
            if (myRequestList == null)
            {

                Alert("You are not authorise to approve the selected Request.", Enums.Enums.NotificationType.info);
                return RedirectToAction("Index");
            }
            var approvalComments = await _approvalRepository.GetApprovalCommentsAsync(requisition.Id);
            RequisitionModel myRequests = _mapper.Map<RequisitionModel>(myRequestList);


            List<ItemsModel> items = new List<ItemsModel>();

            //var item = myRequestList.Where(x => x.Id == request.Id)
            //                        .FirstOrDefault();
            bool[] IsProcurement = requisition.Items.Select(x=>x.IsProcurement).ToArray();
            var newItems = myRequestList.RequisitionDetails.Select(u => new ItemsModel
            {
                HotelCategoryId = u.HotelCategoryId,
                ItemDescription = u.ItemDescription,
                HotelCategory = _HotelCategoryRepository.GetHotelCategoryByIdAsync(u.HotelCategoryId).Result.CategoryName,
                IsProcurement = u.IsProcurement,
                Quantity = u.Quantity
            }).ToList();

            // lazy code for email to be optimized.
            var isProcurementMail = false;
            for (int i = 0; i < QuantityIssued.Length; i++)
            {
                var newitem = newItems.Where(x => x.HotelCategoryId == ItemId[i]).FirstOrDefault();                
                newitem.QuantityIssued = Convert.ToInt16(QuantityIssued[i]);
                newitem.IsProcurement = IsProcurement[i];
                if (IsProcurement[i])
                {
                    newitem.QuantityToProcure = newitem.Quantity - newitem.QuantityIssued;
                    isProcurementMail = true;
                }
            }


            var ditem = _mapper.Map<List<ItemsModel>>(newItems);
            items.AddRange(ditem);

            myRequests.Status = requisition.Status;
            myRequests.Items = items;
            //var IsProcurement = ditem.Select(x => x.IsProcurement).ToArray();
            // disburse process
            if (ModelState.IsValid)
            {
                var result = await _approvalRepository.DisburseRequest(requisition.Status, requisition.Id, ItemId, QuantityIssued, IsProcurement);
                if (result)
                {
                    string getAllUserEmail = "";
                    string emailBody = "";
                    string CC = "";

                    // if user is an approval
                    //var checkUserInIdentity = await _accountManager.IsEmailExistAsync(requisition.StaffEmail);
                    //if (checkUserInIdentity)
                    //{
                    //    var userApprovalLevel = _contextAccessor.HttpContext.User.FindFirst("UserApprovalLevel").Value;
                    //    // copy approvals in the mail
                    //    CC = await _ApprovalConfigRepository.GetApprovalByDepartmentAsync(requisition.Department);
                    //}
                    //else
                    //{
                    //    // copy approvals in the mail
                    //    CC = await _ApprovalConfigRepository.GetApprovalEmailByUnitAsync(requisition.Department, requisition.Unit);
                    //}

                    CC = await _ApprovalConfigRepository.GetApprovalEmailByUnitAsync(requisition.Department, requisition.Unit);

                    string requisitionUrl = _config.GetSection("ProjectEndPoint:RequisitionURL").Value;
                    myRequests.AppLink = requisitionUrl;

                    getAllUserEmail = myRequests.StaffEmail;
                    string subject="";
                    if (isProcurementMail)
                    {
                        subject = "REQUISITION PROCUREMENT NOTIFICATION";
                        CC = CC + "," + await _ApprovalConfigRepository.GetUsersEmailByRoleAsync("Procurement");
                        emailBody = await _viewRender.RenderToStringAsync("/Views/MailTemplates/RequisitionProcurementTemplate.cshtml", myRequests);
                    }
                    else
                    {
                        subject = "REQUISITION DISBURSMENT NOTIFICATION";
                        emailBody = await _viewRender.RenderToStringAsync("/Views/MailTemplates/RequisitionDisbursementTemplate.cshtml", myRequests);

                    }
                    
                    // email section                    
                    await _emailSender.SendEmailAsync(getAllUserEmail, CC, subject, emailBody, "");

                    Alert("Success.", Enums.Enums.NotificationType.success);
                    return RedirectToAction("Index");
                }
                else
                {
                    Alert("SomeProblems were encountered while trying to perform operation. <br> Please try again.", Enums.Enums.NotificationType.error);
                }
            }

            return RedirectToAction("Index");
        }
      
      

      
        // }


    }
}