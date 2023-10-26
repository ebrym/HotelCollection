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
    public class HotelCategoryController :  BaseController
    {
        private readonly IHotelCategoryRepository _HotelCategoryRepository;
        private readonly IMapper _mapper;

        public HotelCategoryController(IHotelCategoryRepository HotelCategoryRepository, IMapper mapper)
        {
            _HotelCategoryRepository = HotelCategoryRepository;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var categoryList = await _HotelCategoryRepository.GetHotelCategoryAsync();

            List<HotelCategoryModel> category = _mapper.Map<List<HotelCategoryModel>>(categoryList);

            return View(category);
        }

    



        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(HotelCategoryModel category)
        {
            if (ModelState.IsValid)
            {

                var mappedCategory = _mapper.Map<HotelCategory>(category);

                var result = await _HotelCategoryRepository.CreateHotelCategoryAsync(mappedCategory);

                if (result)
                {
                    Alert("Category created successfully.", Enums.Enums.NotificationType.success);
                    return RedirectToAction("Index");
                }
                else
                {
                    Alert("SomeProblems were encountered while trying to perform operation. <br> Please try again.", Enums.Enums.NotificationType.error);
                }


            }

            return View(category);
        }
        public async Task<IActionResult> Edit(int Id)
        {
            try
            {
                var category = await _HotelCategoryRepository.GetHotelCategoryByIdAsync(Id);

                HotelCategoryModel cat = _mapper.Map<HotelCategoryModel>(category);
                if(category == null)
                {
                   Alert("Invalid Category selected.", Enums.Enums.NotificationType.error);
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
        public async Task<IActionResult> Edit(HotelCategoryModel category)
        {

            if (ModelState.IsValid)
            {
                var getCategory = await _HotelCategoryRepository.GetHotelCategoryByIdAsync(category.Id);
                if (getCategory == null)
                    return View();
                getCategory.CategoryName = category.CategoryName;
                
                var result = await _HotelCategoryRepository.UpdateHotelCategoryAsync(getCategory);

              
                if (result)
                {
                    Alert("Item Category updated successfully.", Enums.Enums.NotificationType.success);
                    return RedirectToAction("Index");
                }
                else
                {
                    Alert("SomeProblems were encountered while trying to perform operation. Please try again.", Enums.Enums.NotificationType.error);
                }
            }
            return View(category);
        }

        public async Task<IActionResult> Delete(int Id)
        {
          
                var result = await _HotelCategoryRepository.DeleteHotelCategoryAsync(Id);

            if (result)
            {
                Alert("Category deleted successfully.", Enums.Enums.NotificationType.success);
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