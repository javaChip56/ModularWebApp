using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ModularAngularWeb.Models;
using ModularAngularWeb.Common;

namespace ModularAngularWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InputDetailsController : ControllerBase
    {
        private List<ClientModel> _clientModels = null;

        [HttpPost]
        public IActionResult Submit([FromBody] ClientModel model)
        {
            // save data to the session
            if (HttpContext.Session.GetString("CLIENT_LIST") != null)
            {
                _clientModels = SessionHelper.GetObjectFromJson<List<ClientModel>>(HttpContext.Session, "CLIENT_LIST");
                setModelToSession(model);
            }
            else 
            {
                _clientModels = new List<ClientModel>();
                setModelToSession(model);
            }
            return Ok();
        }

        private void setModelToSession(ClientModel model)
        {
            _clientModels.Add(model);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "CLIENT_LIST", _clientModels);
        }

    }
}
