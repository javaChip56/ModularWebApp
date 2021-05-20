using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModulesRCL.Controllers
{
    public class InputDetailsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
