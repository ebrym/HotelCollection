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
    public class PaymentController :  BaseController
    {
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IPaymentTransaction _paymentTransaction;
        private IHostingEnvironment _hostingEnv;
        private readonly IAccountManager _accountManager;
        private IViewRenderService _viewRender;
        private readonly ISMTPService _emailSender;
        private IConfiguration _config;

        public PaymentController(IHttpContextAccessor contextAccessor, 
                                     IRequisition requisitionRepository, 
                                     IPaymentRepository paymentRepository,
                                     IMapper mapper,
                                     IConfiguration config,
                                     IAccountManager accountManager,
                                     IHostingEnvironment hostingEnv,
                                     IViewRenderService viewRender, 
                                     ISMTPService emailSender,
                                     IPaymentTransaction paymentTransaction)
        {
            _mapper = mapper;
            _contextAccessor = contextAccessor;
            _hostingEnv = hostingEnv;
            _accountManager = accountManager;
            _viewRender = viewRender;
             _paymentRepository =  paymentRepository;
            _emailSender = emailSender;
            _config = config;
            _paymentTransaction = paymentTransaction;
    }

        public async Task<IActionResult> Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(string reference)
        {
            Response.Redirect("/payment/Reference/" + reference);
            return View();
        }
        public async Task<IActionResult> Reference(string id)
        {
        // public const string PayStackKey = "pk_test_6021bef74aeb2e8276bf85e3744f5c20a9af29af";
        // public const string Currency = "NGN";

            var myRequestList = await _paymentRepository.GetPaymentDetailsByReferenceAsync(id);
            if (myRequestList == null)
            {

                Alert("Invalid payment Reference or payment not approved!.", Enums.Enums.NotificationType.info);
                return RedirectToAction("Index");
            }
            PaymentSetupModel myRequests = _mapper.Map<PaymentSetupModel>(myRequestList);
            
          
            return View(myRequests);
        }
        [HttpPost]
        public async Task<IActionResult> Reference(PaymentSetupModel payment)
        {
            var myRequestList = await _paymentRepository.GetPaymentDetailsByReferenceAsync(payment.ReferenceNo);
            if (myRequestList == null)
            {

                Alert("Invalid payment Reference or payment not approved!.", Enums.Enums.NotificationType.info);
                return RedirectToAction("Index");
            }
            PaymentSetupModel myRequests = _mapper.Map<PaymentSetupModel>(myRequestList);

            var paymentModel = new PaymentModel
            {
                Amount = myRequests.Amount,
                HotelName = myRequests.Hotel.Name,
                Reference = myRequests.ReferenceNo,
                Email = myRequests.Hotel.Email,
                PaymentType = myRequests.PaymentType.Type
            };
            //Response.Redirect("~/Reference/" + reference);
            // ViewBag.Amount = payment.Amount;
            // ViewBag.HotelName = payment.Hotel.Name;
            // ViewBag.Reference = payment.ReferenceNo;
            // ViewBag.Email = payment.Hotel.Email;
            // ViewBag.PaymentType = payment.PaymentType;
           // Response.Redirect("~/Payment/Pay");
            return RedirectToAction("Pay", paymentModel);
           // return View(null);
        }

        public IActionResult Pay(PaymentModel payment)
        {
            return View(payment);
        }

        [HttpGet("Transaction/{reference}")]
        public async Task<IActionResult> TransactionUpdate(string reference)
        {
            var transaction = await _paymentTransaction.GetTransactionByReference(reference);
            var paymentSetup = await _paymentRepository.GetPaymentDetailsByReferenceAsync(reference);
            
            if (transaction == null && paymentSetup != null)
            {
                var paymentModel = new PaymentTransaction
                {
                    Amount = Convert.ToDecimal(paymentSetup.Amount), // 100 ,
                    PaymentSetup = paymentSetup,
                    Reference = reference,
                    Status = "Paid",
                    PaymentMode = "Card",
                    PaymentSetupId = paymentSetup.Id
                };
                await _paymentTransaction.CreatePaymentTransactionAsync(paymentModel);

            }
            
            
            return null;
        }

    }
}