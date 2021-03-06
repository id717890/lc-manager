﻿using System;
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
using ReportResponse = LCManager.Infrastructure.Response.ReportResponse;

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
                if (!string.IsNullOrEmpty(Request["columns[1][search][value]"]))
                    request.Name = Request["columns[1][search][value]"];
                request.DateStart = Request["date_from"];
                request.DateEnd = Request["date_to"];
                request.Page = Convert.ToInt64(param.start);
                request.PageSize = Convert.ToInt64(param.length);

                var i = 0;
                if (request.IsOperator)
                {
                    i = 1;
                    if (!string.IsNullOrEmpty(Request["columns[2][search][value]"]))
                    {
                        request.PosName = Request["columns[2][search][value]"];
                    }
                }

                if (!string.IsNullOrEmpty(Request["columns[" + (2 + i) + "][search][value]"]))
                {
                    var values = Request["columns[" + (2 + i) + "][search][value]"].Split('-');
                    try { request.PurchasesMore = values[0]; } catch { };
                    try { request.PurchasesLess = values[1]; } catch { };
                }
                if (!string.IsNullOrEmpty(Request["columns[" + (3 + i) + "][search][value]"]))
                {
                    var values = Request["columns[" + (3 + i) + "][search][value]"].Split('-');
                    try { request.AddedMore = values[0]; } catch { };
                    try { request.AddedLess = values[1]; } catch { };
                }
                if (!string.IsNullOrEmpty(Request["columns[" + (4 + i) + "][search][value]"]))
                {
                    var values = Request["columns[" + (4 + i) + "][search][value]"].Split('-');
                    try { request.RedeemedMore = values[0]; } catch { };
                    try { request.RedeemedLess = values[1]; } catch { };
                }
                if (!string.IsNullOrEmpty(Request["columns[" + (5 + i) + "][search][value]"]))
                {
                    var values = Request["columns[" + (5 + i) + "][search][value]"].Split('-');
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
                            longArrayByMonth[0] = item.GainMonth1;
                            longArrayByMonth[1] = item.GainMonth2;
                            longArrayByMonth[2] = item.GainMonth3;
                            longArrayByMonth[3] = item.GainMonth4;
                            longArrayByMonth[4] = item.GainMonth5;
                            longArrayByMonth[5] = item.GainMonth6;
                            longArrayByMonth[6] = item.GainMonth7;
                            longArrayByMonth[7] = item.GainMonth8;
                            longArrayByMonth[8] = item.GainMonth9;
                            longArrayByMonth[9] = item.GainMonth10;
                            longArrayByMonth[10] = item.GainMonth11;
                            longArrayByMonth[11] = item.GainMonth12;
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
                                posname = item.PosName,
                                purchases = item.Gain.ToString(CultureInfo.InvariantCulture),
                                added = item.Added.ToString(CultureInfo.InvariantCulture),
                                redeemed = item.Redeemed.ToString(CultureInfo.InvariantCulture),
                                clients = item.Clients.ToString(CultureInfo.InvariantCulture),
                                diagrams = @"
                                <div class='line-chart-bl' style='padding-bottom: 20px;'>
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
                                                    label: 'Выручка',
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

        /// <summary>
        /// Отчет по бонусам не за покупки
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="name"></param>
        /// <param name="buys"></param>
        /// <param name="added"></param>
        /// <param name="redeemed"></param>
        /// <param name="clients"></param>
        /// <returns></returns>
        [AuthorizeJwt]
        [HttpPost]
        public FileResult BookkeepingReport(
            string from,
            string to,
            string name,
            string buys,
            string added,
            string redeemed,
            string clients)
        {
            ReportResponse response = new ReportResponse();
            BookkeepingRequest request = new BookkeepingRequest();

            try { request.Operator = JwtProps.GetOperator(); } catch { }
            try { request.Partner = JwtProps.GetPartner(); } catch { }
            try { request.Pos = JwtProps.GetPos(); } catch { }
            try
            {
                if (!string.IsNullOrEmpty(name)) request.Name = name;
                request.DateStart = from;
                request.DateEnd = to;
                request.Page = -1;
                request.PageSize = -1;

                if (!string.IsNullOrEmpty(buys))
                {
                    var values = buys.Split('-');
                    try { request.PurchasesMore = values[0]; } catch { };
                    try { request.PurchasesLess = values[1]; } catch { };
                }
                if (!string.IsNullOrEmpty(added))
                {
                    var values = added.Split('-');
                    try { request.AddedMore = values[0]; } catch { };
                    try { request.AddedLess = values[1]; } catch { };
                }
                if (!string.IsNullOrEmpty(redeemed))
                {
                    var values = redeemed.Split('-');
                    try { request.RedeemedMore = values[0]; } catch { };
                    try { request.RedeemedLess = values[1]; } catch { };
                }
                if (!string.IsNullOrEmpty(clients))
                {
                    var values = clients.Split('-');
                    try { request.ClientsMore = values[0]; } catch { };
                    try { request.ClientsLess = values[1]; } catch { };
                }
                HttpResponseMessage responseMessage = HttpClientService.PostAsync("api/reports/BookkeepingReport", request).Result;
                if (responseMessage.IsSuccessStatusCode)
                {
                    response = responseMessage.Content.ReadAsAsync<ReportResponse>().Result;
                    if (response.ErrorCode != 0 || !string.IsNullOrEmpty(response.Message)) return null;
                    var reportName = "Отчёт по сверке";
                    if (!string.IsNullOrEmpty(from)) reportName = reportName + " с " + from;
                    if (!string.IsNullOrEmpty(to)) reportName = reportName + " по " + to;
                    reportName = reportName + ".xlsx";
                    return File(response.Report, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", reportName);
                }
                return null;
            }
            catch { return null; }
        }
    }
}