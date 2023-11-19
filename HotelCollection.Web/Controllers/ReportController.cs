using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HotelCollection.Repository.Interface;
using Microsoft.AspNetCore.Mvc.Rendering;
using HotelCollection.Infrastructure.Interface;
using HotelCollection.Web.Models;
using HotelCollection.Repository.Repo;
using HotelCollection.Repository.Repos;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HotelCollection.Web.Controllers
{
    public class ReportController : Controller
    {
        private readonly IReportRepo _repo;
        private readonly IProjectService _projectService;
        private readonly IMapper _mapper;
        private readonly IHotelCategoryRepository _HotelCategoryRepository;
        private readonly ILocalGovernmentAreaRepository _LocalGovernmentAreaRepository;
        //private readonly IAuthentication _authService;

        public ReportController(IReportRepo repo, IProjectService projectService,
                                                IMapper mapper, IHotelCategoryRepository HotelCategoryRepository,ILocalGovernmentAreaRepository LocalGovernmentAreaRepository)
        {
            _repo = repo;
            _projectService = projectService;
            _mapper = mapper;
            _HotelCategoryRepository = HotelCategoryRepository;
            _LocalGovernmentAreaRepository = LocalGovernmentAreaRepository;
          //  _authService = authService;
        }
        // GET: /<controller>/
        public async Task<IActionResult> ByDepartmentAndRequisitionType()
        {
            var requisitions = await _repo.GetAll();
            // var department = await _authService.DepartmentFromAD();
            // var departmentList = department.Select(a => new SelectListItem()
            // {
            //     Value = a.ToString(),
            //     Text = a.ToString()
            // }).Distinct()
            // .OrderBy(a => a.Value)
            // .ToList();

            ViewBag.Departments = "";// departmentList;
            List<RequisitionModel> myRequests = _mapper.Map<List<RequisitionModel>>(requisitions);
            return View(myRequests);
        }

        [HttpPost]
        public async Task<IActionResult> ByDepartmentAndRequisitionType(string department, string requisitionType, DateTime startDate, DateTime endDate)
        {
            var requisitions = await _repo.GetByDepartmentRequisitionTypeAndDate(department,requisitionType,startDate,endDate);
            // var departments = await _authService.DepartmentFromAD();
            // var departmentList = departments.Select(a => new SelectListItem()
            // {
            //     Value = a.ToString(),
            //     Text = a.ToString()
            // }).Distinct()
            // .OrderBy(a => a.Value)
            // .ToList();

            ViewBag.Departments = "";//sdepartmentList;
            List<RequisitionModel> myRequests = _mapper.Map<List<RequisitionModel>>(requisitions);
            return View(myRequests);
        }

        public async Task<IActionResult> ByProject()
        {
            var requisitions = await _repo.GetAll();
            var projects = await _projectService.GetAllProjectsAsync();
            var projectsList = projects.Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.ProjectCode.ToString()
            }).Distinct()
            .OrderBy(a => a.Value)
            .ToList();

            ViewBag.Projects = projectsList;
            List<RequisitionModel> myRequests = _mapper.Map<List<RequisitionModel>>(requisitions);
            return View(myRequests);
        }

        [HttpPost]
        public async Task<IActionResult> ByProject(int projectId, DateTime startDate, DateTime endDate)
        {
            var requisitions = await _repo.GetByProject(projectId,startDate,endDate);
            var projects = await _projectService.GetAllProjectsAsync();
            var projectsList = projects.Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.ProjectCode.ToString()
            }).Distinct()
            .OrderBy(a => a.Value)
            .ToList();

            ViewBag.Projects = projectsList;
            List<RequisitionModel> myRequests = _mapper.Map<List<RequisitionModel>>(requisitions);
            return View(myRequests);
        }

        public async Task<IActionResult> ByDisbursedStatus()
        {
            var requisitions = await _repo.GetAll();
            List<RequisitionModel> myRequests = _mapper.Map<List<RequisitionModel>>(requisitions);
            return View(myRequests);
        }

        [HttpPost]
        public async Task<IActionResult> ByDisbursedStatus(string disburseStatus, DateTime startDate, DateTime endDate)
        {
            var requisitions = await _repo.GetByDisbursedStatus(disburseStatus, startDate,endDate);
            List<RequisitionModel> myRequests = _mapper.Map<List<RequisitionModel>>(requisitions);
            return View(myRequests);
        }

        public async Task<IActionResult> ByApprovalStatus()
        {
            var requisitions = await _repo.GetAll();
            List<RequisitionModel> myRequests = _mapper.Map<List<RequisitionModel>>(requisitions);
            return View(myRequests);
        }

        [HttpPost]
        public async Task<IActionResult> ByApprovalStatus(string approvalStatus, DateTime startDate, DateTime endDate)
        {
            var requisitions = await _repo.GetByApprovalStatus(approvalStatus,startDate,endDate);
            List<RequisitionModel> myRequests = _mapper.Map<List<RequisitionModel>>(requisitions);
            return View(myRequests);
        }
        public async Task<IActionResult> GetAllHotels()
        {
            var hotels = await _repo.GetAllHotels();
            List<HotelRegistrationModel> myRequests = _mapper.Map<List<HotelRegistrationModel>>(hotels);
            return View(myRequests);
        }

        public async Task<IActionResult> GetHotelsByCategory()
        {
            var hotels = await _repo.GetAllHotels();
            var categoryList = await _HotelCategoryRepository.GetHotelCategoryAsync();
            var dcategory = categoryList.Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.CategoryName
            }).ToList();

            ViewBag.hotelCategory = dcategory;

            List<HotelRegistrationModel> myRequests = _mapper.Map<List<HotelRegistrationModel>>(hotels);
            return View(myRequests);
        }
        [HttpPost]
        public async Task<IActionResult> GetHotelsByCategory(int categoryid)
        {
            var hotels = await _repo.GetHotelsByCategory(categoryid);
            var categoryList = await _HotelCategoryRepository.GetHotelCategoryAsync();
            var dcategory = categoryList.Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.CategoryName
            }).ToList();

            ViewBag.hotelCategory = dcategory;
            List<HotelRegistrationModel> myRequests = _mapper.Map<List<HotelRegistrationModel>>(hotels);
            return View(myRequests);
        }
        public async Task<IActionResult> GetByHotelsByLGA()
        {
            var hotels = await _repo.GetAllHotels();
            var lgaList = await _LocalGovernmentAreaRepository.GetLocalGovernmentAreaAsync();
            var dlga = lgaList.Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.LGAName
            }).ToList();

            ViewBag.lga = dlga;

            List<HotelRegistrationModel> myRequests = _mapper.Map<List<HotelRegistrationModel>>(hotels);
            return View(myRequests);
        }
        [HttpPost]
        public async Task<IActionResult> GetByHotelsByLGA(int lgaId)
        {
            var hotels = await _repo.GetByHotelsByLGA(lgaId);
            var lgaList = await _LocalGovernmentAreaRepository.GetLocalGovernmentAreaAsync();
            var dlga = lgaList.Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.LGAName
            }).ToList();

            ViewBag.lga = dlga;
            List<HotelRegistrationModel> myRequests = _mapper.Map<List<HotelRegistrationModel>>(hotels);
            return View(myRequests);
        }

    }
}
