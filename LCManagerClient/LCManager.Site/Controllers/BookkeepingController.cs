using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using LCManager.Infrastructure.Data;
using LCManager.Infrastructure.Request;
using LCManager.Infrastructure.Response;
using LCManager.JWT;
using LC_Manager.Models;
using Newtonsoft.Json;

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

        [AuthorizeJwt(Roles = "Total"), HttpPost]
        public string GetBookkeeping(JQueryDataTableParamModel param)
        {
            BookkeepingsResponse response = new BookkeepingsResponse();
            BookkeepingRequest request = new BookkeepingRequest();
            try { request.Operator = JwtProps.GetOperator();} catch { }
            try { request.Partner = JwtProps.GetPartner();} catch { }
            try { request.Pos = JwtProps.GetPos(); } catch { }
            try
            {
                //BookkeepingRequest request = new BookkeepingRequest
                //{
                //    Operator = JwtProps.GetOperator(),
                //    Partner = JwtProps.GetPartner(),
                //    Pos = JwtProps.GetPos(),
                //    //DateStart = Request["date_from"],
                //    //DateEnd = Request["date_to"],
                //    //Page = Convert.ToInt64(param.start),
                //    //PageSize = Convert.ToInt64(param.length)
                //};

                //if (!string.IsNullOrEmpty(Request["columns[2][search][value]"]))
                //{
                //    request.Name = Request["columns[2][search][value]"]
                //        .Substring(1, Request["columns[2][search][value]"].Length - 2);
                //}
                //if (!string.IsNullOrEmpty(Request["columns[3][search][value]"]))
                //{
                //    var values = Request["columns[3][search][value]"].Split('-');
                //    try { request.AddedMore = values[0]; } catch { };
                //    try { request.AddedLess = values[1]; } catch { };
                //}
                //if (!string.IsNullOrEmpty(Request["columns[4][search][value]"]))
                //{
                //    var values = Request["columns[4][search][value]"].Split('-');
                //    try { request.RedeemedMore = values[0]; } catch { };
                //    try { request.RedeemedLess = values[1]; } catch { };
                //}
                //if (!string.IsNullOrEmpty(Request["columns[5][search][value]"]))
                //{
                //    var values = Request["columns[5][search][value]"].Split('-');
                //    try { request.BurnMore = values[0]; } catch { };
                //    try { request.BurnLess = values[1]; } catch { };
                //}
                try { request.Page++; } catch { }
                HttpResponseMessage responseMessage = HttpClientService.PostAsync("api/bookkeeping/GetBookkeepings", request).Result;
                if (responseMessage.IsSuccessStatusCode)
                {
                    try { response = responseMessage.Content.ReadAsAsync<BookkeepingsResponse>().Result; } catch { }
                    if (response.ErrorCode == 0)
                    {
                        BookkeepingDataTableModel bookkeepings = new BookkeepingDataTableModel();
                        foreach (Bookkeeping c in response.Bookkeepings)
                        {
                            BookkeepingViewModel bonus = new BookkeepingViewModel
                            {
                                id = c.Id,
                                caption = c.Caption,
                                purchases = c.Purchases.ToString(CultureInfo.InvariantCulture),
                                added = c.Added.ToString(CultureInfo.InvariantCulture),
                                redeemed = c.Redeemed.ToString(CultureInfo.InvariantCulture),
                                clients = c.Clients.ToString(CultureInfo.InvariantCulture),
                                diagrams = ""
                            };
                            bookkeepings.data.Add(bonus);
                        }
                        bookkeepings.draw = param.draw;
                        bookkeepings.recordsTotal = response.RecordTotal;
                        bookkeepings.recordsFiltered = response.RecordFilterd;
                        var data = JsonConvert.SerializeObject(bookkeepings);
                        return data;
                    }
                    return responseMessage.ReasonPhrase;
                }
                response.ErrorCode = 10;
                response.Message = "Ошибка получения данных";
            }
            catch (Exception ex)
            {
                response.ErrorCode = 2;
                response.Message = ex.Message;
            }
            return JsonConvert.SerializeObject(response);
        }
    }
}