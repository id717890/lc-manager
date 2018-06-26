namespace LC_Manager.Controllers
{
    using System.Collections.Generic;
    using Implementation.Data;
    using OfficeOpenXml;
    using Implementation;
    using Implementation.RequestData;
    using Implementation.ResponseData;
    using Models;
    using Newtonsoft.Json;
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Web;
    using System.Web.Mvc;

    public class GoodController : Controller
    {
        [AuthorizeJwt(Roles = "Goods")]
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

        [AuthorizeJwt(Roles = "GoodsMyGoods")]
        public string GetGoods()
        {
            OperatorGoodsResponse operatorGoodsResponse = new OperatorGoodsResponse();
            try
            {
                OperatorGoodsRequest operatorGoodsRequest = new OperatorGoodsRequest
                {
                    Operator = JwtProps.GetOperator()
                };
                HttpResponseMessage responseMessage = HttpClientService.PostAsync("api/values/OperatorGoods", operatorGoodsRequest).Result;
                if (responseMessage.IsSuccessStatusCode)
                {
                    operatorGoodsResponse = responseMessage.Content.ReadAsAsync<OperatorGoodsResponse>().Result;
                    if (operatorGoodsResponse.ErrorCode == 0 && string.IsNullOrEmpty(operatorGoodsResponse.Message))
                    {
                        goodsdata data = new goodsdata();
                        foreach (var c in operatorGoodsResponse.OperatorGoods)
                        {
                            good g = new good
                            {
                                chek = "<input type='checkbox' class='checkbox checkbox-for-good' name='checkbox[]" + c.Id + "' id='checkbox" + c.Id + "' value='" + c.Id + "'><label for='checkbox" + c.Id + "'></label>",
                                id = c.Id.ToString(),
                                name = c.Code,
                                text = c.Name
                            };
                            data.data.Add(g);
                        }
                        return JsonConvert.SerializeObject(data);
                    }
                    return JsonConvert.SerializeObject(operatorGoodsResponse);
                }
                else
                {
                    operatorGoodsResponse.ErrorCode = 10;
                    operatorGoodsResponse.Message = "Ошибка получения данных";
                }
            }
            catch (Exception ex)
            {
                operatorGoodsResponse.ErrorCode = 2;
                operatorGoodsResponse.Message = ex.Message;
            }
            return JsonConvert.SerializeObject(operatorGoodsResponse);
        }

        //Сохранение списка товаров через Ajax
        [AuthorizeJwt(Roles = "GoodsMyGoodsList")]
        public string OperatorGoodListSave(string list_name, string list_id)
        {
            var operatorGoodListResponse = new OperatorGoodListResponse();
            try
            {
                var data = new OperatorGoodListCreateRequest
                {
                    Operator = JwtProps.GetOperator(),
                    GoodList = list_id.Split(',').Select(n => Convert.ToInt32(n)).ToList(),
                    GoodListName = list_name
                };
                HttpResponseMessage responseMessage = HttpClientService.PostAsync("api/values/SaveOperatorGoodList", data).Result;
                if (responseMessage.IsSuccessStatusCode)
                {
                    operatorGoodListResponse = responseMessage.Content.ReadAsAsync<OperatorGoodListResponse>().Result;
                    if (operatorGoodListResponse.ErrorCode == 0 && string.IsNullOrEmpty(operatorGoodListResponse.Message))
                    {
                        return JsonConvert.SerializeObject(new { success = true });
                    }
                    return JsonConvert.SerializeObject(operatorGoodListResponse);
                }
                operatorGoodListResponse.ErrorCode = 10;
                operatorGoodListResponse.Message = "Ошибка получения данных";
            }
            catch (Exception ex)
            {
                operatorGoodListResponse.ErrorCode = 2;
                operatorGoodListResponse.Message = ex.Message;
            }
            return JsonConvert.SerializeObject(operatorGoodListResponse);
        }

        //Подгузка списка списков товаров через Ajax для Jquery DataTable
        [AuthorizeJwt(Roles = "GoodsMyGoodsList")]
        public string OpretorGoodListGet()
        {
            OperatorGoodListResponse operatorGoodResponse = new OperatorGoodListResponse();
            try
            {
                OperatorGoodsRequest operatorGoodRequest = new OperatorGoodsRequest
                {
                    Operator = JwtProps.GetOperator()
                };
                HttpResponseMessage responseMessage = HttpClientService.PostAsync("api/values/OperatorGoodList", operatorGoodRequest).Result;
                if (responseMessage.IsSuccessStatusCode)
                {
                    operatorGoodResponse = responseMessage.Content.ReadAsAsync<OperatorGoodListResponse>().Result;
                    if (operatorGoodResponse.ErrorCode == 0 && string.IsNullOrEmpty(operatorGoodResponse.Message))
                    {
                        GoodModel.GoodListData data = new GoodModel.GoodListData();
                        foreach (var c in operatorGoodResponse.GoodLists)
                        {
                            #region Формируем выпадающий список для списка ТТ
                            var detailList = c.Goods.Aggregate(string.Empty, (current, good) => current + (@"<tr>
                                        <td colspan='2' style='color:#000;'>" + good.Code + @"</td>
                                        <td colspan='2' style='color:#000;'>" + good.Name+ @"</td>
                                      </tr>"));
                            #endregion
                            Implementation.Data.OperatorGoodList g = new Implementation.Data.OperatorGoodList
                            {
                                Id = c.Id,
                                Name = c.Name,
                                Text = c.Goods.Count().ToString(),
                                Goods = c.Goods,
                                HtmlForDelete = "<img class='dell-control' data-id=" + c.Id + " src='/img/dell.png'>",
                                details = detailList
                            };
                            data.data.Add(g);
                        }
                        return JsonConvert.SerializeObject(data);
                    }
                    return JsonConvert.SerializeObject(operatorGoodResponse);
                }
                operatorGoodResponse.ErrorCode = 10;
                operatorGoodResponse.Message = "Ошибка получения данных";
            }
            catch (Exception ex)
            {
                operatorGoodResponse.ErrorCode = 2;
                operatorGoodResponse.Message = ex.Message;
            }
            return JsonConvert.SerializeObject(operatorGoodResponse);
        }

        //Удаление списка товаров через Ajax
        [AuthorizeJwt(Roles = "GoodsMyGoodsList")]
        public JsonResult OperatorGoodListRemove(Int16 id)
        {
            DefaultResponse defaultResponse = new DefaultResponse();
            try
            {
                OperatorGoodRemoveRequest operatorGoodRemoveRequest = new OperatorGoodRemoveRequest
                {
                    OperatorGoodList = id
                };
                HttpResponseMessage responseMessage = HttpClientService.PostAsync("api/values/RemoveOperatorGoodList", operatorGoodRemoveRequest).Result;
                if (responseMessage.IsSuccessStatusCode)
                {
                    defaultResponse = responseMessage.Content.ReadAsAsync<DefaultResponse>().Result;
                    if (defaultResponse.ErrorCode == 0 && string.IsNullOrEmpty(defaultResponse.Message))
                    {
                        return Json(new { success = true });
                    }
                    return Json(new { success = false, error = JsonConvert.SerializeObject(defaultResponse) });
                }
                defaultResponse.ErrorCode = 10;
                defaultResponse.Message = "Ошибка удаления данных";
            }
            catch (Exception ex)
            {
                defaultResponse.ErrorCode = 10;
                defaultResponse.Message = ex.Message;
            }
            return Json(new { success = false, error = JsonConvert.SerializeObject(defaultResponse) });
        }

        [HttpPost]
        [AuthorizeJwt(Roles = "GoodsMyGoodsList")]
        public JsonResult OperatorGoodImportFromExcel(HttpPostedFileBase file)
        {
            if (Request.Files.Count > 0)
            {
                var goodList = new List<Good>();
                using (var excel = new ExcelPackage(Request.Files[0].InputStream))
                {
                    var sheet = excel.Workbook.Worksheets.First();
                    for (var rowNum = 2; rowNum <= sheet.Dimension.End.Row; rowNum++)
                    {
                        try
                        {
                            goodList.Add(new Good
                            {
                                Code = sheet.Cells[rowNum, 2].Text,
                                Name = sheet.Cells[rowNum, 1].Text
                            });
                        }
                        catch { }
                    }
                }
                OperatorGoodImportResponse operatorGoodImportResponse = new OperatorGoodImportResponse();
                try
                {
                    short partner = JwtProps.GetPartner();
                    if(partner == 0)
                    {
                        partner = JwtProps.GetDefaultPartner();
                    }
                    var data = new OperatorGoodImportRequest
                    {
                        Partner = partner,
                        Goods = goodList
                    };
                    HttpResponseMessage responseMessage = HttpClientService.PostAsync("api/values/ImportOperatorGoods", data).Result;
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        operatorGoodImportResponse = responseMessage.Content.ReadAsAsync<OperatorGoodImportResponse>().Result;
                        if (operatorGoodImportResponse.ErrorCode == 0 && string.IsNullOrEmpty(operatorGoodImportResponse.Message))
                        {
                            return Json(new { success = true });

                        }
                        return Json(new { success = false });
                    }
                    operatorGoodImportResponse.ErrorCode = 10;
                    operatorGoodImportResponse.Message = "Ошибка импорта данных";

                }
                catch (Exception e)
                {
                }





                //var reader = new StreamReader(Request.Files[0].InputStream);
                //var ex=new ExcelPackage(Request.Files[0].InputStream);
                //var book = ex.
                //    Factory.GetWorkbookSet().Workbooks.OpenFromStream(Request.Files[0].InputStream);
                //var sheet = book.ActiveWorksheet;

                //var usedColumns = sheet.UsedRange.ColumnCount;
                //var usedRows = sheet.UsedRange.RowCount;


                //for (var row = 0; row < usedRows; row++)
                //{
                //    for (var col = 0; col < usedColumns; col++)
                //    {
                //        var iss = sheet.Cells[row, col].Text;
                //    }
                //}

                //book.Close();





                return Json(new { success = true });
            }

            //ПОКА ЗАГЛУШКА ВОЗВРАЩАЮЩАЯ УСПЕХ
            //После уточнения механизма загрузки данных будет реализовано
            return Json(new { success = true });
        }

        [HttpGet]
        [AuthorizeJwt(Roles = "GoodsMyGoodsList")]
        public FileResult OperatorGoodDownloadTemplate()
        {
            return File(Server.MapPath("~/Templates/goods.xltx"), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Товары (шаблон).xltx");
        }
    }
}