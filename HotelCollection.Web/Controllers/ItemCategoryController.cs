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
    public class ItemCategoryController :  BaseController
    {
        private readonly IItemCategoryRepository _itemCategoryRepository;
        private readonly IMapper _mapper;

        public ItemCategoryController(IItemCategoryRepository itemCategoryRepository, IMapper mapper)
        {
            _itemCategoryRepository = itemCategoryRepository;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var categoryList = await _itemCategoryRepository.GetItemCategoryAsync();

            List<ItemCategoryModel> category = _mapper.Map<List<ItemCategoryModel>>(categoryList);

            return View(category);
        }

    



        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ItemCategoryModel category)
        {
            if (ModelState.IsValid)
            {

                var mappedCategory = _mapper.Map<ItemCategory>(category);

                var result = await _itemCategoryRepository.CreateItemCategoryAsync(mappedCategory);

                if (result)
                {
                    Alert("Item Category created successfully.", Enums.Enums.NotificationType.success);
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
                var category = await _itemCategoryRepository.GetItemCategoryByIdAsync(Id);

                ItemCategoryModel cat = _mapper.Map<ItemCategoryModel>(category);
                if(category == null)
                {
                   Alert("Invalid Item Category selected.", Enums.Enums.NotificationType.error);
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
        public async Task<IActionResult> Edit(ItemCategoryModel category)
        {

            if (ModelState.IsValid)
            {
                var getCategory = await _itemCategoryRepository.GetItemCategoryByIdAsync(category.Id);
                if (getCategory == null)
                    return View();
                getCategory.CategoryName = category.CategoryName;
                
                var result = await _itemCategoryRepository.UpdateItemCategoryAsync(getCategory);

              
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
          
                var result = await _itemCategoryRepository.DeleteItemCategoryAsync(Id);

            if (result)
            {
                Alert("Item Category deleted successfully.", Enums.Enums.NotificationType.success);
                return RedirectToAction("Index");
            }
            else
            {
                Alert("SomeProblems were encountered while trying to perform operation.  Please try again.", Enums.Enums.NotificationType.error);
            }

            return View("Index");
        }


      
        public ActionResult AccessDenied()
        {
            return View();
        }


    }
}