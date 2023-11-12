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
        private readonly IMapper _mapper;
        public PaymentTypeController(IPaymentTypeRepository PaymentTypeRepository, IMapper mapper)
        {
            _PaymentTypeRepository = PaymentTypeRepository;
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
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(PaymentTypeModel PaymentType)
        {
            if (ModelState.IsValid)
            {

                var mappedpaymentType = _mapper.Map<PaymentType>(PaymentType);

                var result = await _PaymentTypeRepository.CreatePaymentTypeAsync(mappedpaymentType);

                if (result)
                {
                    Alert("Payment Type created successfully.", Enums.Enums.NotificationType.success);
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
               
                getPaymentType.Type = PaymentType.Type;

                var result = await _PaymentTypeRepository.UpdatePaymentTypeAsync(getPaymentType);


                if (result)
                {
                    Alert("Payment Type updated successfully.", Enums.Enums.NotificationType.success);
                    return RedirectToAction("Index");
                }
                else
                {
                    Alert("Some Problems were encountered while trying to perform operation. Please try again.", Enums.Enums.NotificationType.error);
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
                Alert("Some Problems were encountered while trying to perform operation.  Please try again.", Enums.Enums.NotificationType.error);
            }

            return View("Index");
        }
    }
}
