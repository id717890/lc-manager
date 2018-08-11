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
using Site.Infrastrucure;

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
        public JsonResult SaveClientData(ClientChangeModel model)
        {
            try
            {
                if (model.Phone != model.PhoneInitial)
                {
                    if (!CheckPhoneOrCardNumberIsFree(model.Phone, string.Empty)) throw new Exception("Данный номер телефона принадлежит другому клиенту");
                }
                if (model.Card != model.CardInitial)
                {
                    if (!CheckPhoneOrCardNumberIsFree(string.Empty, model.Card)) throw new Exception("Данный номер карты принадлежит другому клиенту");
                }
                if (model.Email != model.EmailInitial)
                {
                    if (!CheckEmailIsFree(model.Email)) throw new Exception("Данный email принадлежит другому клиенту");
                }
                return Json(new { success = true, message = "Процедура успешно выполнена, до заглушки в ПО" });
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }
        }

        /// <summary>
        /// Получает инфу по клиенту для карточки редактирования
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
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
                    CardInfoResponse response2 = new CardInfoResponse();
                    CardStatisticsRequest request2 = new CardStatisticsRequest
                    {
                        Card = clientInfoResponse.Card
                    };
                    HttpResponseMessage responseMessage2 = HttpClientService.PostAsync("api/card/GetCard", request2).Result;
                    if (responseMessage2.IsSuccessStatusCode)
                    {
                        response2 = responseMessage2.Content.ReadAsAsync<CardInfoResponse>().Result;
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

        private bool CheckPhoneOrCardNumberIsFree(string phone, string card)
        {
            if (string.IsNullOrEmpty(phone) && string.IsNullOrEmpty(card)) return false;
            LCManager.Infrastructure.Request.GetClientInfoRequest request = new LCManager.Infrastructure.Request.GetClientInfoRequest();
            if (!string.IsNullOrEmpty(card))
            {
                try { request.Card = Convert.ToInt64(card); } catch { }
            }
            if (!string.IsNullOrEmpty(phone))
            {
                try { request.Phone = Convert.ToInt64(PhoneService.GetPhoneFromStr(phone)); } catch { }
            }
            try { request.Operator = Convert.ToInt16(JwtProps.GetOperator()); } catch { }
            HttpResponseMessage response = HttpClientService.PostAsync("api/values/ClientInfo", request).Result;
            if (response.IsSuccessStatusCode)
            {
                LCManager.Infrastructure.Response.GetClientInfoResponse data = response.Content.ReadAsAsync<LCManager.Infrastructure.Response.GetClientInfoResponse>().Result;
                if (data.ErrorCode == 0)
                {
                    return
                        string.IsNullOrEmpty(data.Name)
                        && string.IsNullOrEmpty(data.Surname)
                        && string.IsNullOrEmpty(data.Patronymic)
                        && data.Id == 0
                        && data.Card == 0;
                }
                return true;
            }
            return false;
        }

        private bool CheckEmailIsFree(string email)
        {
            if (string.IsNullOrEmpty(email)) return false;
            ClientEmailIsFreeRequest request = new ClientEmailIsFreeRequest();
            if (!string.IsNullOrEmpty(email))
            {
                try { request.Email= email; } catch { }
            }
            try { request.Operator = Convert.ToInt16(JwtProps.GetOperator()); } catch { }
            HttpResponseMessage response = HttpClientService.PostAsync("api/client/CheckEmailForFree", request).Result;
            if (response.IsSuccessStatusCode)
            {
                ClientEmailIsFreeResponse data = response.Content.ReadAsAsync<ClientEmailIsFreeResponse>().Result;
                if (string.IsNullOrEmpty(data.Message))
                {
                    return data.IsFree;
                }
            }
            return false;
        }
    }
}
