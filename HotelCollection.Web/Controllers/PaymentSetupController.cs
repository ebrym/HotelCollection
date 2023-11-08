using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HotelCollection.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using HotelCollection.Data.Entity;
using HotelCollection.Repository.Interface;
using static HotelCollection.Web.Enums.Enums;
using HotelCollection.Repository.Repo;
using HotelCollection.Web.Helpers;

namespace HotelCollection.Web.Controllers
{
    public class PaymentSetupController :   BaseController
    {
        private readonly IPaymentSetupRepository _PaymentSetupRepository;
        private readonly IHotelRepository _HotelRepository;
        private readonly IPaymentTypeRepository _PaymentTypeRepository;
        private readonly IMapper _mapper;
        public PaymentSetupController(IPaymentSetupRepository PaymentSetupRepository,IPaymentTypeRepository PaymentTypeRepository, IHotelRepository HotelRepository, IMapper mapper)
        {
            _PaymentSetupRepository = PaymentSetupRepository;
            _PaymentTypeRepository = PaymentTypeRepository;
            _HotelRepository = HotelRepository;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var PaymentSetupList = await _PaymentSetupRepository.GetPaymentSetupAsync();

            List<PaymentSetupModel> paymentSetup = _mapper.Map<List<PaymentSetupModel>>(PaymentSetupList);

            return View(paymentSetup);
        }
        public async Task<IActionResult> Create()
        {
            var hotelList = await _HotelRepository.GetHotelAsync();
            var dhotel = hotelList.Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.Name.ToString()
            }).ToList();

            ViewBag.hotel = dhotel;

            var paymentTypeList = await _PaymentTypeRepository.GetPaymentTypeAsync();
            var dpaymentType = paymentTypeList.Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.Type.ToString()
            }).ToList();

            ViewBag.paymentType = dpaymentType;


            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(PaymentSetupModel PaymentSetup)
        {
            if (ModelState.IsValid)
            {
                
                var mappedpaymentSetup = _mapper.Map<PaymentSetup>(PaymentSetup);

                mappedpaymentSetup.Amount = PaymentSetup.Amount;
                mappedpaymentSetup.ReferenceNo = BLL.GetUniqueReferenceNumber(12);
                var result = await _PaymentSetupRepository.CreatePaymentSetupAsync(mappedpaymentSetup);

                if (result)
                {
                    Alert("Payment Setup created successfully.", Enums.Enums.NotificationType.success);
                    return RedirectToAction("Index");
                }
                else
                {
                    Alert("SomeProblems were encountered while trying to perform operation. <br> Please try again.", Enums.Enums.NotificationType.error);
                }


            }

            return View(PaymentSetup);
        }
        public async Task<IActionResult> Edit(int Id)
        {
            try
            {

                var hotelList = await _HotelRepository.GetHotelAsync();
                var dhotel = hotelList.Select(a => new SelectListItem()
                {
                    Value = a.Id.ToString(),
                    Text = a.Name
                }).ToList();

                ViewBag.hotel = dhotel;


                var paymentTypeList = await _PaymentTypeRepository.GetPaymentTypeAsync();
                var dpaymentType = paymentTypeList.Select(a => new SelectListItem()
                {
                    Value = a.Id.ToString(),
                    Text = a.Type.ToString()
                }).ToList();

                ViewBag.paymentType = dpaymentType;


                var PaymentSetup = await _PaymentSetupRepository.GetPaymentSetupByIdAsync(Id);

                PaymentSetupModel cat = _mapper.Map<PaymentSetupModel>(PaymentSetup);
                if (PaymentSetup == null)
                {
                    Alert("Invalid Payment Setup selected.", Enums.Enums.NotificationType.error);
                    return RedirectToAction("Index");
                }
                return View(cat);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpPost]
        public async Task<IActionResult> Edit(PaymentSetupModel PaymentSetup)
        {

            if (ModelState.IsValid)
            {
                var getPaymentSetup = await _PaymentSetupRepository.GetPaymentSetupByIdAsync(PaymentSetup.Id);
                if (getPaymentSetup == null)
                    return View();
                var hotel = await _HotelRepository.GetHotelByIdAsync(PaymentSetup.HotelId);
                var paymentType = await _PaymentTypeRepository.GetPaymentTypeByIdAsync(PaymentSetup.PaymentTypeId);

                getPaymentSetup.Amount = PaymentSetup.Amount;
                getPaymentSetup.ReferenceNo = PaymentSetup.ReferenceNo;
                getPaymentSetup.Hotel = hotel;
                getPaymentSetup.PaymentType = paymentType;

                var result = await _PaymentSetupRepository.UpdatePaymentSetupAsync(getPaymentSetup);


                if (result)
                {
                    Alert("Payment Setup updated successfully.", Enums.Enums.NotificationType.success);
                    return RedirectToAction("Index");
                }
                else
                {
                    Alert("Some Problems were encountered while trying to perform operation. Please try again.", Enums.Enums.NotificationType.error);
                }
            }
            return View(PaymentSetup);
        }
        public async Task<IActionResult> Delete(int Id)
        {

            var result = await _PaymentSetupRepository.DeletePaymentSetupAsync(Id);

            if (result)
            {
                Alert("Payment Setup deleted successfully.", Enums.Enums.NotificationType.success);
                return RedirectToAction("Index");
            }
            else
            {
                Alert("Some Problems were encountered while trying to perform operation.  Please try again.", Enums.Enums.NotificationType.error);
            }

            return View("Index");
        }

    }
}
