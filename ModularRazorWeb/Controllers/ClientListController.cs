using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ModularRazorWeb.Models;
using ModularRazorWeb.Common;

namespace ModularRazorWeb.Controllers
{
    public class ClientListController : Controller
    {
        [Route("[controller]/show")]
        public IActionResult ShowList()
        {
            var model = new ClientListViewModel();

            if (HttpContext.Session.GetString("CLIENT_LIST") != null)
            {
                model.Clients = SessionHelper.GetObjectFromJson<List<ClientModel>>(HttpContext.Session, "CLIENT_LIST");
            }
            else
            {
                model.Clients = new List<ClientModel>();
            }

            return View(model);
        }
    }
}
