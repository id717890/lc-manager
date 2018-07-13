using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using LCManager.JWT;
using LC_Manager.Models;

namespace LC_Manager.Controllers
{
    public class ClientController : Controller
    {
        //// GET: Client
        //public ActionResult Index()
        //{
        //    return View();
        //}

        [AuthorizeJwt]
        public ActionResult SaveClientData(ClientChangeModel model)
        {
            return null;
        }

        [AuthorizeJwt]
        public JsonResult GetClientInfo(string card)
        {
            GetClientInfoRequest clientInfoRequest = new GetClientInfoRequest();
            if (!string.IsNullOrEmpty(card))
            {
                try { clientInfoRequest.Card = Convert.ToInt64(card); } catch { }
            }
            else
            {
                return Json(new GetClientInfoResponse { ErrorCode = 10, Message = "Не указана карта клиента" });
            }
            try { clientInfoRequest.Operator = Convert.ToInt16(JwtProps.GetOperator()); } catch { }
            HttpResponseMessage response = HttpClientService.PostAsync("api/values/ClientInfo", clientInfoRequest).Result;
            if (response.IsSuccessStatusCode)
            {
                GetClientInfoResponse clientInfoResponse = response.Content.ReadAsAsync<GetClientInfoResponse>().Result;
                return Json(clientInfoResponse);
            }
            else
            {
                return Json(new GetClientInfoResponse { ErrorCode = 11, Message = "Ошибка запроса к ClientInfo" });
            }
        }
    }
}
