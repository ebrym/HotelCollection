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
    public class RequisitionController :  BaseController
    {
        private readonly IItemCategoryRepository _itemCategoryRepository;
        private readonly IMapper _mapper;
        private readonly IProjectService _projectService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IRequisition _requisitionRepository;
        private IHostingEnvironment _hostingEnv;
        private readonly IAccountManager _accountManager;
        private IViewRenderService _viewRender;
        private readonly ISMTPService _emailSender;
        private readonly IApprovalConfigRepository _ApprovalConfigRepository;
        private IConfiguration _config;
        private readonly IApprovalRepository _approvalRepository;


        public RequisitionController(IItemCategoryRepository itemCategoryRepository, 
                                     IHttpContextAccessor contextAccessor, 
                                     IRequisition requisitionRepository, 
                                     IProjectService projectService,
                                     IMapper mapper,
                                     IConfiguration config,
                                     IApprovalConfigRepository ApprovalConfigRepository,
                                     IAccountManager accountManager,
                                     IHostingEnvironment hostingEnv,
                                     IViewRenderService viewRender,
                                     IApprovalRepository approvalRepository,
                                     ISMTPService emailSender)
        {
            _itemCategoryRepository = itemCategoryRepository;
            _projectService = projectService;
            _mapper = mapper;
            _contextAccessor = contextAccessor;
            _hostingEnv = hostingEnv;
            _accountManager = accountManager;
            _viewRender = viewRender;
            _ApprovalConfigRepository = ApprovalConfigRepository;
            _approvalRepository = approvalRepository;
            _emailSender = emailSender;
            _requisitionRepository = requisitionRepository;
            _config = config;
        }
        public async Task<IActionResult> Index()
        {
            var myRequestList = await _requisitionRepository.GetRequisitionsByUserAsync();


            List<RequisitionModel> myRequests = _mapper.Map<List<RequisitionModel>>(myRequestList);

            foreach (var request in myRequests)
            {
                request.ApprovalStatus = request.ApprovalStatus == null? await _ApprovalConfigRepository.GetApprovalByDepartmentAsync(request.Department, request.CurrentApprovalLevel) : request.ApprovalStatus;
                List<ItemsModel> items = new List<ItemsModel>();

                var item = myRequestList.Where(x => x.Id == request.Id)                                        
                                        .FirstOrDefault();
                var newItems = item.RequisitionDetails.Select(u => new ItemsModel {
                    ItemCategoryId = u.ItemCategoryId,
                    ItemDescription = u.ItemDescription,
                    ItemCategory =  _itemCategoryRepository.GetItemCategoryByIdAsync(u.ItemCategoryId).Result.CategoryName,
                    Quantity=u.Quantity
                }).ToList();

                var ditem = _mapper.Map<List<ItemsModel>>(newItems);
                    items.AddRange(ditem);

                request.Items = items;   

            }

            return View(myRequests);
        }
        
        public async Task<IActionResult> Create()
        {
            var projectList = await _projectService.GetAllProjectsAsync();
            var project = projectList.Where(a => a.accountManager != null).Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.ProjectName + " (" + a.ProjectCode + ")"
            }).ToList();


            var marketers = projectList.Where(a => a.accountManager != null).Select(a => new ProjectMarketer
            {
                ProjectId = a.Id.ToString(),
                ProjectCode = a.ProjectCode.ToString(),
                ProjectName = a.ProjectName.ToString(),
                fullName = a.accountManager.fullName,
                email = a.accountManager.email
            }).ToList();

            ViewBag.project = project;
            ViewBag.marketer = marketers;

            // get item category

            var categoryList = await _itemCategoryRepository.GetItemCategoryAsync();
            List<ItemCategoryModel> category = _mapper.Map<List<ItemCategoryModel>>(categoryList);
            var dcategory = category.Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.CategoryName 
            }).ToList();

            ViewBag.category = dcategory;


            // defailt model values
            var reqHearderDetails = new RequisitionModel
                {
                    StaffName =  _contextAccessor.HttpContext.User.FindFirst("displayName").Value,
                    StaffEmail = _contextAccessor.HttpContext.User.FindFirst("mail").Value,
                    Department = _contextAccessor.HttpContext.User.FindFirst("department").Value,
                    LineManager = _contextAccessor.HttpContext.User.FindFirst("manager").Value,
                    JobTitle = _contextAccessor.HttpContext.User.FindFirst("jobTitle").Value,
                    Unit = _contextAccessor.HttpContext.User.FindFirst("Unit").Value
                };
            // deafaul item to the model list
            var defaultItem = new ItemsModel
            {
                ItemCategoryId = 0,
                ItemDescription = "",
                Quantity = 0
            };
            var defaultItemList = new List<ItemsModel>();
            defaultItemList.Add(defaultItem);


            // get the item count for the collection dynamic add.
            ViewBag.itemCount = defaultItemList.Count();

            // bind difault item to the model list
            reqHearderDetails.Items = defaultItemList;
            

            return View(reqHearderDetails);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RequisitionModel requisition)
        {
            var projectList = await _projectService.GetAllProjectsAsync();
            var project = projectList.Where(a => a.accountManager != null).Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.ProjectName + " (" + a.ProjectCode + ")"
            }).ToList();


            var marketers = projectList.Where(a => a.accountManager != null).Select(a => new ProjectMarketer
            {
                ProjectId = a.Id.ToString(),
                ProjectCode = a.ProjectCode.ToString(),
                ProjectName = a.ProjectName.ToString(),
                fullName = a.accountManager.fullName,
                email = a.accountManager.email
            }).ToList();

            ViewBag.project = project;
            ViewBag.marketer = marketers;

            // get item category
            var categoryList = await _itemCategoryRepository.GetItemCategoryAsync();
            List<ItemCategoryModel> category = _mapper.Map<List<ItemCategoryModel>>(categoryList);
            var dcategory = category.Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.CategoryName
            }).ToList();

            ViewBag.category = dcategory;

            // get the item count for the collection dynamic add.
            ViewBag.itemCount = requisition.Items.Count();

            //var errors = ModelState.Select(x => x.Value.Errors).ToList();

            if (ModelState.IsValid)
            {
                //Requisition req = new Requisition
                //{

                //};
                
                var mappedRequisition = _mapper.Map<Requisition>(requisition);

                List<ItemsModel> itemDetails = new List<ItemsModel>(); //_mapper.Map<List<RequisitionDetails>>(requisition.Items);
                itemDetails.AddRange(requisition.Items);

                var item = _mapper.Map<List<RequisitionDetails>>(itemDetails);

                mappedRequisition.RequisitionDetails = item;


                // uload file if exist
                if (requisition.UploadDoc != null && !string.IsNullOrEmpty(requisition.UploadDoc.FileName))
                {

                    string webRootPath = _hostingEnv.WebRootPath;

                    var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png" };
                    var checkextension = Path.GetExtension(requisition.UploadDoc.FileName).ToLower();
                    var InvoiceFilePath = Path.GetExtension(requisition.UploadDoc.FileName);

                    if (!allowedExtensions.Contains(checkextension))
                    {
                        Alert("Invalid file extention.", Enums.Enums.NotificationType.error);
                        return View(requisition);
                    }

                    var path = Path.Combine(webRootPath, "uploads", InvoiceFilePath);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }

                    using (Stream stream = new FileStream(path, FileMode.Create))
                    {
                        await requisition.UploadDoc.CopyToAsync(stream);
                    }

                    requisition.DocumentPath = path;
                }

                var getAllUserEmail = "";
                var checkUserInIdentity = await _accountManager.IsEmailExistAsync(requisition.StaffEmail);
                var userApprovalLevel = _contextAccessor.HttpContext.User.FindFirst("UserApprovalLevel").Value;
                if (checkUserInIdentity && Convert.ToInt16(userApprovalLevel) > 0)
                {
                    
                
                   // await _ApprovalConfigRepository.GetApprovalLevelByUserAsync(requisition.StaffEmail);
                    mappedRequisition.CurrentApprovalLevel = (Convert.ToInt16(userApprovalLevel) + 1).ToString();

                    // user in identity
                    //var approvalRoleBydepartment = await _ApprovalConfigRepository.GetApprovalRoleByDepartmentAsync(requisition.Department);
                    getAllUserEmail = await _ApprovalConfigRepository.GetApprovalEmailByDepartmentAsync(requisition.Department, mappedRequisition.CurrentApprovalLevel);
                }
                else
                {
                    //user not in identity
                    mappedRequisition.CurrentApprovalLevel = "1";
                    getAllUserEmail = await _ApprovalConfigRepository.GetApprovalEmailByUnitAsync(requisition.Department, requisition.Unit);
                }

                var result = await _requisitionRepository.CreateRequisitionAsync(mappedRequisition);

                if (result)
                {

                    string requisitionUrl = _config.GetSection("ProjectEndPoint:RequisitionURL").Value;
                    requisition.AppLink = requisitionUrl;

                    var subject = "REQUEST NOTIFICATION";
                     string emailBody = await _viewRender.RenderToStringAsync("/Views/MailTemplates/RequisitionMailTemplate.cshtml", requisition);
                    
                    
                    await _emailSender.SendEmailAsync(getAllUserEmail, subject, emailBody, "");
                    Alert("Requisition created successfully.", Enums.Enums.NotificationType.success);
                    return RedirectToAction("Index");
                }
                else
                {
                    Alert("SomeProblems were encountered while trying to perform operation. <br> Please try again.", Enums.Enums.NotificationType.error);
                }


            }

            return View(requisition);
        }
        public async Task<IActionResult> Details(int Id)
        {


            var myRequestList = await _requisitionRepository.GetRequisitionDetailsByUserAsync(Id);
            if (myRequestList == null)
            {

                Alert("You are not authorize to view the selected Request.", Enums.Enums.NotificationType.info);
                return RedirectToAction("Index");
            }
            var approvalComments = await _approvalRepository.GetApprovalCommentsAsync(Id);
            RequisitionModel myRequests = _mapper.Map<RequisitionModel>(myRequestList);


            List<ItemsModel> items = new List<ItemsModel>();

            //var item = myRequestList.Where(x => x.Id == request.Id)
            //                        .FirstOrDefault();
            var newItems = myRequestList.RequisitionDetails.Select(u => new ItemsModel
            {
                ItemCategoryId = u.ItemCategoryId,
                ItemDescription = u.ItemDescription,
                ItemCategory = _itemCategoryRepository.GetItemCategoryByIdAsync(u.ItemCategoryId).Result.CategoryName,
                Quantity = u.Quantity
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

        //
        // public ActionResult AccessDenied()
        // {
        //     return View();
        // }


    }
}