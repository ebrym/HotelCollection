using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using HotelCollection.Data.Entity;
using HotelCollection.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelCollection.Web.Filters
{
    public class AuditFilter: ActionFilterAttribute
    {
       
        private readonly IAuditLogRepository _auditLogRepo;
        public AuditFilter(IAuditLogRepository auditLogRepo)
        {
            _auditLogRepo = auditLogRepo;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                string actionName = null;
                string controllerName = null;

                // Getting ActionName
                if (((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ActionName != null)
                {
                    actionName =
                        ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor)
                        .ActionName;
                }
                // Getting ControllerName
                if (((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ControllerName != null)
                {
                    controllerName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ControllerName;
                }
                // context.HttpContext.User.FindFirst("username").Value,
                // Assigning values to AuditTb Classva

                //var username = context.HttpContext.User.FindFirst("username").Value;
                //if (!string.IsNullOrEmpty(username))
                //{
                var user = context.HttpContext.User;
                if(user != null)
                {
                     var objaudit = new AuditLog
                                        {
                                            UserName = context.HttpContext.User.Identity.Name ?? "",
                                            Id = 0,
                                            UserEmail = context.HttpContext.User.FindFirst("Email").Value ?? "",
                                            UserFullName = context.HttpContext.User.FindFirst("FullName").Value ?? "",
                                            SessionId = context.HttpContext.Session.Id ?? "",
                                            IpAddress = context.HttpContext.Connection.RemoteIpAddress.ToString() ?? "",
                                            PageAccessed = context.HttpContext.Request.GetDisplayUrl() ?? "",
                                            LoggedInAt = DateTime.Now,
                                            Method = context.HttpContext.Request.Method
                                        };
                     if (actionName == "Logout")
                      {
                          objaudit.LoggedOutAt = DateTime.Now;
                      }
                                          
                      objaudit.LoginStatus = "A";
                      objaudit.ControllerName = controllerName;
                      objaudit.ActionName = actionName;
  
                       _auditLogRepo.CreateAudit(objaudit);
                }
                   

                   

                //}


                base.OnActionExecuting(context);
            }
            catch (Exception)
            {
               /// throw;
            }
        }
    }
}
