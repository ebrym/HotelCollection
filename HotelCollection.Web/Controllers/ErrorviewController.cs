using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HotelCollection.Web.Controllers
{
    public class ErrorviewController : BaseController
    {
        public IActionResult Error()
        {
            return View();
        }
    }
}