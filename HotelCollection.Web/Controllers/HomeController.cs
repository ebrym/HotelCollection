using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using HotelCollection.Repository.ApprovalRepo;
using HotelCollection.Repository.Interface;
using static HotelCollection.Web.Enums.Enums;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HotelCollection.Web.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {

        private readonly IRequisition _requisitionRepository;
        private readonly IApprovalRepository _approvalRepository;
        public HomeController(IRequisition requisitionRepository,
                                     IApprovalRepository approvalRepository)
        {
            _requisitionRepository = requisitionRepository;
            _approvalRepository = approvalRepository;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            try
            {
                // var request =await _requisitionRepository.GetRequisitionsDashBoardByUserAsync();
                // var pendingApproval = await _approvalRepository.GetPendingApprovalAsync();
                //
                //
                // ViewBag.TotalRequest = request.Count();
                //
                // ViewBag.Pending = request.Where(x=>x.ApprovalStatus != "Completed").Count();
                // ViewBag.Rejected = request.Where(x => x.ApprovalStatus == "Rejected").Count();
                // ViewBag.Approved = request.Where(x => x.ApprovalStatus == "Completed").Count();
                // ViewBag.Disbursed = request.Where(x => x.Status == "Complete").Count();
                // ViewBag.Partial = request.Where(x => x.Status == "Partial").Count();
                // ViewBag.AwaitingDisburse = request.Where(x => x.ApprovalStatus == "Completed" && x.Status == null).Count();
                // ViewBag.PendingApproval = pendingApproval.Count();
            }
            catch(Exception)
            {

            }
            Alert("Welcome to Hotel Collection Management.", Enums.Enums.NotificationType.info);

            return View();
        }
    }
}
