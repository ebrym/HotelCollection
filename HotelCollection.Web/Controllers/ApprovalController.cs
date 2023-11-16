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
    //[Authorize]
    public class ApprovalController :  BaseController
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

        public ApprovalController(IHotelCategoryRepository HotelCategoryRepository, 
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
            var myRequestList = await _approvalRepository.GetPendingApprovalAsync();


            List<PaymentSetupModel> myRequests = _mapper.Map<List<PaymentSetupModel>>(myRequestList);

            

            return View(myRequests);
        }
        
        public async Task<IActionResult> Approve(int Id)
        {


            var myRequestList = await _approvalRepository.GetPaymentApprovalDetailsAsync(Id);
            if (myRequestList == null)
            {

                Alert("You are not authorise to approve the selected Payment.", Enums.Enums.NotificationType.info);
                return RedirectToAction("Index");
            }
            PaymentSetupModel myRequests = _mapper.Map<PaymentSetupModel>(myRequestList);
            
            
            var approvalComments = await _approvalRepository.GetApprovalCommentsAsync(Id);

            List<ApprovalModel> approvals = new List<ApprovalModel>();

            var newApprovals = approvalComments.Select(u => new ApprovalModel
            {
                ApprovalFullName = u.ApproverUserId.FullName,
                Comment = u.Comment,
                ApprovalDate = u.DateCreated,
                PaymentId = u.PaymentId
            }).ToList();

            ViewBag.Comments = newApprovals;
            return View(myRequests);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(PaymentSetupModel request)
        {
            // view return to be optimized.
            var myRequestList = await _approvalRepository.GetPaymentApprovalDetailsAsync(request.Id);
            if (myRequestList == null)
            {

                Alert("You are not authorise to approve the selected Request.", Enums.Enums.NotificationType.info);
                return RedirectToAction("Index");
            }
            PaymentSetupModel myRequests = _mapper.Map<PaymentSetupModel>(myRequestList);


           
            
            // aproval process
            if (ModelState.IsValid)
            {
                var mappedRequisition = _mapper.Map<PaymentSetup>(myRequests);

                var result =await _approvalRepository.CreateApprovalAsync(mappedRequisition,request.ApprovalComments, request.Approved);


                if (result)
                {
                    string getAllUserEmail = "";
                    string emailBody = "";
                    var isFinalApproval = _contextAccessor.HttpContext.User.FindFirst("IsFinalApproval").Value;
                    string requisitionUrl = _config.GetSection("ProjectEndPoint:RequisitionURL").Value;


                    myRequests.AppLink = requisitionUrl;

                    // check if approved or rejected
                    if (request.Approved)
                    {
                        if (!Convert.ToBoolean(isFinalApproval))
                        {
                            getAllUserEmail = await _ApprovalConfigRepository.GetNextApprovalsAsync(myRequests.Id);
                           // emailBody = await _viewRender.RenderToStringAsync("/Views/MailTemplates/RequisitionMailTemplate.cshtml", myRequests);
                        }
                        // else
                        // {
                        //     getAllUserEmail = myRequests.StaffEmail + "," + await _ApprovalConfigRepository.GetUsersEmailByRoleAsync("Store");
                        //     emailBody = await _viewRender.RenderToStringAsync("/Views/MailTemplates/RequisitionApprovedTemplate.cshtml", myRequests);
                        // }
                    }
                    else
                    {
                        myRequests.ApprovalComments = request.ApprovalComments;
                       // getAllUserEmail = myRequests.;
                       // emailBody = await _viewRender.RenderToStringAsync("/Views/MailTemplates/RequisitionRejectedTemplate.cshtml", myRequests);
                    }

                    // email section
                     var subject = "PAYMENT APPROVAL NOTIFICATION";
                   // await _emailSender.SendEmailAsync(getAllUserEmail, subject, emailBody, "");

                    Alert("Payment Approved created successfully.", Enums.Enums.NotificationType.success);
                    return RedirectToAction("Index");
                }
                else
                {
                    Alert("SomeProblems were encountered while trying to perform operation. <br> Please try again.", Enums.Enums.NotificationType.error);
                }


            }

            return View(myRequests);
        }
      

        
      


      
        // public ActionResult AccessDenied()
        // {
        //     return View();
        // }


    }
}