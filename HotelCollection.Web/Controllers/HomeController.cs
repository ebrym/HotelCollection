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

        private readonly IPaymentSetupRepository _paymentRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly IApprovalRepository _approvalRepository;
        public HomeController(IApprovalRepository approvalRepository,
            IPaymentSetupRepository paymentRepository,
            IHotelRepository hotelRepository)
        {
            _approvalRepository = approvalRepository;
            _paymentRepository = paymentRepository;
            _hotelRepository = hotelRepository;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            try
            {
                var hotel = await _hotelRepository.GetHotelAsync();
                 var payments = await _paymentRepository.GetPaymentSetupAsync();
                 var pendingApproval = await _approvalRepository.GetPendingApprovalAsync();
                //
                //
                 ViewBag.TotalPayments = payments.Count();
                 ViewBag.TotalHotel = hotel.Count();
                 //ViewBag.TotalHotelByCategory = hotel.Count(x=>x.);
                //
                 ViewBag.Pending = payments.Where(x=>x.ApprovalStatus != "Completed").Count();
                 ViewBag.Rejected = payments.Where(x => x.ApprovalStatus == "Rejected").Count();
                 ViewBag.Approved = payments.Where(x => x.ApprovalStatus == "Completed").Count();
                 //ViewBag.Disbursed = request.Where(x => x.Status == "Complete").Count();
                // ViewBag.Partial = request.Where(x => x.Status == "Partial").Count();
                // ViewBag.AwaitingDisburse = request.Where(x => x.ApprovalStatus == "Completed" && x.Status == null).Count();
                 ViewBag.PendingApproval = pendingApproval.Count();
            }
            catch(Exception)
            {

            }
            Alert("Welcome to Hotel Collection Management.", Enums.Enums.NotificationType.info);

            return View();
        }
    }
}
