using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using LCManager.JWT;
using LC_Manager.Models;
using LCManager.Infrastructure.Response;
using LCManager.Infrastructure.Request;

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
            LCManager.Infrastructure.Request.GetClientInfoRequest clientInfoRequest = new LCManager.Infrastructure.Request.GetClientInfoRequest();
            if (!string.IsNullOrEmpty(card))
            {
                try { clientInfoRequest.Card = Convert.ToInt64(card); } catch { }
            }
            else
            {
                return Json(new LCManager.Infrastructure.Response.GetClientInfoResponse { ErrorCode = 10, Message = "Не указана карта клиента" });
            }
            try { clientInfoRequest.Operator = Convert.ToInt16(JwtProps.GetOperator()); } catch { }
            HttpResponseMessage response = HttpClientService.PostAsync("api/values/ClientInfo", clientInfoRequest).Result;
            if (response.IsSuccessStatusCode)
            {
                LCManager.Infrastructure.Response.GetClientInfoResponse clientInfoResponse = response.Content.ReadAsAsync<LCManager.Infrastructure.Response.GetClientInfoResponse>().Result;
                if (clientInfoResponse.ErrorCode == 0)
                {
                    ClientInfoResponse response2 = new ClientInfoResponse();
                    ClientInfoRequest request2 = new ClientInfoRequest
                    {
                        ClientId = Convert.ToInt32(clientInfoResponse.Id),
                        OperatorId = JwtProps.GetOperator()
                    };
                    HttpResponseMessage responseMessage2 = HttpClientService.PostAsync("api/client/GetClient", request2).Result;
                    if (responseMessage2.IsSuccessStatusCode)
                    {
                        response2 = responseMessage2.Content.ReadAsAsync<ClientInfoResponse>().Result;
                        if (response2.ErrorCode == 0)
                        {
                            return Json(new { data1 = clientInfoResponse, data2 = response2 });
                        }
                    }
                    return Json(new DefaultResponse { ErrorCode = 11, Message = "Ошибка запроса к ClientInfo" });
                }
            }
            return Json(new DefaultResponse { ErrorCode = 11, Message = "Ошибка запроса к ClientInfo" });
        }
    }
}
