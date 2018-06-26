namespace LC_Manager.Controllers
{
    using System;
    using System.Web.Mvc;
    using Implementation;
    using System.Linq;
    using System.Net.Http;
    using Implementation.RequestData;
    using Implementation.ResponseData;
    using Models;
    using Newtonsoft.Json;

    public class PosController : Controller
    {
        // GET: Shop
        [AuthorizeJwt(Roles = "Shops")]
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

        ///// <summary>
        ///// Получает модель для представления, содержащая сразу и список магазинов и списки оператора
        ///// </summary>
        ///// <returns></returns>
        //private PosModel.PosViewModel GetOperatorPosModel()
        //{
        //    var model = new PosModel.PosViewModel();

        //    OperatorPosResponse operatorPosResponse = new OperatorPosResponse();
        //    try
        //    {
        //        OperatorPosRequest operatorPosRequest = new OperatorPosRequest
        //        {
        //            Operator = JwtProps.GetOperator()
        //        };
        //        HttpResponseMessage responseMessage = HttpClientService.PostAsync("api/pos/OperatorPos", operatorPosRequest).Result;
        //        if (responseMessage.IsSuccessStatusCode)
        //        {
        //            operatorPosResponse = responseMessage.Content.ReadAsAsync<OperatorPosResponse>().Result;
        //            if (operatorPosResponse.ErrorCode == 0 && string.IsNullOrEmpty(operatorPosResponse.Message))
        //            {
        //                //model.Poses = operatorPosResponse.Poses;
        //                //model.OperatorPosLists = operatorPosResponse.PosLists;
        //            }
        //        }
        //        else
        //        {
        //            operatorPosResponse.ErrorCode = 10;
        //            operatorPosResponse.Message = "Ошибка получения данных";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        operatorPosResponse.ErrorCode = 10;
        //        operatorPosResponse.Message = ex.Message;
        //    }
        //    return model;
        //}

        //Удаление списка ТТ через Ajax
        [AuthorizeJwt(Roles = "ShopsCreateShopList")]
        public JsonResult OperatorPosListRemove(Int16 id)
        {
            DefaultResponse defaultResponse = new DefaultResponse();
            try
            {
                OperatorPosRemoveRequest operatorPosRemoveRequest = new OperatorPosRemoveRequest
                {
                    OperatorPosList = id
                };
                HttpResponseMessage responseMessage = HttpClientService.PostAsync("api/pos/RemoveOperatorPosList", operatorPosRemoveRequest).Result;
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

        //Сохранение списка ТТ через Ajax
        [AuthorizeJwt(Roles = "ShopsCreateShopList")]
        public string OperatorPosListSave(string list_name, string list_id)
        {
            var operatorPosListResponse = new OperatorPosListResponse();
            try
            {
                var data = new OperatorPosListCreateRequest
                {
                    Operator = JwtProps.GetOperator(),
                    PosList = list_id.Split(',').Select(n => Convert.ToInt32(n)).ToList(),
                    PosListName = list_name
                };
                HttpResponseMessage responseMessage = HttpClientService.PostAsync("api/pos/SaveOperatorPosList", data).Result;
                if (responseMessage.IsSuccessStatusCode)
                {
                    operatorPosListResponse = responseMessage.Content.ReadAsAsync<OperatorPosListResponse>().Result;
                    if (operatorPosListResponse.ErrorCode == 0 && string.IsNullOrEmpty(operatorPosListResponse.Message))
                    {
                        return JsonConvert.SerializeObject(new { success = true });
                    }
                    return JsonConvert.SerializeObject(operatorPosListResponse);
                }
                operatorPosListResponse.ErrorCode = 10;
                operatorPosListResponse.Message = "Ошибка получения данных";
            }
            catch (Exception ex)
            {
                operatorPosListResponse.ErrorCode = 2;
                operatorPosListResponse.Message = ex.Message;
            }
            return JsonConvert.SerializeObject(operatorPosListResponse);
        }

        //Подгузка ТТ через Ajax для Jquery DataTable
        [AuthorizeJwt(Roles = "ShopsMyShops")]
        public string GetOperatorPos()
        {
            OperatorPosResponse operatorPosResponse = new OperatorPosResponse();
            try
            {
                OperatorPosRequest operatorPosRequest = new OperatorPosRequest
                {
                    Operator = JwtProps.GetOperator()
                };
                HttpResponseMessage responseMessage = HttpClientService.PostAsync("api/pos/OperatorPos", operatorPosRequest).Result;
                if (responseMessage.IsSuccessStatusCode)
                {
                    operatorPosResponse = responseMessage.Content.ReadAsAsync<OperatorPosResponse>().Result;
                    if (operatorPosResponse.ErrorCode == 0 && string.IsNullOrEmpty(operatorPosResponse.Message))
                    {
                        PosModel.PosData data = new PosModel.PosData();
                        int i = 0;
                        foreach (var c in operatorPosResponse.Poses)
                        {
                            i++;
                            var item = new LC_Manager.Implementation.Data.Pos
                            {
                                Check = "<input type='checkbox' class='checkbox checkbox-for-shop' name='checkbox[]" + c.Id + "' id='checkbox" + c.Id + "' value='" + c.Id + "'><label for='checkbox" + c.Id + "'></label>",
                                Id = c.Id,
                                Address = c.Address,
                                City = c.City,
                                Region = c.Region
                            };
                            data.data.Add(item);
                        }
                        return JsonConvert.SerializeObject(data);
                    }
                    return JsonConvert.SerializeObject(operatorPosResponse);
                }
                else
                {
                    operatorPosResponse.ErrorCode = 10;
                    operatorPosResponse.Message = "Ошибка получения данных";
                }
            }
            catch (Exception ex)
            {
                operatorPosResponse.ErrorCode = 2;
                operatorPosResponse.Message = ex.Message;
            }
            return JsonConvert.SerializeObject(operatorPosResponse);
        }

        //Подгузка списка списков ТТ через Ajax для Jquery DataTable
        [AuthorizeJwt(Roles = "ShopsCreateShopList")]
        public string GetListOfPos()
        {
            OperatorPosListResponse operatorPosResponse = new OperatorPosListResponse();
            try
            {
                OperatorPosRequest operatorPosRequest = new OperatorPosRequest
                {
                    Operator = JwtProps.GetOperator()
                };
                HttpResponseMessage responseMessage = HttpClientService.PostAsync("api/pos/OperatorPosList", operatorPosRequest).Result;
                if (responseMessage.IsSuccessStatusCode)
                {
                    operatorPosResponse = responseMessage.Content.ReadAsAsync<OperatorPosListResponse>().Result;
                    if (operatorPosResponse.ErrorCode == 0 && string.IsNullOrEmpty(operatorPosResponse.Message))
                    {
                        PosModel.PosListData data = new PosModel.PosListData();
                        int i = 0;
                        foreach (var c in operatorPosResponse.PosLists)
                        {
                            i++;
                            #region Формируем выпадающий список для списка ТТ
                            var detailList = c.Poses.Aggregate(string.Empty, (current, pos) => current + (@"<tr>
                                        <td colspan='2' style='color:#000;'>" + pos.City + @"</td>
                                        <td colspan='2' style='color:#000;'>" + pos.Address + @"</td>
                                      </tr>"));
                            #endregion
                            Implementation.Data.OperatorPosList g = new Implementation.Data.OperatorPosList
                            {
                                Id = c.Id,
                                Caption = c.Caption,
                                Text = c.Poses.Count().ToString(),
                                Poses = c.Poses,
                                HtmlForDelete = "<img class='dell-control' data-id=" + c.Id + " src='/img/dell.png'>",
                                details = detailList
                            };
                            data.data.Add(g);
                        }
                        return JsonConvert.SerializeObject(data);
                    }
                    return JsonConvert.SerializeObject(operatorPosResponse);
                }
                operatorPosResponse.ErrorCode = 10;
                operatorPosResponse.Message = "Ошибка получения данных";
            }
            catch (Exception ex)
            {
                operatorPosResponse.ErrorCode = 2;
                operatorPosResponse.Message = ex.Message;
            }
            return JsonConvert.SerializeObject(operatorPosResponse);
        }
    }
}