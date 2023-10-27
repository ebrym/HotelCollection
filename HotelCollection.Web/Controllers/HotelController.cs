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
   // [Authorize]
    public class HotelController :  BaseController
    {
        private readonly IHotelRepository _HotelRepository;
        private readonly IHotelCategoryRepository _HotelCategoryRepository;
        private readonly IMapper _mapper;

        public HotelController(IHotelRepository HotelRepository,IHotelCategoryRepository HotelCategoryRepository, IMapper mapper)
        {
            _HotelRepository = HotelRepository;
            _HotelCategoryRepository = HotelCategoryRepository;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var HotelList = await _HotelRepository.GetHotelAsync();

            List<HotelRegistrationModel> hotel = _mapper.Map<List<HotelRegistrationModel>>(HotelList);

            return View(hotel);
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
        public async Task<IActionResult> Create(HotelRegistrationModel Hotel)
        {
            if (ModelState.IsValid)
            {

                var mappedHotel = _mapper.Map<Hotel>(Hotel);
                
                mappedHotel.CategoryId = Hotel.CategoryId;
                var result = await _HotelRepository.CreateHotelAsync(mappedHotel);

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

            return View(Hotel);
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
                
                
                var Hotel = await _HotelRepository.GetHotelByIdAsync(Id);

                HotelRegistrationModel cat = _mapper.Map<HotelRegistrationModel>(Hotel);
                if(Hotel == null)
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
        public async Task<IActionResult> Edit(HotelRegistrationModel Hotel)
        {

            if (ModelState.IsValid)
            {
                var getHotel = await _HotelRepository.GetHotelByIdAsync(Hotel.Id);
                if (getHotel == null)
                    return View();
                var category = await _HotelCategoryRepository.GetHotelCategoryByIdAsync(Hotel.CategoryId);
                getHotel.Name = Hotel.Name;
                getHotel.Email = Hotel.Email;
                getHotel.Phone = Hotel.Phone;
                getHotel.Address = Hotel.Address;
                getHotel.CACNumber = Hotel.CACNumber;
                getHotel.Category = category;
                
                var result = await _HotelRepository.UpdateHotelAsync(getHotel);

              
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
            return View(Hotel);
        }

        public async Task<IActionResult> Delete(int Id)
        {
          
                var result = await _HotelRepository.DeleteHotelAsync(Id);

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