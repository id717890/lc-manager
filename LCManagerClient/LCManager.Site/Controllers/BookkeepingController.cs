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

        [HttpGet]
        [AuthorizeJwt]
        public string GetBookkeeping(JQueryDataTableParamModel param)
        {
            BookkeepingsResponse response = new BookkeepingsResponse();
            BookkeepingRequest request = new BookkeepingRequest();

            try { request.Operator = JwtProps.GetOperator(); } catch { }
            try { request.Partner = JwtProps.GetPartner(); } catch { }
            try { request.Pos = JwtProps.GetPos(); } catch { }
            try
            {
                if(!string.IsNullOrEmpty(Request["columns[1][search][value]"]))
                request.Name = Request["columns[1][search][value]"];
                request.DateStart = Request["date_from"];
                request.DateEnd = Request["date_to"];
                request.Page = Convert.ToInt64(param.start);
                request.PageSize = Convert.ToInt64(param.length);

                if (!string.IsNullOrEmpty(Request["columns[2][search][value]"]))
                {
                    var values = Request["columns[2][search][value]"].Split('-');
                    try { request.PurchasesMore = values[0]; } catch { };
                    try { request.PurchasesLess = values[1]; } catch { };
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
                    try { request.ClientsMore = values[0]; } catch { };
                    try { request.ClientsLess = values[1]; } catch { };
                }
                try { request.Page++; } catch { }
                HttpResponseMessage responseMessage = HttpClientService.PostAsync("api/bookkeeping/GetBookkeepings", request).Result;
                if (responseMessage.IsSuccessStatusCode)
                {
                    try { response = responseMessage.Content.ReadAsAsync<BookkeepingsResponse>().Result; } catch { }
                    if (response.ErrorCode == 0)
                    {
                        var longArrayByMonth = new long[12];
                        var decimalArrayByMonth = new decimal[12];

                        BookkeepingDataTableModel bookkeepings = new BookkeepingDataTableModel();
                        foreach (Bookkeeping item in response.Bookkeepings)
                        {
                            longArrayByMonth[0] = item.PurchasesMonth1;
                            longArrayByMonth[1] = item.PurchasesMonth2;
                            longArrayByMonth[2] = item.PurchasesMonth3;
                            longArrayByMonth[3] = item.PurchasesMonth4;
                            longArrayByMonth[4] = item.PurchasesMonth5;
                            longArrayByMonth[5] = item.PurchasesMonth6;
                            longArrayByMonth[6] = item.PurchasesMonth7;
                            longArrayByMonth[7] = item.PurchasesMonth8;
                            longArrayByMonth[8] = item.PurchasesMonth9;
                            longArrayByMonth[9] = item.PurchasesMonth10;
                            longArrayByMonth[10] = item.PurchasesMonth11;
                            longArrayByMonth[11] = item.PurchasesMonth12;
                            var buysArrayString = string.Join(",", longArrayByMonth);

                            longArrayByMonth[0] = item.ClientsMonth1;
                            longArrayByMonth[1] = item.ClientsMonth2;
                            longArrayByMonth[2] = item.ClientsMonth3;
                            longArrayByMonth[3] = item.ClientsMonth4;
                            longArrayByMonth[4] = item.ClientsMonth5;
                            longArrayByMonth[5] = item.ClientsMonth6;
                            longArrayByMonth[6] = item.ClientsMonth7;
                            longArrayByMonth[7] = item.ClientsMonth8;
                            longArrayByMonth[8] = item.ClientsMonth9;
                            longArrayByMonth[9] = item.ClientsMonth10;
                            longArrayByMonth[10] = item.ClientsMonth11;
                            longArrayByMonth[11] = item.ClientsMonth12;
                            var clientsArrayString = string.Join(",", longArrayByMonth);

                            longArrayByMonth[0] = item.AddedMonth1;
                            longArrayByMonth[1] = item.AddedMonth2;
                            longArrayByMonth[2] = item.AddedMonth3;
                            longArrayByMonth[3] = item.AddedMonth4;
                            longArrayByMonth[4] = item.AddedMonth5;
                            longArrayByMonth[5] = item.AddedMonth6;
                            longArrayByMonth[6] = item.AddedMonth7;
                            longArrayByMonth[7] = item.AddedMonth8;
                            longArrayByMonth[8] = item.AddedMonth9;
                            longArrayByMonth[9] = item.AddedMonth10;
                            longArrayByMonth[10] = item.AddedMonth11;
                            longArrayByMonth[11] = item.AddedMonth12;
                            var addedArrayString = string.Join(",", longArrayByMonth);

                            longArrayByMonth[0] = item.RedeemedMonth1;
                            longArrayByMonth[1] = item.RedeemedMonth2;
                            longArrayByMonth[2] = item.RedeemedMonth3;
                            longArrayByMonth[3] = item.RedeemedMonth4;
                            longArrayByMonth[4] = item.RedeemedMonth5;
                            longArrayByMonth[5] = item.RedeemedMonth6;
                            longArrayByMonth[6] = item.RedeemedMonth7;
                            longArrayByMonth[7] = item.RedeemedMonth8;
                            longArrayByMonth[8] = item.RedeemedMonth9;
                            longArrayByMonth[9] = item.RedeemedMonth10;
                            longArrayByMonth[10] = item.RedeemedMonth11;
                            longArrayByMonth[11] = item.RedeemedMonth12;
                            var redeemedArrayString = string.Join(",", longArrayByMonth);

                            BookkeepingViewModel bonus = new BookkeepingViewModel
                            {
                                id = item.Id,
                                caption = item.Caption,
                                purchases = item.Gain.ToString(CultureInfo.InvariantCulture),
                                added = item.Added.ToString(CultureInfo.InvariantCulture),
                                redeemed = item.Redeemed.ToString(CultureInfo.InvariantCulture),
                                clients = item.Clients.ToString(CultureInfo.InvariantCulture),
                                diagrams = @"
                                <div class='line-chart-bl'>
                                    <div class='line-chart__head'>
                                        <div id='line-chart-leg' class='line-chart-legend'></div>
                                    </div>
                                    <div class='line-chart__bottom'><canvas id='canvas' class='line-chart'></canvas>
                                        <div id='chartjs-tooltip-1' class='linejs-tooltip'></div>
                                    </div>
                                </div>
                                <script>
                                        var lineChartData = {
                                            labels: ['Январь', 'Февраль', 'Март', 'Апрель', 'Май', 'Июнь', 'Июль', 'Август', 'Сентябрь', 'Октябрь', 'Ноябрь', 'Декабрь'],
                                            datasets: [
                                                {
                                                    label: 'Покупки',
                                                    borderColor: '#58AEDC',
                                                    pointBackgroundColor: '#58AEDC',
                                                    pointRadius: 2,
                                                    backgroundColor: '#58AEDC',
                                                    data: [" + buysArrayString + @"],
                                                    fill: !1,
                                                    borderWidth: 2,
                                                },
                                                {
                                                    label: 'Начислено',
                                                    borderColor: '#11B9A3',
                                                    pointBackgroundColor: '#11B9A3',
                                                    pointRadius: 2,
                                                    backgroundColor: '#11B9A3',
                                                    data: [" + addedArrayString + @"],
                                                    fill: !1,
                                                    borderWidth: 2,
                                                }, {
                                                    label: 'Списано',
                                                    borderColor: '#E5C861',
                                                    pointBackgroundColor: '#E5C861',
                                                    pointRadius: 2,
                                                    backgroundColor: '#E5C861',
                                                    data: [" + redeemedArrayString + @"],
                                                    fill: !1,
                                                    borderWidth: 2,
                                                }, {
                                                    label: 'Клиенты',
                                                    borderColor: '#567BA5',
                                                    pointBackgroundColor: '#567BA5',
                                                    pointRadius: 2,
                                                    backgroundColor: '#567BA5',
                                                    data: [" + clientsArrayString + @"],
                                                    fill: !1,
                                                    borderWidth: 2,
                                                }
                                                ]
                                        };
                                        typeDiagram = 'line';
                                </script>"
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