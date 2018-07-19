using LC_Manager.Models;
using System;
using System.Net.Http;
using System.Web.Mvc;
using System.Globalization;
using LCManager.Infrastructure.Request;
using LCManager.Infrastructure.Response;
using LCManager.JWT;
using Newtonsoft.Json;
using Bonus = LCManager.Infrastructure.Data.Bonus;
using ReportResponse = LCManager.Infrastructure.Response.ReportResponse;

namespace LC_Manager.Controllers
{
    public class BonusController : Controller
    {
        /// <summary>
        /// Открывает представление Бонусы не за покупки
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Получает список бонусов не за покупки (с пагинацией)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [AuthorizeJwt(Roles = "BonusNoChequeList"), HttpPost]
        public string GetBonuses(JQueryDataTableParamModel param)
        {
            BonusesNotForPurchasesResponse response = new BonusesNotForPurchasesResponse();
            try
            {
                BonusesNotForPurchasesRequest request = new BonusesNotForPurchasesRequest
                {
                    Operator = Implementation.JwtProps.GetOperator(),
                    Date = Request["columns[1][search][value]"],
                    DateStart = Request["date_from"],
                    DateEnd = Request["date_to"],
                    Phone = Request["columns[3][search][value]"],
                    Page = Convert.ToInt64(param.start),
                    PageSize = Convert.ToInt64(param.length)
                };

                if (!string.IsNullOrEmpty(Request["columns[2][search][value]"]))
                {
                    request.Name = Request["columns[2][search][value]"];
                }
                if (!string.IsNullOrEmpty(Request["columns[5][search][value]"]))
                {
                    var values = Request["columns[5][search][value]"].Split('-');
                    try { request.AddedMore = values[0]; } catch { };
                    try { request.AddedLess = values[1]; } catch { };
                }
                if (!string.IsNullOrEmpty(Request["columns[6][search][value]"]))
                {
                    var values = Request["columns[6][search][value]"].Split('-');
                    try { request.RedeemedMore = values[0]; } catch { };
                    try { request.RedeemedLess = values[1]; } catch { };
                }
                if (!string.IsNullOrEmpty(Request["columns[7][search][value]"]))
                {
                    var values = Request["columns[7][search][value]"].Split('-');
                    try { request.BurnMore = values[0]; } catch { };
                    try { request.BurnLess = values[1]; } catch { };
                }
                try { request.Page++; } catch { }
                HttpResponseMessage responseMessage = HttpClientService.PostAsync("api/client/BonusesNotForPurchases", request).Result;
                if (responseMessage.IsSuccessStatusCode)
                {
                    try { response = responseMessage.Content.ReadAsAsync<BonusesNotForPurchasesResponse>().Result; } catch { }
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
                                card = c.BonusCard.ToString(CultureInfo.CurrentCulture),
                                phone = c.Phone,
                                reason = "Механика ПЛ",
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

        /// <summary>
        /// Отчет по бонусам не за покупки
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="date"></param>
        /// <param name="shop"></param>
        /// <param name="phone"></param>
        /// <param name="operation"></param>
        /// <param name="cheque"></param>
        /// <param name="sum"></param>
        /// <param name="charge"></param>
        /// <param name="redeem"></param>
        /// <returns></returns>
        [AuthorizeJwt]
        [HttpPost]
        public FileResult BonusNoChequeReport(
            string bonus_from,
            string bonus_to,
            string bonus_date,
            string bonus_type,
            string bonus_added,
            string bonus_redeemed,
            string bonus_burned,
            string bonus_phone)
        {
            try
            {
                BonusesNotForPurchasesRequest request = new BonusesNotForPurchasesRequest
                {
                    Operator = Implementation.JwtProps.GetOperator(),
                    Date = bonus_date,
                    DateStart = bonus_from,
                    DateEnd = bonus_to,
                    Phone = bonus_phone,
                    Page = -1,
                    PageSize = -1
                };

                if (!string.IsNullOrEmpty(bonus_type))
                {
                    request.Name = bonus_type.Substring(1, bonus_type.Length - 2);
                }
                if (!string.IsNullOrEmpty(bonus_added))
                {
                    var values = bonus_added.Split('-');
                    try { request.AddedMore = values[0]; } catch { };
                    try { request.AddedLess = values[1]; } catch { };
                }
                if (!string.IsNullOrEmpty(bonus_redeemed))
                {
                    var values = bonus_redeemed.Split('-');
                    try { request.RedeemedMore = values[0]; } catch { };
                    try { request.RedeemedLess = values[1]; } catch { };
                }
                if (!string.IsNullOrEmpty(bonus_burned))
                {
                    var values = bonus_burned.Split('-');
                    try { request.BurnMore = values[0]; } catch { };
                    try { request.BurnLess = values[1]; } catch { };
                }
                ReportResponse response = new ReportResponse();
                HttpResponseMessage responseMessage = HttpClientService.PostAsync("api/reports/BonusesNoChequeReport", request).Result;
                if (responseMessage.IsSuccessStatusCode)
                {
                    response = responseMessage.Content.ReadAsAsync<ReportResponse>().Result;
                    if (response.ErrorCode != 0 || !string.IsNullOrEmpty(response.Message)) return null;
                    var reportName = "Отчёт по бонусам не за покупки";
                    if (!string.IsNullOrEmpty(bonus_from)) reportName = reportName + " с " + bonus_from;
                    if (!string.IsNullOrEmpty(bonus_to)) reportName = reportName + " по " + bonus_to;
                    reportName = reportName + ".xlsx";
                    return File(response.Report, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", reportName);
                }
                return null;
            }
            catch { return null; }
        }
    }
}