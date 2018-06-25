using LC_Manager.Implementation;
using LC_Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
                ViewBag.RoleName = JwtProps.GetRole();
            }
            catch { }
            return View();
        }

        [AuthorizeJwt(Roles = "BonusNoChequeList"), HttpPost]
        public string GetBonuses(JQueryDataTableParamModel param)
        {
            return null;
            //    ClientInfoResponse response = new ClientInfoResponse();
            //    try
            //    {
            //        ClientInfoRequest request = new ClientInfoRequest
            //        {
            //            ClientId = JwtProps.GetClient(),
            //            OperatorId = JwtProps.GetOperator()
            //        };
            //        HttpResponseMessage responseMessage = HttpClientService.PostAsync("api/client/GetClient", request).Result;
            //        if (responseMessage.IsSuccessStatusCode)
            //        {
            //            response = responseMessage.Content.ReadAsAsync<ClientInfoResponse>().Result;
            //            if (response.ErrorCode == 0)
            //            {
            //                BonusesNotForPurchasesResponse response2 = new BonusesNotForPurchasesResponse();
            //                BonusesNotForPurchasesRequest request2 = new BonusesNotForPurchasesRequest
            //                {
            //                    Card = response.ClientData.CardNumber,
            //                    Date = Request["columns[1][search][value]"],
            //                    Page = Convert.ToInt64(param.start),
            //                    PageSize = Convert.ToInt64(param.length)
            //                };

            //                if (!string.IsNullOrEmpty(Request["columns[2][search][value]"]))
            //                {
            //                    request2.Name = Request["columns[2][search][value]"]
            //                        .Substring(1, Request["columns[2][search][value]"].Length - 2);
            //                }
            //                if (!string.IsNullOrEmpty(Request["columns[3][search][value]"]))
            //                {
            //                    var values = Request["columns[3][search][value]"].Split('-');
            //                    try { request2.AddedMore = values[0]; } catch { };
            //                    try { request2.AddedLess = values[1]; } catch { };
            //                }
            //                if (!string.IsNullOrEmpty(Request["columns[4][search][value]"]))
            //                {
            //                    var values = Request["columns[4][search][value]"].Split('-');
            //                    try { request2.RedeemedMore = values[0]; } catch { };
            //                    try { request2.RedeemedLess = values[1]; } catch { };
            //                }
            //                if (!string.IsNullOrEmpty(Request["columns[5][search][value]"]))
            //                {
            //                    var values = Request["columns[5][search][value]"].Split('-');
            //                    try { request2.BurnMore = values[0]; } catch { };
            //                    try { request2.BurnLess = values[1]; } catch { };
            //                }

            //                try
            //                {
            //                    request2.Page++;
            //                }
            //                catch { }
            //                HttpResponseMessage responseMessage2 = HttpClientService.PostAsync("api/client/BonusesNotForPurchases", request2).Result;
            //                if (responseMessage2.IsSuccessStatusCode)
            //                {
            //                    try
            //                    {
            //                        response2 = responseMessage2.Content.ReadAsAsync<BonusesNotForPurchasesResponse>().Result;
            //                    }
            //                    catch (Exception e)
            //                    {
            //                        var o = e.Message;
            //                    }
            //                    if (response2.ErrorCode == 0)
            //                    {
            //                        Bonuses bonuses = new Bonuses();
            //                        foreach (Bonus c in response2.Bonuses)
            //                        {
            //                            BonusesViewModel bonus = new BonusesViewModel
            //                            {
            //                                date = "<p>" + c.BonusDate.ToString("dd.MM.yyyy") + "</p> <p>" +
            //                                       c.BonusDate.ToString("HH:mm") + "</p>",
            //                                type = c.BonusSource,
            //                                added = c.BonusAdded.ToString(CultureInfo.InvariantCulture),
            //                                redeemed = c.BonusRedeemed.ToString(CultureInfo.InvariantCulture),
            //                                fireed = c.BonusBurn.ToString(CultureInfo.InvariantCulture),
            //                                lorem = ""
            //                            };
            //                            bonuses.data.Add(bonus);
            //                        }
            //                        bonuses.draw = param.draw;
            //                        bonuses.recordsTotal = response2.RecordTotal;
            //                        bonuses.recordsFiltered = response2.RecordFilterd;
            //                        var data = JsonConvert.SerializeObject(bonuses);
            //                        return data;
            //                    }
            //                    return responseMessage2.ReasonPhrase;
            //                }


            //                #region OldVariant
            //                //CardBonusesResponse response2 = new CardBonusesResponse();
            //                //CardBonusesRequest request2 = new CardBonusesRequest
            //                //{
            //                //    Card = response.ClientData.CardNumber,
            //                //    Operator = Convert.ToInt16(Config.GetOperator())
            //                //};
            //                //HttpResponseMessage responseMessage2 = HttpClientService.PostAsync("api/client/CardBonuses", request2).Result;
            //                //if (responseMessage2.IsSuccessStatusCode)
            //                //{
            //                //    response2 = responseMessage2.Content.ReadAsAsync<CardBonusesResponse>().Result;
            //                //    if (response2.ErrorCode == 0)
            //                //    {
            //                //        Bonuses bonuses = new Bonuses();
            //                //        foreach (var c in response2.CardBonuses)
            //                //        {
            //                //            BonusesViewModel bonus = new BonusesViewModel
            //                //            {
            //                //                date = "<p>" + c.BonusTime.ToString("dd.MM.yyyy") + "</p> <p>" +
            //                //                       c.BonusTime.ToString("HH:mm") + "</p>",
            //                //                type = c.BonusType,
            //                //                added = c.Bonus.ToString(),
            //                //                lorem = ""
            //                //            };
            //                //            bonuses.data.Add(bonus);
            //                //        }
            //                //        var data = JsonConvert.SerializeObject(bonuses);
            //                //        return data;
            //                //    }
            //                //    return responseMessage2.ReasonPhrase;
            //                //} 
            //                #endregion
            //            }
            //        }
            //        response.ErrorCode = 10;
            //        response.Message = "Ошибка получения данных";
            //    }
            //    catch (Exception ex)
            //    {
            //        response.ErrorCode = 2;
            //        response.Message = ex.Message;
            //    }
            //    return JsonConvert.SerializeObject(response);
        }
    }
}