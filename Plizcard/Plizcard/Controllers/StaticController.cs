using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace PlizCard.Controllers
{
    public class StaticController : Controller
    {
        public void init()
        {
            UserSession.getUserSession();
            if (ControllerContext.HttpContext.Request.Cookies.AllKeys.Contains("CityID"))
            {
                try
                {
                    var csid = ControllerContext.HttpContext.Request.Cookies["CityID"].Value;
                    var cid = int.Parse(csid);
                    if(cid>0)
                    {
                        API.setCurrentCity(cid);
                    }

                }
                catch(Exception ex)
                { }
            }
        }
        public ActionResult Index()
        {
            init();
            return View();
        }
        public ActionResult HowItWorks()
        {
            init();

            return View();
        }

        public ActionResult Cards()
        {
            init();
            return View();
        }

        public ActionResult Partners()
        {
            init();
            ActionResult retunValue;
            string action = "";
            string subaction = "";
            int id = 0;
            if (RouteData.Values.ContainsKey("action")) action = RouteData.Values["action"].ToString();
            if (RouteData.Values.ContainsKey("subaction")) subaction = RouteData.Values["subaction"].ToString();
            if (RouteData.Values.ContainsKey("id")) id = int.Parse(RouteData.Values["id"].ToString());

            switch (subaction.ToLower())
            {
                case "details":
                    retunValue = View("Partner", API.GetPartner(id));
                    break;
                case "register":
                    retunValue = View("PartnerRegister");
                    break;
                case "poses":
                    retunValue = View("Poses", API.GetPartner(id));
                    break;
                default:
                    retunValue = View();
                    break;
            }
            return retunValue;
        }
        public ActionResult Programs()
        {
            init();
            return View();
        }

    public ActionResult Actions()
        {
            init();
            ActionResult retunValue;
            string action = "";
            string subaction = "";
            int id = 0;
            if (RouteData.Values.ContainsKey("action")) action = RouteData.Values["action"].ToString();
            if (RouteData.Values.ContainsKey("subaction")) subaction = RouteData.Values["subaction"].ToString();
            if (RouteData.Values.ContainsKey("id")) id = int.Parse(RouteData.Values["id"].ToString());

            switch (subaction.ToLower())
            {
                case "details":
                    retunValue = View("Action",ApiConfig.GetCampaign(id));
                    break;
                case "poses":
                    retunValue = View("ActionPoses", ApiConfig.GetCampaignDetail(id));
                    break;
                default:
                    retunValue = View();
                    break;
            }
            return retunValue;
        }

        public ActionResult MobileApplication()
        {
            init();
            return View();
        }
        public ActionResult Rules()
        {
            init();
            return View();
        }
        public ActionResult Faq()
        {
            init();
            return View();
        }
        public ActionResult FeedBack()
        {
            init();
            return View();
        }

        public ActionResult Favorites()
        {
            init();
            if (UserSession.user_id > 0)
                return View();
            else
                return View("Index");
        }

        public new ActionResult Profile()
        {
            init();
            if (UserSession.user_id > 0)
                return View();
            else
                return View("Index");
        }

        public ActionResult MyCards()
        {
            init();
            if (UserSession.user_id > 0)
                return View();
            else
                return View("Index");
        }

        public ActionResult MyOrders()
        {
            init();
            if (UserSession.user_id <= 0)
                return View("Index");
            ActionResult retunValue;
            string action = "";
            string subaction = "";
            int id = 0;
            if (RouteData.Values.ContainsKey("action")) action = RouteData.Values["action"].ToString();
            if (RouteData.Values.ContainsKey("subaction")) subaction = RouteData.Values["subaction"].ToString();
            if (RouteData.Values.ContainsKey("id")) id = int.Parse(RouteData.Values["id"].ToString());

            switch (subaction.ToLower())
            {
                case "details":
                    retunValue = View("OrderDetails",API.GetChequeDetails(id));
                    break;
                default:
                    retunValue = View();
                    break;
            }
            return retunValue;
        }

        public ActionResult PersonalActions()
        {
            init();
            if (UserSession.user_id<=0)
                return View("Index");
            ActionResult retunValue;
            string action = "";
            string subaction = "";
            int id = 0;
            if (RouteData.Values.ContainsKey("action")) action = RouteData.Values["action"].ToString();
            if (RouteData.Values.ContainsKey("subaction")) subaction = RouteData.Values["subaction"].ToString();
            if (RouteData.Values.ContainsKey("id")) id = int.Parse(RouteData.Values["id"].ToString());

            switch (subaction.ToLower())
            {
                case "details":
                    retunValue = View("Action", API.GetCampaign(id));
                    break;
                case "poses":
                    retunValue = View("ActionPoses");
                    break;
                default:
                    retunValue = View();
                    break;
            }
            return retunValue;
        }

        /// <summary>
        /// Email validation
        /// </summary>
        /// <returns></returns>
        [System.Web.Mvc.Route("Email")]
        public ActionResult Email(string client, string code)
        {
            init();
            try
            {
                client = Request.QueryString["client"].ToString();
                code = Request.QueryString["code"].ToString();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            if(!string.IsNullOrEmpty(client) && !string.IsNullOrEmpty(code))
            {
                var result = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<lcclient.ValidateEmailResponse>(API.ValidateEmail(null, code, Convert.ToInt32(client)));
                if (result.ErrorCode == 0)
                {
                    return Redirect("Index#successvalidateimail");
                }
            }
            return RedirectToAction("Index");
        }
    }
}