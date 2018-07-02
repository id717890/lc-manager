using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LC_Manager.Controllers
{
    public class BookkeepingController : Controller
    {
        [AuthorizeJwt(Roles = "Total"), HttpGet]
        public ActionResult Index()
        {
            try
            {
                ViewBag.OperatorName = Convert.ToString(HttpContext.Request.Cookies["operatorName"].Value);
                ViewBag.RoleName = Implementation.JwtProps.GetRole();
            }
            catch { }
            return View();
        }
    }
}