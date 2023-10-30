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
    public class LocalGovernmentAreaController : BaseController
    {
        private readonly ILocalGovernmentAreaRepository _LocalGovernmentRepository;
        private readonly IMapper _mapper;
        public LocalGovernmentAreaController(ILocalGovernmentAreaRepository LocalGovernmentAreaRepository, IMapper mapper)
        {
            _LocalGovernmentRepository = LocalGovernmentAreaRepository;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var lgaList = await _LocalGovernmentRepository.GetLocalGovernmentAreaAsync();

            List<LocalGovernmentAreaModel> lga = _mapper.Map<List<LocalGovernmentAreaModel>>(lgaList);

            return View(lga);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(LocalGovernmentAreaModel lga)
        {
            if (ModelState.IsValid)
            {

                var mappedlga = _mapper.Map<LocalGovernmentArea>(lga);

                var result = await _LocalGovernmentRepository.CreateLocalGovernmentAreaAsync(mappedlga);

                if (result)
                {
                    Alert("Local Government created successfully.", Enums.Enums.NotificationType.success);
                    return RedirectToAction("Index");
                }
                else
                {
                    Alert("SomeProblems were encountered while trying to perform operation. <br> Please try again.", Enums.Enums.NotificationType.error);
                }


            }

            return View(lga);
        }
        public async Task<IActionResult> Edit(int Id)
        {
            try
            {
                var lga = await _LocalGovernmentRepository.GetLocalGovernmentAreaByIdAsync(Id);

                LocalGovernmentAreaModel cat = _mapper.Map<LocalGovernmentAreaModel>(lga);
                if (lga == null)
                {
                    Alert("Invalid Local Government selected.", Enums.Enums.NotificationType.error);
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
        public async Task<IActionResult> Edit(LocalGovernmentAreaModel lga)
        {

            if (ModelState.IsValid)
            {
                var getlga = await _LocalGovernmentRepository.GetLocalGovernmentAreaByIdAsync(lga.Id);
                if (getlga == null)
                    return View();
                getlga.LGAName = lga.LGAName;

                var result = await _LocalGovernmentRepository.UpdateLocalGovernmentAreaAsync(getlga);


                if (result)
                {
                    Alert("Local Government updated successfully.", Enums.Enums.NotificationType.success);
                    return RedirectToAction("Index");
                }
                else
                {
                    Alert("SomeProblems were encountered while trying to perform operation. Please try again.", Enums.Enums.NotificationType.error);
                }
            }
            return View(lga);
        }
        public async Task<IActionResult> Delete(int Id)
        {

            var result = await _LocalGovernmentRepository.DeleteLocalGovernmentAreaAsync(Id);

            if (result)
            {
                Alert("Local Government deleted successfully.", Enums.Enums.NotificationType.success);
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
