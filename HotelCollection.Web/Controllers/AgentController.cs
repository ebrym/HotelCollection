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
    public class AgentController :  BaseController
    {
        private readonly IAgentRepository _AgentRepository;
        private readonly IMapper _mapper;

        public AgentController(IAgentRepository AgentRepository, IMapper mapper)
        {
            _AgentRepository = AgentRepository;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var agentList = await _AgentRepository.GetAgentAsync();

            List<AgentModel> agent = _mapper.Map<List<AgentModel>>(agentList);

            return View(agent);
        }

    



        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AgentModel agent)
        {
            if (ModelState.IsValid)
            {

                var mappedagent = _mapper.Map<Agent>(agent);

                var result = await _AgentRepository.CreateAgentAsync(mappedagent);

                if (result)
                {
                    Alert("agent created successfully.", Enums.Enums.NotificationType.success);
                    return RedirectToAction("Index");
                }
                else
                {
                    Alert("SomeProblems were encountered while trying to perform operation. <br> Please try again.", Enums.Enums.NotificationType.error);
                }


            }

            return View(agent);
        }
        public async Task<IActionResult> Edit(int Id)
        {
            try
            {
                var agent = await _AgentRepository.GetAgentByIdAsync(Id);

                AgentModel cat = _mapper.Map<AgentModel>(agent);
                if(agent == null)
                {
                   Alert("Invalid agent selected.", Enums.Enums.NotificationType.error);
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
        public async Task<IActionResult> Edit(AgentModel agent)
        {

            if (ModelState.IsValid)
            {
                var getagent = await _AgentRepository.GetAgentByIdAsync(agent.Id);
                if (getagent == null)
                    return View();
                getagent.Name = agent.Name;
                getagent.Email = agent.Email;
                getagent.Phone = agent.Phone;
                getagent.Address = agent.Address;
                
                var result = await _AgentRepository.UpdateAgentAsync(getagent);

              
                if (result)
                {
                    Alert("Agent updated successfully.", Enums.Enums.NotificationType.success);
                    return RedirectToAction("Index");
                }
                else
                {
                    Alert("SomeProblems were encountered while trying to perform operation. Please try again.", Enums.Enums.NotificationType.error);
                }
            }
            return View(agent);
        }

        public async Task<IActionResult> Delete(int Id)
        {
          
                var result = await _AgentRepository.DeleteAgentAsync(Id);

            if (result)
            {
                Alert("agent deleted successfully.", Enums.Enums.NotificationType.success);
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