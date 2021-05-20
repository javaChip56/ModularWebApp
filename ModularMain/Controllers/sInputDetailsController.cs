using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModularMain.Controllers
{
    [Route("[controller]")]
    public class sInputDetailsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
