using Site.Infrastrucure.Data;
using LC_Manager.Models;
using Site.Infrastrucure.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using LCManager.Infrastructure.Request;
using LCManager.Infrastructure.Response;
using LCManager.JWT;
using Newtonsoft.Json;
using Bonus = LCManager.Infrastructure.Data.Bonus;

//using LC_Manager.Implementation;

namespace LC_Manager.Controllers
{
    public class BonusController : Controller
    {
        // GET: Bonus
        [AuthorizeJwt(Roles = "BonusNoCheque")]
        public ActionResult Index()
        {
            try
            {
                ViewBag.OperatorName = Convert.ToString(HttpContext.Request.Cookies["operatorName"].Value);
                ViewBag.RoleName = LC_Manager.Implementation.JwtProps.GetRole();
            }
            catch { }
            return View();
        }

        [AuthorizeJwt(Roles = "BonusNoChequeList"), HttpPost]
        public string GetBonuses(JQueryDataTableParamModel param)
        {
            BonusesNotForPurchasesResponse response = new BonusesNotForPurchasesResponse();
            try
            {
                BonusesNotForPurchasesRequest request = new BonusesNotForPurchasesRequest
                {
                    Operator = Implementation.JwtProps.GetOperator(),
                    //Card = response.ClientData.CardNumber,
                    Date = Request["columns[1][search][value]"],
                    DateStart = Request["date_from"],
                    DateEnd = Request["date_to"],
                    Page = Convert.ToInt64(param.start),
                    PageSize = Convert.ToInt64(param.length)
                };

                if (!string.IsNullOrEmpty(Request["columns[2][search][value]"]))
                {
                    request.Name = Request["columns[2][search][value]"]
                        .Substring(1, Request["columns[2][search][value]"].Length - 2);
                }
                if (!string.IsNullOrEmpty(Request["columns[3][search][value]"]))
                {
                    var values = Request["columns[3][search][value]"].Split('-');
                    try { request.AddedMore = values[0]; } catch { };
                    try { request.AddedLess = values[1]; } catch { };
                }
                if (!string.IsNullOrEmpty(Request["columns[4][search][value]"]))
                {
                    var values = Request["columns[4][search][value]"].Split('-');
                    try { request.RedeemedMore = values[0]; } catch { };
                    try { request.RedeemedLess = values[1]; } catch { };
                }
                if (!string.IsNullOrEmpty(Request["columns[5][search][value]"]))
                {
                    var values = Request["columns[5][search][value]"].Split('-');
                    try { request.BurnMore = values[0]; } catch { };
                    try { request.BurnLess = values[1]; } catch { };
                }
                try { request.Page++; } catch { }
                HttpResponseMessage responseMessage2 = HttpClientService.PostAsync("api/client/BonusesNotForPurchases", request).Result;
                if (responseMessage2.IsSuccessStatusCode)
                {
                    try
                    {
                        response = responseMessage2.Content.ReadAsAsync<BonusesNotForPurchasesResponse>().Result;
                    }
                    catch (Exception e)
                    {
                        var o = e.Message;
                    }
                    if (response.ErrorCode == 0)
                    {
                        Bonuses bonuses = new Bonuses();
                        foreach (Bonus c in response.Bonuses)
                        {
                            BonusesViewModel bonus = new BonusesViewModel
                            {
                                date = "<p>" + c.BonusDate.ToString("dd.MM.yyyy") + "</p> <p>" +
                                       c.BonusDate.ToString("HH:mm") + "</p>",
                                type = c.BonusSource,
                                added = c.BonusAdded.ToString(CultureInfo.InvariantCulture),
                                redeemed = c.BonusRedeemed.ToString(CultureInfo.InvariantCulture),
                                fireed = c.BonusBurn.ToString(CultureInfo.InvariantCulture),
                                lorem = ""
                            };
                            bonuses.data.Add(bonus);
                        }
                        bonuses.draw = param.draw;
                        bonuses.recordsTotal = response.RecordTotal;
                        bonuses.recordsFiltered = response.RecordFilterd;
                        var data = JsonConvert.SerializeObject(bonuses);
                        return data;
                    }
                    return responseMessage2.ReasonPhrase;
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