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

namespace HotelCollection.Web.Controllers
{
    public class HotelCategoryFeeController  :  BaseController
    {
        private readonly IHotelCategoryFeeRepository _HotelCategoryFeeRepository;
        private readonly IHotelCategoryRepository _HotelCategoryRepository;
        private readonly IMapper _mapper;
        public HotelCategoryFeeController(IHotelCategoryFeeRepository HotelCategoryFeeRepository, IHotelCategoryRepository HotelCategoryRepository, IMapper mapper)
        {
            _HotelCategoryFeeRepository = HotelCategoryFeeRepository;
            _HotelCategoryRepository = HotelCategoryRepository;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var HotelCategoryFeeList = await _HotelCategoryFeeRepository.GetHotelCategoryFeeAsync();

            List<HotelCategoryFeeModel> hotelCategoryFee = _mapper.Map<List<HotelCategoryFeeModel>>(HotelCategoryFeeList);

            return View(hotelCategoryFee);
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
        public async Task<IActionResult> Create(HotelCategoryFeeModel HotelCategoryFee)
        {
            if (ModelState.IsValid)
            {

                var mappedHotel = _mapper.Map<HotelCategoryFee>(HotelCategoryFee);

                mappedHotel.CategoryId = HotelCategoryFee.CategoryId;
                var result = await _HotelCategoryFeeRepository.CreateHotelCategoryFeeAsync(mappedHotel);

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

            return View(HotelCategoryFee);
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


                var HotelCategoryFee = await _HotelCategoryFeeRepository.GetHotelCategoryFeeByIdAsync(Id);

                HotelCategoryFeeModel cat = _mapper.Map<HotelCategoryFeeModel>(HotelCategoryFee);
                if (HotelCategoryFee == null)
                {
                    Alert("Invalid Hotel selected.", Enums.Enums.NotificationType.error);
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
        public async Task<IActionResult> Edit(HotelCategoryFeeModel Hotel)
        {

            if (ModelState.IsValid)
            {
                var getHotel = await _HotelCategoryFeeRepository.GetHotelCategoryFeeByIdAsync(Hotel.Id);
                if (getHotel == null)
                    return View();
                var category = await _HotelCategoryRepository.GetHotelCategoryByIdAsync(Hotel.CategoryId);
                getHotel.Fee = (double)Hotel.Fee;
                getHotel.Category = category;

                var result = await _HotelCategoryFeeRepository.UpdateHotelCategoryFeeAsync(getHotel);


                if (result)
                {
                    Alert("Hotel Category Fee updated successfully.", Enums.Enums.NotificationType.success);
                    return RedirectToAction("Index");
                }
                else
                {
                    Alert("SomeProblems were encountered while trying to perform operation. Please try again.", Enums.Enums.NotificationType.error);
                }
            }
            return View(Hotel);
        }
        public async Task<IActionResult> Delete(int Id)
        {

            var result = await _HotelCategoryFeeRepository.DeleteHotelCategoryFeeAsync(Id);

            if (result)
            {
                Alert("Hotel deleted successfully.", Enums.Enums.NotificationType.success);
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
