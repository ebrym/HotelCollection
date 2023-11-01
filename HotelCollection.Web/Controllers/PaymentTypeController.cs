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

namespace HotelCollection.Web.Controllers
{
    public class PaymentTypeController :   BaseController
    {
        private readonly IPaymentTypeRepository _PaymentTypeRepository;
        private readonly IHotelCategoryRepository _HotelCategoryRepository;
        private readonly IMapper _mapper;
        public PaymentTypeController(IPaymentTypeRepository PaymentTypeRepository, IHotelCategoryRepository HotelCategoryRepository, IMapper mapper)
        {
            _PaymentTypeRepository = PaymentTypeRepository;
            _HotelCategoryRepository = HotelCategoryRepository;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var PaymentTypeList = await _PaymentTypeRepository.GetPaymentTypeAsync();

            List<PaymentTypeModel> paymentType = _mapper.Map<List<PaymentTypeModel>>(PaymentTypeList);

            return View(paymentType);
        }
        public async Task<IActionResult> Create()
        {
            var categoryList = await _HotelCategoryRepository.GetHotelCategoryAsync();
            var dcategory = categoryList.Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.CategoryName
            }).ToList();

            ViewBag.hotelCategory = dcategory;

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(PaymentTypeModel PaymentType)
        {
            if (ModelState.IsValid)
            {

                var mappedpaymentType = _mapper.Map<PaymentType>(PaymentType);

                mappedpaymentType.CategoryId = PaymentType.CategoryId;
                var result = await _PaymentTypeRepository.CreatePaymentTypeAsync(mappedpaymentType);

                if (result)
                {
                    Alert("Hotel created successfully.", Enums.Enums.NotificationType.success);
                    return RedirectToAction("Index");
                }
                else
                {
                    Alert("SomeProblems were encountered while trying to perform operation. <br> Please try again.", Enums.Enums.NotificationType.error);
                }


            }

            return View(PaymentType);
        }
        public async Task<IActionResult> Edit(int Id)
        {
            try
            {

                var categoryList = await _HotelCategoryRepository.GetHotelCategoryAsync();
                var dcategory = categoryList.Select(a => new SelectListItem()
                {
                    Value = a.Id.ToString(),
                    Text = a.CategoryName
                }).ToList();

                ViewBag.hotelCategory = dcategory;


                var PaymentType = await _PaymentTypeRepository.GetPaymentTypeByIdAsync(Id);

                PaymentTypeModel cat = _mapper.Map<PaymentTypeModel>(PaymentType);
                if (PaymentType == null)
                {
                    Alert("Invalid Payment Type selected.", Enums.Enums.NotificationType.error);
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
        public async Task<IActionResult> Edit(PaymentTypeModel PaymentType)
        {

            if (ModelState.IsValid)
            {
                var getPaymentType = await _PaymentTypeRepository.GetPaymentTypeByIdAsync(PaymentType.Id);
                if (getPaymentType == null)
                    return View();
                var category = await _HotelCategoryRepository.GetHotelCategoryByIdAsync(PaymentType.CategoryId);
                getPaymentType.Type = PaymentType.Type;
                getPaymentType.Category = category;

                var result = await _PaymentTypeRepository.UpdatePaymentTypeAsync(getPaymentType);


                if (result)
                {
                    Alert("Hotel updated successfully.", Enums.Enums.NotificationType.success);
                    return RedirectToAction("Index");
                }
                else
                {
                    Alert("SomeProblems were encountered while trying to perform operation. Please try again.", Enums.Enums.NotificationType.error);
                }
            }
            return View(PaymentType);
        }
        public async Task<IActionResult> Delete(int Id)
        {

            var result = await _PaymentTypeRepository.DeletePaymentTypeAsync(Id);

            if (result)
            {
                Alert("Payment Type deleted successfully.", Enums.Enums.NotificationType.success);
                return RedirectToAction("Index");
            }
            else
            {
                Alert("SomeProblems were encountered while trying to perform operation.  Please try again.", Enums.Enums.NotificationType.error);
            }

            return View("Index");
        }
    }
}
