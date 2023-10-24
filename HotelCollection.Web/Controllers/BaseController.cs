using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using static HotelCollection.Web.Enums.Enums;

namespace HotelCollection.Web.Controllers
{
    public class BaseController : Controller
    {
        public void Alert(string message, Enums.Enums.NotificationType notificationType)
        {
            var msg = "<script language='javascript'>swal('" + notificationType.ToString().ToUpper() + "', '" + message + "','" + notificationType + "')" + "</script>";
            TempData["notification"] = msg;
        }
        /// <summary>
        /// Sets the information for the system notification.
        /// </summary>
        /// <param name="message">The message to display to the user.</param>
        /// <param name="notifyType">The type of notification to display to the user: Success, Error or Warning.</param>
        public void Message(string message, Enums.Enums.NotificationType notifyType)
        {
            TempData["Notification2"] = message;

            switch (notifyType)
            {
                case Enums.Enums.NotificationType.success:
                    TempData["NotificationCSS"] = "alert-box success";
                    break;
                case Enums.Enums.NotificationType.error:
                    TempData["NotificationCSS"] = "alert-box errors";
                    break;
                case Enums.Enums.NotificationType.warning:
                    TempData["NotificationCSS"] = "alert-box warning";
                    break;

                case Enums.Enums.NotificationType.info:
                    TempData["NotificationCSS"] = "alert-box notice";
                    break;
            }
        }
    }
}