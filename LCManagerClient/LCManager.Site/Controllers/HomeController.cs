namespace LC_Manager.Controllers
{
    using Models;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Implementation;
    using System.Web;
    using System.IO;

    public class HomeController : Controller
    {
        [AuthorizeJwt(Roles = "Analytics")]
        public ActionResult Index()
        {
            try
            {
                ViewBag.OperatorName = Convert.ToString(HttpContext.Request.Cookies["operatorName"].Value);
                ViewBag.RoleName = JwtProps.GetRole();
            }
            catch { }

            //ClientAnalyticMoneyRequest moneyRequest = new ClientAnalyticMoneyRequest
            //{
            //    Operator = Convert.ToInt16(JwtProps.GetOperator())
            //};
            //try
            //{
            //    moneyRequest.Partner = JwtProps.GetPartner();
            //    moneyRequest.Pos = JwtProps.GetPos();
            //}
            //catch { }
            //var t = JwtProps.GetToken();
            //HttpResponseMessage responseMessage = HttpClientService.PostAsync("api/values/ClientAnalyticMoney", moneyRequest).Result;
            //ClientAnalyticMoneyResponse moneyResponse = new ClientAnalyticMoneyResponse();
            //if (responseMessage.IsSuccessStatusCode)
            //{
            //    moneyResponse = responseMessage.Content.ReadAsAsync<ClientAnalyticMoneyResponse>().Result;
            //    if (moneyResponse.AddedBonus != 0)
            //    {
            //        moneyResponse.AvgDiscount = moneyResponse.RedeemedBonus / moneyResponse.AddedBonus;
            //    }
            //    else
            //    {
            //        moneyResponse.AvgDiscount = 0;
            //    }
            //}
            return View();
        }

        [AuthorizeJwt(Roles = "Sales")]
        public ActionResult Sales()
        {
            try
            {
                ViewBag.OperatorName = Convert.ToString(HttpContext.Request.Cookies["operatorName"].Value);
                ViewBag.RoleName = JwtProps.GetRole();
            }
            catch { }
            return View();
        }

        [AuthorizeJwt(Roles = "Clients")]
        public ActionResult Clients()
        {
            try
            {
                ViewBag.OperatorName = Convert.ToString(HttpContext.Request.Cookies["operatorName"].Value);
                ViewBag.RoleName = JwtProps.GetRole();
            }
            catch { }
            return View();
        }

        [AuthorizeJwt(Roles = "Terminal")]
        public ActionResult Terminal()
        {
            try
            {
                ViewBag.OperatorName = Convert.ToString(HttpContext.Request.Cookies["operatorName"].Value);
                ViewBag.RoleName = JwtProps.GetRole();
                //string connectionstring = ConfigurationManager.ConnectionStrings["Partner"].ConnectionString;
                //HttpClient client = new HttpClient();
                //client.BaseAddress = new Uri(connectionstring);
                //client.DefaultRequestHeaders.Accept.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                //var token = HttpContext.Request.Cookies["lcmanageruserdata"].Value;
                //client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                //ClientAnalyticMoneyRequest moneyRequest = new ClientAnalyticMoneyRequest
                //{
                //    Operator = Convert.ToInt16(JwtProps.GetOperator())
                //};
                //HttpResponseMessage responseMessage = HttpClientService.PostAsync("api/values/ClientAnalyticMoney", moneyRequest).Result;
                //ClientAnalyticMoneyResponse moneyResponse = new ClientAnalyticMoneyResponse();
                //if (responseMessage.IsSuccessStatusCode)
                //{
                //    moneyResponse = responseMessage.Content.ReadAsAsync<ClientAnalyticMoneyResponse>().Result;
                //    ViewBag.ClientQty = moneyResponse.ClientQty;
                //}
            }
            catch { }
            return View();
        }

        [AuthorizeJwt(Roles = "Campaign")]
        public ActionResult Campaigns()
        {
            try
            {
                ViewBag.OperatorName = Convert.ToString(HttpContext.Request.Cookies["operatorName"].Value);
                ViewBag.RoleName = JwtProps.GetRole();
            }
            catch { }
            return View();
        }

        [AuthorizeJwt(Roles = "Mailing")]
        public ActionResult Distributions()
        {
            try
            {
                ViewBag.OperatorName = Convert.ToString(HttpContext.Request.Cookies["operatorName"].Value);
                ViewBag.RoleName = JwtProps.GetRole();
            }
            catch { }
            return View();
        }

        [AuthorizeJwt(Roles = "Writer, Admin")]
        public ActionResult Applications()
        {
            try
            {
                ViewBag.OperatorName = Convert.ToString(HttpContext.Request.Cookies["operatorName"].Value);
                ViewBag.RoleName = JwtProps.GetRole();
            }
            catch { }
            return View();
        }

        [AuthorizeJwt(Roles = "Profile")]
        public ActionResult Profile()
        {
            try
            {
                ViewBag.OperatorName = Convert.ToString(HttpContext.Request.Cookies["operatorName"].Value);
                ViewBag.RoleName = JwtProps.GetRole();
            }
            catch { }
            return View();
        }

        [AuthorizeJwt(Roles = "Total")]
        public ActionResult Bookkeeping()
        {
            try
            {
                ViewBag.OperatorName = Convert.ToString(HttpContext.Request.Cookies["operatorName"].Value);
                ViewBag.RoleName = JwtProps.GetRole();
            }
            catch { }
            return View();
        }

        [AuthorizeJwt(Roles = "Faq")]
        public ActionResult FAQ()
        {
            try
            {
                ViewBag.OperatorName = Convert.ToString(HttpContext.Request.Cookies["operatorName"].Value);
                ViewBag.RoleName = JwtProps.GetRole();
            }
            catch { }
            return View();
        }

        [HttpPost]
        [AuthorizeJwt]
        public string TerminalConfirmCode(string phone, string code)
        {
            //string connectionstring = ConfigurationManager.ConnectionStrings["Partner"].ConnectionString;
            //HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri(connectionstring);
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            //var token = HttpContext.Request.Cookies["lcmanageruserdata"].Value;
            //client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            phone = phone.Substring(4).Replace(")", "").Replace("-", "").Replace(" ", "");
            GetConfirmCodeRequest confirmRequest = new GetConfirmCodeRequest
            {
                Code = code,
                Phone = Convert.ToInt64(phone)
            };
            HttpResponseMessage confirmResponse = HttpClientService.PostAsync("api/values/GetConfirmCode", confirmRequest).Result;
            if (confirmResponse.IsSuccessStatusCode)
            {
                GetConfirmCodeResponse confirmCodeResponse = confirmResponse.Content.ReadAsAsync<GetConfirmCodeResponse>().Result;
                return JsonConvert.SerializeObject(confirmCodeResponse);
            }
            return "";
        }

        [HttpPost]
        [AuthorizeJwt]
        public string Terminal(RegisterModel model)
        {
            //string connectionstring = ConfigurationManager.ConnectionStrings["Partner"].ConnectionString;
            //HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri(connectionstring);
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            //var token = HttpContext.Request.Cookies["lcmanageruserdata"].Value;
            //client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            var phone = model.Phone.Substring(4).Replace(")", "").Replace("-", "").Replace(" ", "");

            //var client = new LoyconClient.ServiceClientSoapClient();
            //var phone = model.Phone.Substring(4).Replace(")", "").Replace("-", "").Replace(" ", "");
            GetConfirmCodeRequest confirmRequest = new GetConfirmCodeRequest
            {
                Code = model.Code,
                Phone = Convert.ToInt64(phone)
            };
            //var confirmResponse = client.GetConfirmCode(confirmRequest);
            HttpResponseMessage confirmResponse = HttpClientService.PostAsync("api/values/GetConfirmCode", confirmRequest).Result;
            if (confirmResponse.IsSuccessStatusCode)
            {
                GetConfirmCodeResponse confirmCodeResponse = confirmResponse.Content.ReadAsAsync<GetConfirmCodeResponse>().Result;
                if (confirmCodeResponse.ErrorCode == 0)
                {
                    GetRegistrationUserRequest registrationRequest = new GetRegistrationUserRequest
                    {
                        AgreePersonalData = true,
                        Phone = Convert.ToInt64(phone),
                        ClientSetPassword = true
                    };
                    try
                    {
                        registrationRequest.Operator = Convert.ToInt16(JwtProps.GetOperator());
                    }
                    catch { }
                    try
                    {
                        registrationRequest.PartnerID = JwtProps.GetPartner();
                        if (registrationRequest.PartnerID == 0)
                        {
                            registrationRequest.PartnerID = JwtProps.GetDefaultPartner();
                        }
                    }
                    catch { }
                    try
                    {
                        registrationRequest.PosCode = JwtProps.GetPosCode();
                        if (string.IsNullOrEmpty(registrationRequest.PosCode))
                        {
                            registrationRequest.PosCode = JwtProps.GetDefaultPosCode();
                        }
                    }
                    catch { }
                    if (!string.IsNullOrEmpty(model.FriendPhone))
                    {
                        string friendPhone = model.FriendPhone.Substring(4).Replace(")", "").Replace("-", "").Replace(" ", "");
                        registrationRequest.FriendPhone = Convert.ToInt64(friendPhone);
                    }
                    //var registrationResponse = client.GetRegistrationUser(registrationRequest);
                    HttpResponseMessage registrationResponse = HttpClientService.PostAsync("api/values/GetRegistrationUser", registrationRequest).Result;
                    if (registrationResponse.IsSuccessStatusCode)
                    {
                        GetRegistrationUserResponse registrationUserResponse = registrationResponse.Content.ReadAsAsync<GetRegistrationUserResponse>().Result;
                        if (registrationUserResponse.ErrorCode == 0)
                        {
                            int gender = 0;
                            if (model.Gender.Equals("1"))
                            {
                                gender = 1;
                            }
                            else if (model.Gender.Equals("0"))
                            {
                                gender = -1;
                            }
                            Client clientData = new Client
                            {
                                allowemail = true,
                                allowpush = true,
                                allowsms = true,
                                email = model.Email,
                                firstname = model.Name,
                                lastname = model.Surname,
                                middlename = model.Patronymic,
                                id = registrationUserResponse.Client,
                                phone = Convert.ToInt64(phone),
                                gender = gender
                            };
                            if (!string.IsNullOrEmpty(model.BirthDate))
                            {
                                clientData.birthdate = Convert.ToDateTime(model.BirthDate);
                            }
                            ChangeClientRequest clientRequest = new ChangeClientRequest
                            {
                                ClientData = clientData,
                                Operator = JwtProps.GetOperator()
                            };

                            HttpResponseMessage changeResponse = HttpClientService.PostAsync("api/values/ChangeClient", clientRequest).Result;
                            if (changeResponse.IsSuccessStatusCode)
                            {
                                ChangeClientResponse changeClientResponse = changeResponse.Content.ReadAsAsync<ChangeClientResponse>().Result;
                                return JsonConvert.SerializeObject(changeClientResponse);
                            }
                        }
                        else
                        {
                            return JsonConvert.SerializeObject(registrationUserResponse);
                            //return View(registrationUserResponse);
                        }
                    }
                }
                else
                {
                    return JsonConvert.SerializeObject(confirmCodeResponse);
                }
            }
            else
            {
                return "";
                //return View(confirmResponse);
            }
            return "";
            //return View();
        }

        [HttpPost]
        [AuthorizeJwt]
        public ActionResult AjaxRegister(RegisterModel data)
        {
            //string connectionstring = ConfigurationManager.ConnectionStrings["Partner"].ConnectionString;
            //HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri(connectionstring);
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            //var token = HttpContext.Request.Cookies["lcmanageruserdata"].Value;
            //client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            var phone = data.Phone;
            GetSendVerificationCodeRequest request = new GetSendVerificationCodeRequest
            {
                Phone = Convert.ToInt64(phone),
                Operator = Convert.ToInt16(JwtProps.GetOperator())
            };

            HttpResponseMessage response = HttpClientService.PostAsync("api/values/GetSendVerificationCode", request).Result;
            if (response.IsSuccessStatusCode)
            {
                return Json("Ok");
            }
            else
            {
                return Json("Error");
            }
        }

        [HttpPost]
        [AuthorizeJwt]
        public string SearchClient(string searchClient)
        {
            GetClientInfoRequest clientInfoRequest = new GetClientInfoRequest();
            if (searchClient.Length == 4 || searchClient.Length == 6 || searchClient.Length == 10)
            {
                try
                {
                    clientInfoRequest.Phone = Convert.ToInt64(searchClient);
                }
                catch { }
            }
            else if (searchClient.Length == 8 || searchClient.Length == 13)
            {
                try
                {
                    clientInfoRequest.Card = Convert.ToInt64(searchClient);
                }
                catch { }
            }
            else
            {
                return JsonConvert.SerializeObject(new GetClientInfoResponse { ErrorCode = 10, Message = "Неверное количество символов" });
            }

            //string connectionstring = ConfigurationManager.ConnectionStrings["Partner"].ConnectionString;
            //HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri(connectionstring);
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            //var token = HttpContext.Request.Cookies["lcmanageruserdata"].Value;
            //client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            clientInfoRequest.Operator = Convert.ToInt16(JwtProps.GetOperator());
            HttpResponseMessage response = HttpClientService.PostAsync("api/values/ClientInfo", clientInfoRequest).Result;
            if (response.IsSuccessStatusCode)
            {
                GetClientInfoResponse clientInfoResponse = response.Content.ReadAsAsync<GetClientInfoResponse>().Result;
                //if(clientInfoResponse.ErrorCode == 0)
                //{
                return JsonConvert.SerializeObject(clientInfoResponse);
                //return Json(clientInfoResponse);
                //}
                //return JsonConvert.SerializeObject("Ok");
            }
            else
            {
                return JsonConvert.SerializeObject("Error");
            }
        }

        [HttpPost]
        [AuthorizeJwt]
        public string TerminalMaxSumRedeem(TerminalRedeemSumModel model)
        {
            ChequeMaxSumRedeemRequest chequeMaxSumRedeemRequest = new ChequeMaxSumRedeemRequest();
            chequeMaxSumRedeemRequest.Partner = JwtProps.GetPartner();
            chequeMaxSumRedeemRequest.Card = Convert.ToInt64(model.Card);
            model.Sum = model.Sum.Replace('.', ',');
            chequeMaxSumRedeemRequest.ChequeSum = Convert.ToDecimal(model.Sum, new System.Globalization.CultureInfo("de-de"));
            if (chequeMaxSumRedeemRequest.Partner == 0)
            {
                try
                {
                    chequeMaxSumRedeemRequest.Partner = JwtProps.GetDefaultPartner();
                }
                catch { }
            }

            //string connectionstring = ConfigurationManager.ConnectionStrings["Partner"].ConnectionString;
            //HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri(connectionstring);
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            //var token = HttpContext.Request.Cookies["lcmanageruserdata"].Value;
            //client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            HttpResponseMessage response = HttpClientService.PostAsync("api/values/ChequeMaxSumRedeem", chequeMaxSumRedeemRequest).Result;
            if (response.IsSuccessStatusCode)
            {
                ChequeMaxSumRedeemResponse chequeMaxSumRedeemResponse = response.Content.ReadAsAsync<ChequeMaxSumRedeemResponse>().Result;
                if (chequeMaxSumRedeemResponse.ErrorCode == 0)
                {
                    return JsonConvert.SerializeObject(chequeMaxSumRedeemResponse);
                    //return Json(clientInfoResponse);
                }
                return JsonConvert.SerializeObject("Ok");
            }
            else
            {
                return JsonConvert.SerializeObject("Error");
            }
        }

        [HttpPost]
        [AuthorizeJwt]
        public string TerminalChequeAdd(TerminalChequeModel chequeModel)
        {
            decimal paidByBonus = 0;
            decimal maxRedeem = 0;
            decimal amount = 0;
            //if (chequeModel.PaidByBonus > chequeModel.MaxRedeem)
            //{
            //    chequeModel.PaidByBonus = chequeModel.MaxRedeem;
            //}
            System.Globalization.CultureInfo germanCulture = new System.Globalization.CultureInfo("de-de");
            if (!string.IsNullOrEmpty(chequeModel.Amount))
            {
                chequeModel.Amount = chequeModel.Amount.Replace('.', ',');
                amount = Convert.ToDecimal(chequeModel.Amount, germanCulture);
            }
            if (!string.IsNullOrEmpty(chequeModel.PaidByBonus))
            {
                chequeModel.PaidByBonus = chequeModel.PaidByBonus.Replace('.', ',');
                paidByBonus = Convert.ToDecimal(chequeModel.PaidByBonus, germanCulture);
            }

            if (!string.IsNullOrEmpty(chequeModel.MaxRedeem))
            {
                chequeModel.MaxRedeem = chequeModel.MaxRedeem.Replace('.', ',');
                maxRedeem = Convert.ToDecimal(chequeModel.MaxRedeem, germanCulture);
            }

            if (paidByBonus > maxRedeem)
            {
                paidByBonus = maxRedeem;
            }

            //string lcclient = ConfigurationManager.ConnectionStrings["Partner"].ConnectionString;
            //HttpClient httpClient = new HttpClient();
            //httpClient.BaseAddress = new Uri(lcclient);
            //httpClient.DefaultRequestHeaders.Accept.Clear();
            //httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            //var token = HttpContext.Request.Cookies["lcmanageruserdata"].Value;
            //httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            ChequeAddRequest chequeAdd = new ChequeAddRequest();
            /*номер карты, сумма покупки, сумма списаных бонусов, номер, время, код ТТ*/
            chequeAdd.Amount = amount;
            chequeAdd.Card = chequeModel.Card;
            chequeAdd.ChequeTime = DateTime.Now;
            chequeAdd.PaidByBonus = paidByBonus;
            chequeAdd.Number = DateTime.Now.ToString("HHmmss");
            chequeAdd.Redeemed = paidByBonus;
            chequeAdd.Partner = JwtProps.GetPartner();
            if (chequeAdd.Partner == 0)
            {
                try
                {
                    chequeAdd.Partner = JwtProps.GetDefaultPartner();
                }
                catch { }
            }
            chequeAdd.POS = JwtProps.GetPosCode();
            if (string.IsNullOrEmpty(chequeAdd.POS))
            {
                try
                {
                    chequeAdd.POS = JwtProps.GetDefaultPosCode();
                }
                catch { }
            }
            chequeAdd.NoAdd = false;
            chequeAdd.NoRedeem = false;
            HttpResponseMessage responseCheque = HttpClientService.PostAsync("api/values/ChequeAdd", chequeAdd).Result;
            if (responseCheque.IsSuccessStatusCode)
            {
                ChequeAddResponse chequeAddResponse = responseCheque.Content.ReadAsAsync<ChequeAddResponse>().Result;
                TerminalChequeAddResult result = new TerminalChequeAddResult
                {
                    Amount = chequeAdd.Amount.ToString(),
                    BonusAdd = chequeAddResponse.Bonus.ToString(),
                    BonusRedeem = ((-1) * chequeAddResponse.Redeemed).ToString(),
                    Cash = (chequeAdd.Amount - chequeAdd.PaidByBonus).ToString(),
                    ErrorCode = chequeAddResponse.ErrorCode,
                    Message = chequeAddResponse.Message
                };
                return JsonConvert.SerializeObject(result);
            }
            return "";
        }

        [HttpPost]
        [AuthorizeJwt]
        public string TerminalGetCheques(long Card)
        {
            //string lcpartner = ConfigurationManager.ConnectionStrings["Partner"].ConnectionString;
            //HttpClient httpClient = new HttpClient();
            //httpClient.BaseAddress = new Uri(lcpartner);
            //httpClient.DefaultRequestHeaders.Accept.Clear();
            //httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            //var token = HttpContext.Request.Cookies["lcmanageruserdata"].Value;
            //httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            GetChequesByCardRequest chequesByCardRequest = new GetChequesByCardRequest
            {
                CardNumber = Card
            };
            HttpResponseMessage responseCheques = HttpClientService.PostAsync("api/values/GetChequesByCard", chequesByCardRequest).Result;
            if (responseCheques.IsSuccessStatusCode)
            {
                GetChequesByCardResponse chequesByCardResponse = responseCheques.Content.ReadAsAsync<GetChequesByCardResponse>().Result;
                if (chequesByCardResponse.ErrorCode == 0)
                {
                    List<ChequeForSalePage> cheques = new List<ChequeForSalePage>();
                    foreach (var c in chequesByCardResponse.ChequeData)
                    {
                        ChequeForSalePage cheque = new ChequeForSalePage
                        {
                            added = c.Bonus.ToString(),
                            id = c.Id,
                            operation = c.OperationType,
                            number = c.Number,
                            redeemed = c.PaidByBonus.ToString(),
                            pos = c.Shop,
                            summ = c.Summ.ToString(),
                            date = "<p>" + c.Date.ToString("dd.MM.yyyy") + "</p> <p>" + c.Date.ToString("HH:mm"),
                            phone = 80000000000 + c.Phone
                        };
                        if (c.Items.Count > 0)
                        {
                            foreach (var item in c.Items)
                            {
                                cheque.lorem += @"<tr>
                                                    <td></td>
                                                    <td></td>
                                                    <td colspan='2'>" + item.Code + @"</td>
                                                    <td></td>
                                                    <td>x" + item.Qty.ToString() + @"</td>
                                                    <td>" + item.Amount.ToString() + @"р.</td>
                                                    <td>" + item.AddedBonus.ToString() + @"</td>
                                                    <td>" + item.RedeemedBonus.ToString() + @"</td>
                                                </tr>";
                            }
                            cheque.lorem += @"<tr style='visibility: collapse;'>
                                                <td></td>
                                                <td></td>
                                                <td></td>
                                                <td></td>
                                                <td></td>
                                                <td></td>
                                                <td></td>
                                                <td></td>
                                                <td></td>
                                            </tr>";
                        }
                        else
                        {
                            cheque.lorem = "";
                        }
                        cheques.Add(cheque);
                    }

                    var data = JsonConvert.SerializeObject(cheques);
                    return data;
                }
            }
            return "";
        }

        [HttpPost]
        [AuthorizeJwt]
        public string TerminalRefund(TerminalRefundModel refundModel)
        {
            //string lcpartner = ConfigurationManager.ConnectionStrings["Partner"].ConnectionString;
            //HttpClient httpClient = new HttpClient();
            //httpClient.BaseAddress = new Uri(lcpartner);
            //httpClient.DefaultRequestHeaders.Accept.Clear();
            //httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            //var token = HttpContext.Request.Cookies["lcmanageruserdata"].Value;
            //httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            decimal amount = 0;
            if (!string.IsNullOrEmpty(refundModel.ChequeSum))
            {
                refundModel.ChequeSum = refundModel.ChequeSum.Replace('.', ',');
                amount = Convert.ToDecimal(refundModel.ChequeSum, new System.Globalization.CultureInfo("de-de"));
            }

            DateTime chequeDate = DateTime.ParseExact(refundModel.ChequeDate, "dd.MM.yyyy", null);

            RefundRequest refundRequest = new RefundRequest
            {
                Amount = amount,
                Card = refundModel.Card,
                ChequeTime = DateTime.Now,
                PurchaseNumber = refundModel.ChequeNum,
                PurchaseDate = chequeDate,
                Partner = JwtProps.GetPartner(),
                Number = DateTime.Now.ToString("HHmmss")
            };
            if (refundRequest.Partner == 0)
            {
                try
                {
                    refundRequest.Partner = JwtProps.GetDefaultPartner();
                }
                catch { }
            }
            try
            {
                refundRequest.Pos = JwtProps.GetPosCode();
                if (string.IsNullOrEmpty(refundRequest.Pos))
                {
                    refundRequest.Pos = JwtProps.GetDefaultPosCode();
                }
            }
            catch { }
            HttpResponseMessage responseRefund = HttpClientService.PostAsync("api/values/Refund", refundRequest).Result;
            if (responseRefund.IsSuccessStatusCode)
            {
                RefundResponse refundResponse = responseRefund.Content.ReadAsAsync<RefundResponse>().Result;
                return JsonConvert.SerializeObject(refundResponse);
            }
            return "";
        }

        [HttpPost]
        [AuthorizeJwt]
        public string HandBonusAdd(long Card, decimal Bonus)
        {
            //string lcpartner = ConfigurationManager.ConnectionStrings["Partner"].ConnectionString;
            //HttpClient httpClient = new HttpClient
            //{
            //    BaseAddress = new Uri(lcpartner)
            //};
            //httpClient.DefaultRequestHeaders.Accept.Clear();
            //httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            //var token = HttpContext.Request.Cookies["lcmanageruserdata"].Value;
            //httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            BonusAddRequest bonusAddRequest = new BonusAddRequest
            {
                Bonus = Bonus,
                Card = Card,
                Operator = Convert.ToInt16(JwtProps.GetOperator())
            };
            HttpResponseMessage responseMessage = HttpClientService.PostAsync("api/values/BonusAdd", bonusAddRequest).Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                BonusAddResponse bonusAddResponse = responseMessage.Content.ReadAsAsync<BonusAddResponse>().Result;
                return JsonConvert.SerializeObject(bonusAddResponse);
            }

            return "";
        }

        [HttpPost]
        [AuthorizeJwt]
        public string HandBonusRedeem(long Card, decimal Bonus)
        {
            //string lcpartner = ConfigurationManager.ConnectionStrings["Partner"].ConnectionString;
            //HttpClient httpClient = new HttpClient
            //{
            //    BaseAddress = new Uri(lcpartner)
            //};
            //httpClient.DefaultRequestHeaders.Accept.Clear();
            //httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            //var token = HttpContext.Request.Cookies["lcmanageruserdata"].Value;
            //httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            BonusAddRequest bonusAddRequest = new BonusAddRequest
            {
                Bonus = (-1) * Bonus,
                Card = Card,
                Operator = Convert.ToInt16(JwtProps.GetOperator())
            };
            HttpResponseMessage responseMessage = HttpClientService.PostAsync("api/values/BonusAdd", bonusAddRequest).Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                BonusAddResponse bonusAddResponse = responseMessage.Content.ReadAsAsync<BonusAddResponse>().Result;
                return JsonConvert.SerializeObject(bonusAddResponse);
            }

            return "";
        }

        [AuthorizeJwt]
        public string SalesGetCheques(JQueryDataTableParamModel param)
        {
            GetChequesRequest chequesRequest = new GetChequesRequest
            {
                Operator = Convert.ToInt16(JwtProps.GetOperator()),
                DateBuy = Request["columns[1][search][value]"],
                PosStr = Request["columns[2][search][value]"],
                Phone = Request["columns[3][search][value]"],
                Operation = Request["columns[4][search][value]"],
                Number = Request["columns[5][search][value]"],
                Sum = Request["columns[6][search][value]"],
                Added = Request["columns[7][search][value]"],
                Redeemed = Request["columns[8][search][value]"],
                DateStart = Request["date_from"],
                DateEnd = Request["date_to"],
                Page = Convert.ToInt64(param.start),
                PageSize = Convert.ToInt64(param.length)
            };
            try
            {
                chequesRequest.Page++;
                chequesRequest.PartnerId = JwtProps.GetPartner();
            }
            catch { }
            try
            {
                chequesRequest.Pos = JwtProps.GetPos();
            }
            catch { }
            HttpResponseMessage responseMessage = HttpClientService.PostAsync("api/values/GetCheques", chequesRequest).Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                GetChequesResponse chequesResponse = responseMessage.Content.ReadAsAsync<GetChequesResponse>().Result;
                if (chequesResponse.ErrorCode == 0)
                {
                    Cheques cheques = new Cheques();
                    foreach (var c in chequesResponse.ChequeData)
                    {
                        ChequeForSalePage cheque = new ChequeForSalePage
                        {
                            added = c.Bonus.ToString(),
                            id = c.Id,
                            operation = c.OperationType,
                            number = c.Number,
                            redeemed = c.PaidByBonus.ToString(),
                            pos = c.PosName,
                            summ = c.Summ.ToString(),
                            date = "<p>" + c.Date.ToString("dd.MM.yyyy") + "</p> <p>" + c.Date.ToString("HH:mm") +"</p>"
                        };
                        //if(c.Phone > 0)
                        //{
                        cheque.phone = c.Phone;
                        //}
                        if (c.Items.Count > 0)
                        {
                            foreach (var item in c.Items)
                            {
                                cheque.lorem += @"<tr>
                                                    <td></td>
                                                    <td></td>
                                                    <td colspan='2'>" + item.Code + @"</td>
                                                    <td></td>
                                                    <td>x" + item.Qty.ToString() + @"</td>
                                                    <td>" + item.Amount.ToString() + @"р.</td>
                                                    <td>" + item.AddedBonus.ToString() + @"</td>
                                                    <td>" + item.RedeemedBonus.ToString() + @"</td>
                                                </tr>";
                            }
                            cheque.lorem += @"<tr style='visibility: collapse;'>
                                                <td></td>
                                                <td></td>
                                                <td></td>
                                                <td></td>
                                                <td></td>
                                                <td></td>
                                                <td></td>
                                                <td></td>
                                                <td></td>
                                            </tr>";
                            //cheque.lorem = @" < tr>
                            //                <td></td>
                            //                <td></td>
                            //                <td>1. Колбаса 'Деловая'</td>
                            //                <td></td>
                            //                <td></td>
                            //                <td>x1</td>
                            //                <td>350р.00коп.</td>
                            //                <td>13,0</td>
                            //                <td>35</td>
                            //            </tr>
                            //            <tr>
                            //                <td></td>
                            //                <td></td>
                            //                <td>2. Сыр 'Пророссийский'</td>
                            //                <td></td>
                            //                <td></td>
                            //                <td>x2</td>
                            //                <td>200р.00коп.</td>
                            //                <td>13,0</td>
                            //                <td>35</td>
                            //            </tr>
                            //            <tr>
                            //                <td></td>
                            //                <td></td>
                            //                <td>3. Пельмени 'Неваляшки' 1кг.</td>
                            //                <td></td>
                            //                <td></td>
                            //                <td>x1</td>
                            //                <td>450р.00коп.</td>
                            //                <td>13,0</td>
                            //                <td>35</td>
                            //            </tr>
                            //            <tr>
                            //                <td></td>
                            //                <td></td>
                            //                <td>4. Сок 'Свежевыжатый' 5л.</td>
                            //                <td></td>
                            //                <td></td>
                            //                <td>x3</td>
                            //                <td>156р.00коп.</td>
                            //                <td>13,0</td>
                            //                <td>35</td>
                            //            </tr>
                            //            <tr>
                            //                <td></td>
                            //                <td></td>
                            //                <td>5. Кофе 'Обама' 300г.</td>
                            //                <td></td>
                            //                <td></td>
                            //                <td>x2</td>
                            //                <td>120р.00коп.</td>
                            //                <td>13,0</td>
                            //                <td>35</td>
                            //            </tr>
                            //            <tr>
                            //                <td></td>
                            //                <td></td>
                            //                <td>6. Сливки 'Армейские' 100г.</td>
                            //                <td></td>
                            //                <td></td>
                            //                <td>x1</td>
                            //                <td>60р.00коп.</td>
                            //                <td>13,0</td>
                            //                <td>35</td>
                            //            </tr>";
                        }
                        else
                        {
                            cheque.lorem = "";
                        }
                        cheques.data.Add(cheque);
                    }
                    cheques.draw = param.draw;
                    cheques.recordsTotal = chequesResponse.RecordTotal;
                    cheques.recordsFiltered = chequesResponse.RecordFilterd;

                    var data = JsonConvert.SerializeObject(cheques);
                    return data;
                }
            }
            return "";
        }

        [AuthorizeJwt]
        [HttpPost]
        public string ClientData(JQueryDataTableParamModel param)
        {
            int year = DateTime.Now.Year;
            OperatorClientsManagerRequest managerRequest = new OperatorClientsManagerRequest
            {
                Operator = Convert.ToInt16(JwtProps.GetOperator()),
                From = new DateTime(year, 1, 1),
                To = new DateTime(year + 1, 1, 1),
                Name = Request["columns[1][search][value]"],
                Phone = Request["columns[2][search][value]"],
                Email = Request["columns[3][search][value]"],
                Birthdate = Request["columns[4][search][value]"],
                Sex = Request["columns[5][search][value]"],
                ClientType = Request["columns[6][search][value]"],
                Number = Request["columns[7][search][value]"],
                Level = Request["columns[8][search][value]"],
                Balance = Request["columns[9][search][value]"],
                Page = Convert.ToInt64(param.start),
                PageSize = Convert.ToInt64(param.length)
            };
            try
            {
                managerRequest.Page++;
                managerRequest.Partner = JwtProps.GetPartner();
            }
            catch { }
            try
            {
                managerRequest.Pos = JwtProps.GetPosCode();
            }
            catch { }
            HttpResponseMessage responseMessage = HttpClientService.PostAsync("api/values/OperatorClientsManager", managerRequest).Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                OperatorClientsManagerResponse clientsManagerResponse = responseMessage.Content.ReadAsAsync<OperatorClientsManagerResponse>().Result;
                if (clientsManagerResponse.ErrorCode == 0)
                {
                    clientdata clientdatavalue = new clientdata();
                    foreach (var c in clientsManagerResponse.OperatorClients)
                    {
                        var client = new ClientsForClientsPage
                        {
                            balance = c.Balance,
                            birthdayBonusAmount = c.BirthdayBonus,
                            buyAmount = c.BuySum,
                            buyCount = c.BuyQty,
                            buyLastAmount = c.LastBuyAmount,
                            card = c.Card,
                            client_type = c.ClientType,
                            email = c.Email,
                            friendBonusAmount = c.FriendBonus,
                            gender = c.Gender,
                            id = c.Id,
                            level = c.Level,
                            name = c.Name,
                            operatorBonusAmount = c.OperatorBonus,
                            phone = c.Phone,
                            promoBonusAmount = c.PromoBonus,
                            welcomeBonusAmount = c.WelcomeBonus,
                            writeOffAmount = c.BonusRedeemSum,
                            writeOffCount = c.BonusRedeemQty,
                            refund = c.Refund,
                            refundQty = c.RefundQty,
                            posRegister = c.PosRegister
                        };
                        if (c.BirthDate > new DateTime(1900, 01, 01))
                        {
                            client.birthdate = c.BirthDate.ToString("dd.MM.yyyy");
                        }
                        else
                        {
                            client.birthdate = "";
                        }
                        if (c.BirthdayBonusDate > new DateTime(1900, 01, 01))
                        {
                            client.birthdayBonusDate = c.BirthdayBonusDate.ToString("dd.MM.yyyy");
                        }
                        else
                        {
                            client.birthdayBonusDate = "";
                        }
                        if (c.LastBuyDate > new DateTime(1900, 01, 01))
                        {
                            client.buyLastDate = c.LastBuyDate.ToString("dd.MM.yyyy");
                        }
                        else
                        {
                            client.buyLastDate = "";
                        }
                        if (c.FriendBonusDate > new DateTime(1900, 01, 01))
                        {
                            client.friendBonusDate = c.FriendBonusDate.ToString("dd.MM.yyyy");
                        }
                        else
                        {
                            client.friendBonusDate = "";
                        }
                        if (c.OperatorBonusDate > new DateTime(1900, 01, 01))
                        {
                            client.opperatorBonusDate = c.OperatorBonusDate.ToString("dd.MM.yyyy");
                        }
                        else
                        {
                            client.opperatorBonusDate = "";
                        }
                        if (c.PromoBonusDate > new DateTime(1900, 01, 01))
                        {
                            client.promoBonusDate = c.PromoBonusDate.ToString("dd.MM.yyyy");
                        }
                        else
                        {
                            client.promoBonusDate = "";
                        }
                        if (c.WelcomeBonusDate > new DateTime(1900, 01, 01))
                        {
                            client.welcomeBonusDate = c.WelcomeBonusDate.ToString("dd.MM.yyyy");
                        }
                        else
                        {
                            client.welcomeBonusDate = "";
                        }
                        if (c.DateRegister > new DateTime(1900, 01, 01))
                        {
                            client.dateRegister = c.DateRegister.ToString("dd.MM.yyyy");
                        }
                        else
                        {
                            client.dateRegister = "";
                        }

                        c.CardBuys = c.CardBuys.OrderBy(x => x.MonthNum).ToList();
                        string buys = "";
                        string avgcheque = "";
                        string added = "";
                        string redeemed = "";
                        for (int i = 1; i <= 12; i++)
                        {
                            buys += Convert.ToInt32(Math.Round(c.CardBuys.Where(x => x.MonthNum == i).Select(x => x.ChequeSum).FirstOrDefault())) + ",";
                            avgcheque += Convert.ToInt32(Math.Round(c.CardBuys.Where(x => x.MonthNum == i).Select(x => x.AvgCheque).FirstOrDefault())) + ",";
                            added += Convert.ToInt32(Math.Round(c.CardBuys.Where(x => x.MonthNum == i).Select(x => x.BonusAdded).FirstOrDefault())) + ",";
                            redeemed += Convert.ToInt32(Math.Round(c.CardBuys.Where(x => x.MonthNum == i).Select(x => x.BonusRedeemed).FirstOrDefault())) + ",";
                        }
                        buys = buys.Substring(0, buys.Length - 1);
                        avgcheque = avgcheque.Substring(0, avgcheque.Length - 1);
                        added = added.Substring(0, added.Length - 1);
                        redeemed = redeemed.Substring(0, redeemed.Length - 1);
                        List<ChequeForSalePage> cheques = new List<ChequeForSalePage>();
                        foreach (var d in c.ChequeData)
                        {
                            ChequeForSalePage cheque = new ChequeForSalePage
                            {
                                added = d.Bonus.ToString(),
                                id = d.Id,
                                operation = d.OperationType,
                                number = d.Number,
                                redeemed = d.PaidByBonus.ToString(),
                                pos = d.PosName,
                                summ = d.Summ.ToString(),
                                date = "<p>" + d.Date.ToString("dd.MM.yyyy") + "</p> <p>" + d.Date.ToString("HH:mm"),
                                phone = 80000000000 + d.Phone
                            };
                            if (d.Items.Count > 0)
                            {
                                foreach (var item in d.Items)
                                {
                                    cheque.lorem += @"<tr>
                                                    <td></td>
                                                    <td></td>
                                                    <td colspan='2'>" + item.Code + @"</td>
                                                    <td></td>
                                                    <td>x" + item.Qty.ToString() + @"</td>
                                                    <td>" + item.Amount.ToString() + @"р.</td>
                                                    <td>" + item.AddedBonus.ToString() + @"</td>
                                                    <td>" + item.RedeemedBonus.ToString() + @"</td>
                                                </tr>";
                                }
                                cheque.lorem += @"<tr style='visibility: collapse;'>
                                                <td></td>
                                                <td></td>
                                                <td></td>
                                                <td></td>
                                                <td></td>
                                                <td></td>
                                                <td></td>
                                                <td></td>
                                                <td></td>
                                            </tr>";
                            }
                            else
                            {
                                cheque.lorem = "";
                            }
                            cheques.Add(cheque);
                        }
                        var clientBuys = JsonConvert.SerializeObject(cheques);
                        //string clientBonuses = "[{ \"date\":\"<p>12.02.2018</p> <p>00:34</p>\",\"bonustype\":\"Возврат\",\"added\":\"0\",\"redeemed\":\"20\",\"burn\":\"50\"}]";
                        //string clientBonuses2 = JsonConvert.SerializeObject(c.Bonuses);
                        string clientBonuses = "";
                        if (c.Bonuses.Count > 0)
                        {
                            clientBonuses = "[";
                            for (int i = 0, n = c.Bonuses.Count; i < n; i++)
                            {
                                string bonus = "{ \"BonusDate\":\"<p>" +
                                               c.Bonuses[i].BonusDate.Value.ToString("dd.MM.yyyy") + "</p> <p>" +
                                               c.Bonuses[i].BonusDate.Value.ToString("HH:mm") +
                                               "</p>\",\"BonusSource\":\"" + c.Bonuses[i].BonusSource +
                                               "\",\"BonusAdded\":\"" + c.Bonuses[i].BonusAdded.ToString() +
                                               "\",\"BonusRedeemed\":\"" + c.Bonuses[i].BonusRedeemed.ToString() +
                                               "\",\"BonusBurn\":\"" + c.Bonuses[i].BonusBurn.ToString() + "\"}";
                                clientBonuses += bonus;
                                if (i < n - 1)
                                {
                                    clientBonuses += ",";
                                }
                            }
                            clientBonuses += "]";
                        }
                        else
                        {
                            clientBonuses = "[]";
                        }
                        client.diagram = @"<div class='line-chart-bl'>
                                            <div class='line-chart__head'>
                                                <div id='line-chart-leg' class='line-chart-leg'>
                                                </div>
                                            </div>
                                            <div class='line-chart__bottom' >
                                                <canvas id='canvas' class='line-chart' ></canvas>
                                                <div id='chartjs-tooltip-1' class='linejs-tooltip'></div>
                                            </div>
                                            
                                        </div>
                                        <script>
                                            lineChartData={labels:['Январь','Февраль','Март','Апрель','Май','Июнь','Июль','Август','Сентябрь','Октябрь','Ноябрь','Декабрь'],
                                                                datasets:[{label:'Выручка', borderColor:'#58AEDC',pointBackgroundColor:'#58AEDC',pointRadius:2,backgroundColor:'#58AEDC',
                                                                            data:[" + buys + @"],fill:!1,borderWidth:2,},
                                                                            {label:'Средний чек',borderColor:'#11B9A3',pointBackgroundColor:'#11B9A3',pointRadius:2,backgroundColor:'#11B9A3',
                                                                            data:[" + avgcheque + @"],fill:!1,borderWidth:2,},
                                                                            {label:'Начислено',borderColor:'#E5C861',pointBackgroundColor:'#E5C861',pointRadius:2,backgroundColor:'#E5C861',
                                                                            data:[" + added + @"],fill:!1,borderWidth:2,},
                                                                            {label:'Списано',borderColor:'#567BA5',pointBackgroundColor:'#567BA5',pointRadius:2,backgroundColor:'#567BA5',
                                                                            data:[" + redeemed + @"],fill:!1,borderWidth:2,}]};typeDiagram='line';
                                            var tableClientCheques = $('table#ClientCheques').DataTable();
                                            tableClientCheques.clear().draw();
                                            tableClientCheques.rows.add(" + clientBuys + @").draw();

                                            var tableClientBonuses = $('table#ClientBonuses').DataTable();
                                            tableClientBonuses.clear().draw();
                                            tableClientBonuses.rows.add(" + clientBonuses + @").draw();
                                        </script>";

                        clientdatavalue.data.Add(client);
                    }
                    clientdatavalue.draw = param.draw;
                    clientdatavalue.recordsFiltered = clientsManagerResponse.RecordFilterd;
                    clientdatavalue.recordsTotal = clientsManagerResponse.RecordTotal;
                    var data = JsonConvert.SerializeObject(clientdatavalue);
                    return data;
                }
            }
            return "";
        }

        [HttpPost]
        //[AuthorizeJwt(Roles = "Analytics")]
        public async Task<string> AnalyticClientGetAge(string period)
        {
            SegmentationAgeRequest ageRequest = new SegmentationAgeRequest
            {
                Operator = Convert.ToInt16(JwtProps.GetOperator())
            };
            try
            {
                ageRequest.Partner = JwtProps.GetPartner();
            }
            catch { }
            try
            {
                ageRequest.Pos = JwtProps.GetPos();
            }
            catch { }

            switch (period)
            {
                case "day":
                    ageRequest.BeginDate = DateTime.Now;
                    ageRequest.EndDate = DateTime.Now.AddDays(1);
                    break;
                case "week":
                    ageRequest.BeginDate = DateTime.Now.AddDays(-7);
                    ageRequest.EndDate = DateTime.Now.AddDays(1);
                    break;
                case "month":
                    ageRequest.BeginDate = DateTime.Now.AddMonths(-1);
                    ageRequest.EndDate = DateTime.Now.AddDays(1);
                    break;
                case "quarter":
                    var now = DateTime.Now;
                    var quarter = (now.Month + 2) / 3;
                    switch (quarter)
                    {
                        case 1:
                            ageRequest.BeginDate = new DateTime(now.Year, 1, 1);
                            ageRequest.EndDate = new DateTime(now.Year, 4, 1);
                            break;
                        case 2:
                            ageRequest.BeginDate = new DateTime(now.Year, 4, 1);
                            ageRequest.EndDate = new DateTime(now.Year, 7, 1);
                            break;
                        case 3:
                            ageRequest.BeginDate = new DateTime(now.Year, 7, 1);
                            ageRequest.EndDate = new DateTime(now.Year, 10, 1);
                            break;
                        case 4:
                            ageRequest.BeginDate = new DateTime(now.Year, 10, 1);
                            ageRequest.EndDate = new DateTime(now.Year + 1, 1, 1);
                            break;
                    }
                    break;
                case "all":
                    ageRequest.BeginDate = new DateTime(1900, 1, 1);
                    ageRequest.EndDate = DateTime.Now.AddDays(1);
                    break;
                default:
                    ageRequest.BeginDate = DateTime.Now;
                    ageRequest.EndDate = DateTime.Now.AddDays(1);
                    break;
            }

            List<int> data = new List<int>();
            HttpResponseMessage responseMessage = await HttpClientService.PostAsync("api/values/SegmentationAge", ageRequest);
            if (responseMessage.IsSuccessStatusCode)
            {
                SegmentationAgeResponse ageResponse = await responseMessage.Content.ReadAsAsync<SegmentationAgeResponse>();
                if (ageResponse.ErrorCode == 0)
                {
                    data.Add(ageResponse.LessThen25);
                    data.Add(ageResponse.More25Less35);
                    data.Add(ageResponse.More35Less45);
                    data.Add(ageResponse.More45);
                    data.Add(ageResponse.Unknown);

                    return JsonConvert.SerializeObject(new
                    {
                        graph = data,
                        table = new
                        {
                            ClientCount = ageResponse.ClientQty,
                            WithBirthDate = ageResponse.WithBirthDate,
                            WithoutBirthDate = ageResponse.WithoutBirthDate
                        }
                    });
                }
            }

            //List<int> data = new List<int>();
            //data.Add(1); // меньше 25
            //data.Add(2); // 25-35
            //data.Add(3); // 35-45
            //data.Add(4); // больше 45
            //data.Add(5); //неизвестно           

            return JsonConvert.SerializeObject(new
            {
                graph = data,
                table = new
                {
                    ClientCount = 0,
                    WithBirthDate = 0,
                    WithoutBirthDate = 0
                }
            });
        }

        [HttpPost]
        //[AuthorizeJwt(Roles = "Analytics")]
        public async Task<string> AnalyticClientGetBuys(string period)
        {


            //string lcpartner = ConfigurationManager.ConnectionStrings["Partner"].ConnectionString;
            //HttpClient httpClient = new HttpClient
            //{
            //    BaseAddress = new Uri(lcpartner)
            //};
            //httpClient.DefaultRequestHeaders.Accept.Clear();
            //httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            //var token = HttpContext.Request.Cookies["lcmanageruserdata"].Value;
            //httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            ClientBaseStructureRequest structureRequest = new ClientBaseStructureRequest
            {
                Operator = Convert.ToInt16(JwtProps.GetOperator())
            };
            try
            {
                structureRequest.Partner = JwtProps.GetPartner();
            }
            catch { }
            try
            {
                structureRequest.Pos = JwtProps.GetPos();
            }
            catch { }

            switch (period)
            {
                case "day":
                    structureRequest.BeginDate = DateTime.Now;
                    structureRequest.EndDate = DateTime.Now.AddDays(1);
                    break;
                case "week":
                    structureRequest.BeginDate = DateTime.Now.AddDays(-7);
                    structureRequest.EndDate = DateTime.Now.AddDays(1);
                    break;
                case "month":
                    structureRequest.BeginDate = DateTime.Now.AddMonths(-1);
                    structureRequest.EndDate = DateTime.Now.AddDays(1);
                    break;
                case "quarter":
                    var now = DateTime.Now;
                    var quarter = (now.Month + 2) / 3;
                    switch (quarter)
                    {
                        case 1:
                            structureRequest.BeginDate = new DateTime(now.Year, 1, 1);
                            structureRequest.EndDate = new DateTime(now.Year, 4, 1);
                            break;
                        case 2:
                            structureRequest.BeginDate = new DateTime(now.Year, 4, 1);
                            structureRequest.EndDate = new DateTime(now.Year, 7, 1);
                            break;
                        case 3:
                            structureRequest.BeginDate = new DateTime(now.Year, 7, 1);
                            structureRequest.EndDate = new DateTime(now.Year, 10, 1);
                            break;
                        case 4:
                            structureRequest.BeginDate = new DateTime(now.Year, 10, 1);
                            structureRequest.EndDate = new DateTime(now.Year + 1, 1, 1);
                            break;
                    }
                    break;
                case "all":
                    structureRequest.BeginDate = new DateTime(1900, 1, 1);
                    structureRequest.EndDate = DateTime.Now.AddDays(1);
                    break;
                default:
                    structureRequest.BeginDate = DateTime.Now;
                    structureRequest.EndDate = DateTime.Now.AddDays(1);
                    break;
            }

            List<int> data = new List<int>();
            HttpResponseMessage responseMessage = await HttpClientService.PostAsync("api/values/ClientBaseStructure", structureRequest);
            if (responseMessage.IsSuccessStatusCode)
            {
                var json = new { success = true };

                ClientBaseStructureResponse structureResponse = await responseMessage.Content.ReadAsAsync<ClientBaseStructureResponse>();
                if (structureResponse.ErrorCode == 0)
                {
                    data.Add(structureResponse.MenQty);
                    data.Add(structureResponse.WomenQty);
                    data.Add(structureResponse.UnknownGender);
                    data.Add(structureResponse.ClientsWithBuys);
                    data.Add(structureResponse.ClientsWithoutBuys);

                    return JsonConvert.SerializeObject(new
                    {
                        graph = data,
                        table = new
                        {
                            ClientQty = structureResponse.MenQty + structureResponse.WomenQty + structureResponse.UnknownGender,
                            ClientsWithPhone = structureResponse.ClientsWithPhone,
                            ClientsWithEmail = structureResponse.ClientsWithEmail,
                            ClientsWithTenBuys = structureResponse.ClientsWithTenBuys,
                            ClientsWitnOneBuys = structureResponse.ClientsWitnOneBuys
                        }
                    });
                }
            }
            //data.Add(1000); //мужчины
            //data.Add(2000); //женщины
            //data.Add(3000); //пол не указан
            //data.Add(4000); //с покупками
            //data.Add(5000); //без покупок
            var returnData = JsonConvert.SerializeObject(new
            {
                graph = data,
                table = new
                {
                    ClientQty = 0,
                    ClientsWithPhone = 0,
                    ClientsWithEmail = 0,
                    ClientsWithTenBuys = 0,
                    ClientsWitnOneBuys = 0
                }
            });
            return returnData;
        }

        [HttpPost]
        //[AuthorizeJwt(Roles = "Analytics")]
        public async Task<string> AnalyticGetActive(string period)
        {
            ClientBaseActiveRequest baseActiveRequest = new ClientBaseActiveRequest
            {
                Operator = Convert.ToInt16(JwtProps.GetOperator())
            };
            try
            {
                baseActiveRequest.Partner = JwtProps.GetPartner();
            }
            catch { }
            try
            {
                baseActiveRequest.Pos = JwtProps.GetPos();
            }
            catch { }

            switch (period)
            {
                case "day":
                    baseActiveRequest.BeginDate = DateTime.Now.Date;
                    baseActiveRequest.EndDate = DateTime.Now.Date.AddDays(1);
                    break;
                case "week":
                    baseActiveRequest.BeginDate = DateTime.Now.Date.AddDays(-7);
                    baseActiveRequest.EndDate = DateTime.Now.Date.AddDays(1);
                    break;
                case "month":
                    baseActiveRequest.BeginDate = DateTime.Now.Date.AddMonths(-1);
                    baseActiveRequest.EndDate = DateTime.Now.Date.AddDays(1);
                    break;
                case "quarter":
                    var now = DateTime.Now.Date;
                    var quarter = (now.Month + 2) / 3;
                    switch (quarter)
                    {
                        case 1:
                            baseActiveRequest.BeginDate = new DateTime(now.Year, 1, 1);
                            baseActiveRequest.EndDate = new DateTime(now.Year, 4, 1);
                            break;
                        case 2:
                            baseActiveRequest.BeginDate = new DateTime(now.Year, 4, 1);
                            baseActiveRequest.EndDate = new DateTime(now.Year, 7, 1);
                            break;
                        case 3:
                            baseActiveRequest.BeginDate = new DateTime(now.Year, 7, 1);
                            baseActiveRequest.EndDate = new DateTime(now.Year, 10, 1);
                            break;
                        case 4:
                            baseActiveRequest.BeginDate = new DateTime(now.Year, 10, 1);
                            baseActiveRequest.EndDate = new DateTime(now.Year + 1, 1, 1);
                            break;
                    }
                    break;
                case "all":
                    baseActiveRequest.BeginDate = new DateTime(1900, 1, 1);
                    baseActiveRequest.EndDate = DateTime.Now.Date.AddDays(1);
                    break;
                default:
                    baseActiveRequest.BeginDate = DateTime.Now.Date;
                    baseActiveRequest.EndDate = DateTime.Now.Date.AddDays(1);
                    break;
            }
            List<decimal> data = new List<decimal>();
            HttpResponseMessage responseMessage = await HttpClientService.PostAsync("api/values/ClientBaseActive", baseActiveRequest);
            if (responseMessage.IsSuccessStatusCode)
            {
                ClientBaseActiveResponse baseActiveResponse = await responseMessage.Content.ReadAsAsync<ClientBaseActiveResponse>();
                if (baseActiveResponse.ErrorCode == 0)
                {
                    data.Add(baseActiveResponse.MenBuys);
                    data.Add(baseActiveResponse.WomenBuys);
                    data.Add(baseActiveResponse.UnknownGenderBuys);
                    data.Add(baseActiveResponse.RepeatedBuys);
                    data.Add(baseActiveResponse.BuysOnClient);
                    return JsonConvert.SerializeObject(new
                    {
                        graph = data,
                        table = new
                        {
                            ClientActiveQty = baseActiveResponse.ClientActiveQty,
                            Gain = baseActiveResponse.Gain,
                            AvgCheque = baseActiveResponse.AvgCheque,
                            BuysWeekdays = baseActiveResponse.BuysWeekdays,
                            BuysWeekOff = baseActiveResponse.BuysWeekOff
                        }
                    });
                }
            }
            //data.Add(1000); //мужчины
            //data.Add(3000); //женщины
            //data.Add(5000); //пол не указан
            //data.Add(7000); //повторные
            //data.Add(9000); //покупок на клиента
            var returnData = JsonConvert.SerializeObject(data);
            return returnData;
        }

        public string AnalitycClientGetActivityBuys()
        {

            return "";
        }

        [HttpPost]
        //[AuthorizeJwt(Roles = "Analytics")]
        public async Task<string> AnalyticClientGetBonus(string period)
        {
            BonusesRequest bonusRequest = new BonusesRequest
            {
                Operator = Convert.ToInt16(JwtProps.GetOperator())
            };
            try
            {
                bonusRequest.Partner = JwtProps.GetPartner();
            }
            catch { }
            try
            {
                bonusRequest.Pos = JwtProps.GetPos();
            }
            catch { }

            switch (period)
            {
                case "day":
                    bonusRequest.BeginDate = DateTime.Now;
                    bonusRequest.EndDate = DateTime.Now.AddDays(1);
                    break;
                case "week":
                    bonusRequest.BeginDate = DateTime.Now.AddDays(-7);
                    bonusRequest.EndDate = DateTime.Now.AddDays(1);
                    break;
                case "month":
                    bonusRequest.BeginDate = DateTime.Now.AddMonths(-1);
                    bonusRequest.EndDate = DateTime.Now.AddDays(1);
                    break;
                case "quarter":
                    var now = DateTime.Now;
                    var quarter = (now.Month + 2) / 3;
                    switch (quarter)
                    {
                        case 1:
                            bonusRequest.BeginDate = new DateTime(now.Year, 1, 1);
                            bonusRequest.EndDate = new DateTime(now.Year, 4, 1);
                            break;
                        case 2:
                            bonusRequest.BeginDate = new DateTime(now.Year, 4, 1);
                            bonusRequest.EndDate = new DateTime(now.Year, 7, 1);
                            break;
                        case 3:
                            bonusRequest.BeginDate = new DateTime(now.Year, 7, 1);
                            bonusRequest.EndDate = new DateTime(now.Year, 10, 1);
                            break;
                        case 4:
                            bonusRequest.BeginDate = new DateTime(now.Year, 10, 1);
                            bonusRequest.EndDate = new DateTime(now.Year + 1, 1, 1);
                            break;
                    }
                    break;
                case "all":
                    bonusRequest.BeginDate = new DateTime(1900, 1, 1);
                    bonusRequest.EndDate = DateTime.Now.AddDays(1);
                    break;
                default:
                    bonusRequest.BeginDate = DateTime.Now;
                    bonusRequest.EndDate = DateTime.Now.AddDays(1);
                    break;
            }

            HttpResponseMessage responseMessage = await HttpClientService.PostAsync("api/values/GetClientBonuses", bonusRequest);
            if (responseMessage.IsSuccessStatusCode)
            {
                BonusesResponse bonusResponse = await responseMessage.Content.ReadAsAsync<BonusesResponse>();
                if (bonusResponse.ErrorCode == 0)
                {
                    return JsonConvert.SerializeObject(new
                    {
                        table = new
                        {
                            AddedBonus = bonusResponse.AddedBonus.ToString("0.00"),
                            AvgCharge = bonusResponse.AvgCharge.ToString("0.00"),
                            RedeemedBonus = bonusResponse.RedeemedBonus.ToString("0.00"),
                            AvgRedeem = bonusResponse.AvgRedeem.ToString("0.00"),
                            AvgBalance = bonusResponse.AvgBalance.ToString("0.00"),
                            AvgDiscount = bonusResponse.AvgDiscount.ToString("0.00") + "%"
                        }
                    });
                }
            }
            return JsonConvert.SerializeObject(new
            {
                table = new
                {
                    AddedBonus = 0,
                    AvgCharge = 0,
                    RedeemedBonus = 0,
                    AvgRedeem = 0,
                    AvgBalance = 0,
                    AvgDiscount = 0
                }
            });
        }

        [HttpPost]
        //[AuthorizeJwt(Roles = "Analytics")]
        public async Task<string> AnalyticGetDataDiagram()
        {
            //string lcpartner = ConfigurationManager.ConnectionStrings["Partner"].ConnectionString;
            //HttpClient httpClient = new HttpClient
            //{
            //    BaseAddress = new Uri(lcpartner)
            //};
            //httpClient.DefaultRequestHeaders.Accept.Clear();
            //httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            //var token = HttpContext.Request.Cookies["lcmanageruserdata"].Value;
            //httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            Dictionary<string, List<int>> data = new Dictionary<string, List<int>>();
            int year = DateTime.Now.Year;
            GainOperatorPeriodRequest gainOperatorPeriodRequest = new GainOperatorPeriodRequest
            {
                Operator = Convert.ToInt16(JwtProps.GetOperator()),
                From = new DateTime(year, 1, 1),
                To = new DateTime(year + 1, 1, 1)
            };

            try
            {
                gainOperatorPeriodRequest.Partner = JwtProps.GetPartner();
            }
            catch { }
            try
            {
                gainOperatorPeriodRequest.Pos = JwtProps.GetPos();
            }
            catch { }

            HttpResponseMessage responseMessage = await HttpClientService.PostAsync("api/values/GainOperatorPeriod", gainOperatorPeriodRequest);
            if (responseMessage.IsSuccessStatusCode)
            {
                GainOperatorPeriodResponse gainOperatorPeriodResponse = await responseMessage.Content.ReadAsAsync<GainOperatorPeriodResponse>();
                List<int> avgCheque = new List<int>();
                List<int> gain = new List<int>();
                for (int i = 1; i <= 12; i++)
                {
                    avgCheque.Add(Convert.ToInt32(Math.Round(gainOperatorPeriodResponse.GainOperatorPeriod.Where(x => x.Month == i).Select(x => x.AvgCheque).FirstOrDefault())));
                    gain.Add(Convert.ToInt32(Math.Round(gainOperatorPeriodResponse.GainOperatorPeriod.Where(x => x.Month == i).Select(x => x.Gain).FirstOrDefault())));
                }
                data.Add("avgCheque", avgCheque);
                data.Add("gain", gain);
            }

            RefundOperatorPeriodRequest refundOperatorPeriodRequest = new RefundOperatorPeriodRequest
            {
                Operator = Convert.ToInt16(JwtProps.GetOperator()),
                From = new DateTime(year, 1, 1),
                To = new DateTime(year + 1, 1, 1)
            };
            try
            {
                refundOperatorPeriodRequest.Partner = JwtProps.GetPartner();
            }
            catch { }
            try
            {
                refundOperatorPeriodRequest.Pos = JwtProps.GetPos();
            }
            catch { }

            responseMessage = await HttpClientService.PostAsync("api/values/RefundOperatorPeriod", refundOperatorPeriodRequest);
            if (responseMessage.IsSuccessStatusCode)
            {
                RefundOperatorPeriodResponse refundOperatorPeriodResponse = await responseMessage.Content.ReadAsAsync<RefundOperatorPeriodResponse>();
                List<int> refunds = new List<int>();
                for (int i = 1; i <= 12; i++)
                {
                    refunds.Add(Convert.ToInt32(Math.Round(refundOperatorPeriodResponse.RefundOperatorPeriod.Where(x => x.Month == i).Select(x => x.RefundSum).FirstOrDefault())));
                }
                data.Add("refunds", refunds);
            }

            ClientOperatorPeriodRequest clientOperatorPeriodRequest = new ClientOperatorPeriodRequest
            {
                Operator = Convert.ToInt16(JwtProps.GetOperator()),
                From = new DateTime(year, 1, 1),
                To = new DateTime(year + 1, 1, 1)
            };
            try
            {
                clientOperatorPeriodRequest.Partner = JwtProps.GetPartner();
            }
            catch { }
            try
            {
                clientOperatorPeriodRequest.Pos = JwtProps.GetPos();
            }
            catch { }

            responseMessage = await HttpClientService.PostAsync("api/values/ClientOperatorPeriod", refundOperatorPeriodRequest);
            if (responseMessage.IsSuccessStatusCode)
            {
                ClientOperatorPeriodResponse clientOperatorPeriodResponse = await responseMessage.Content.ReadAsAsync<ClientOperatorPeriodResponse>();
                List<int> clientsQty = new List<int>();
                for (int i = 1; i <= 12; i++)
                {
                    clientsQty.Add(clientOperatorPeriodResponse.ClientOperatorPeriod.Where(x => x.Month == i).Select(x => x.ClientQty).FirstOrDefault());
                }
                data.Add("clientsQty", clientsQty);
            }
            if (data.Count > 0)
            {
                return JsonConvert.SerializeObject(data);
            }

            return "";
        }

        [HttpPost]
        [AuthorizeJwt]
        public string ClientCreate(string Card, string Phone, string Name, string Surname, string Patronymic, string Email, string Birthdate, string AllowSms, string AllowEmail, int Gender, string Promocode)
        {
            //string lcpartner = ConfigurationManager.ConnectionStrings["Partner"].ConnectionString;
            //HttpClient httpClient = new HttpClient
            //{
            //    BaseAddress = new Uri(lcpartner)
            //};
            //httpClient.DefaultRequestHeaders.Accept.Clear();
            //httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            //var token = HttpContext.Request.Cookies["lcmanageruserdata"].Value;
            //httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            ClientCreateRequest clientCreateRequest = new ClientCreateRequest();
            clientCreateRequest.AgreePersonalData = true;
            clientCreateRequest.ClientSetPassword = true;
            try
            {
                clientCreateRequest.AllowEmail = Convert.ToBoolean(AllowEmail);
            }
            catch { }
            try
            {
                clientCreateRequest.AllowSms = Convert.ToBoolean(AllowSms);
            }
            catch { }
            try
            {
                clientCreateRequest.Birthdate = Convert.ToDateTime(Birthdate);
            }
            catch { }
            clientCreateRequest.Email = Email;
            clientCreateRequest.Patronymic = Patronymic;
            clientCreateRequest.Surname = Surname;
            clientCreateRequest.Name = Name;
            clientCreateRequest.Gender = Gender;
            try
            {
                clientCreateRequest.Phone = Convert.ToInt64(Phone);
            }
            catch { }
            try
            {
                clientCreateRequest.Card = Convert.ToInt64(Card);
            }
            catch { }
            if (!string.IsNullOrEmpty(Promocode))
            {
                clientCreateRequest.Promocode = Promocode;
            }
            clientCreateRequest.Operator = JwtProps.GetOperator();
            clientCreateRequest.Partner = JwtProps.GetPartner();
            if (clientCreateRequest.Partner == 0)
            {
                try
                {
                    clientCreateRequest.Partner = JwtProps.GetDefaultPartner();
                }
                catch { }
            }
            clientCreateRequest.PosCode = JwtProps.GetPosCode();
            if (string.IsNullOrEmpty(clientCreateRequest.PosCode))
            {
                try
                {
                    clientCreateRequest.PosCode = JwtProps.GetDefaultPosCode();
                }
                catch { }
            }
            HttpResponseMessage responseMessage = HttpClientService.PostAsync("api/values/ClientCreate", clientCreateRequest).Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                ClientCreateResponse clientCreateResponse = responseMessage.Content.ReadAsAsync<ClientCreateResponse>().Result;
                return JsonConvert.SerializeObject(clientCreateResponse);
            }

            return "";
        }

        [HttpPost]
        [AuthorizeJwt]
        public string ActivateCard(string Card, string Phone, string Code)
        {
            //string lcpartner = ConfigurationManager.ConnectionStrings["Partner"].ConnectionString;
            //HttpClient httpClient = new HttpClient
            //{
            //    BaseAddress = new Uri(lcpartner)
            //};
            //httpClient.DefaultRequestHeaders.Accept.Clear();
            //httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            //var token = HttpContext.Request.Cookies["lcmanageruserdata"].Value;
            //httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            ActivateCardResponse activateCardResponse = new ActivateCardResponse();
            try
            {
                ActivateCardRequest activateCardRequest = new ActivateCardRequest
                {
                    Card = Convert.ToInt64(Card),
                    Code = Code,
                    Phone = Convert.ToInt64(Phone),
                    Operator = JwtProps.GetOperator()
                };
                HttpResponseMessage responseMessage = HttpClientService.PostAsync("api/values/ActivateCard", activateCardRequest).Result;
                if (responseMessage.IsSuccessStatusCode)
                {
                    activateCardResponse = responseMessage.Content.ReadAsAsync<ActivateCardResponse>().Result;
                    return JsonConvert.SerializeObject(activateCardResponse);
                }
                else
                {
                    activateCardResponse.ErrorCode = 10;
                    activateCardResponse.Message = "Ошибка получения данных";
                }
            }
            catch (Exception ex)
            {
                activateCardResponse.ErrorCode = 10;
                activateCardResponse.Message = ex.Message;
                return JsonConvert.SerializeObject(activateCardResponse);
            }
            return JsonConvert.SerializeObject(activateCardResponse);
        }

        [AuthorizeJwt]
        [HttpPost]
        public FileResult OperatorSalesReport(
            string from,
            string to,
            string date,
            string shop,
            string phone,
            string operation,
            string cheque,
            string sum,
            string charge,
            string redeem)
        {
            OperatorSalesRequest request = new OperatorSalesRequest
            {
                PosName = string.IsNullOrEmpty(shop) ? null : shop,
                Phone = string.IsNullOrEmpty(phone) ? null : phone,
                Cheque = string.IsNullOrEmpty(cheque) ? null : cheque,
            };
            try
            {
                request.DateBuy = !string.IsNullOrEmpty(date) ? DateTime.ParseExact(date, "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture) : (DateTime?)null;
            }
            catch { }

            try
            {
                request.Operation = !string.IsNullOrEmpty(operation)
                    ? operation.Substring(1, operation.Length - 2)
                    : null;
            }
            catch { }

            if (!string.IsNullOrEmpty(sum))
            {
                try
                {
                    var values = sum.Split('-');
                    request.SumMore = Convert.ToInt32(values[0]);
                    request.SumLess = Convert.ToInt32(values[1]);
                }
                catch { }
            }
            if (!string.IsNullOrEmpty(charge))
            {
                try
                {
                    var values = charge.Split('-');
                    request.ChargeMore = Convert.ToInt32(values[0]);
                    request.ChargeLess = Convert.ToInt32(values[1]);
                }
                catch { }
            }
            if (!string.IsNullOrEmpty(redeem))
            {
                try
                {
                    var values = redeem.Split('-');
                    request.RedeemMore = Convert.ToInt32(values[0]);
                    request.RedeemLess = Convert.ToInt32(values[1]);
                }
                catch { }
            }

            try { request.Operator = JwtProps.GetOperator(); } catch { }
            try { request.Partner = JwtProps.GetPartner(); } catch { }
            try { request.Pos = JwtProps.GetPos(); } catch { }
            if (!string.IsNullOrEmpty(from))
            {
                try
                {
                    request.From = DateTime.ParseExact(from, "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
                }
                catch { }
            }
            else request.From = new DateTime(1970, 1, 1);

            if (!string.IsNullOrEmpty(to))
            {
                try
                {
                    request.To = DateTime.ParseExact(to, "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture).Date.AddDays(1);
                }
                catch { }
            }
            else request.To = DateTime.Now.AddDays(1);

            ReportResponse operatorSalesResponse = new ReportResponse();
            HttpResponseMessage responseMessage = HttpClientService.PostAsync("api/reports/OperatorSales", request).Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                operatorSalesResponse = responseMessage.Content.ReadAsAsync<ReportResponse>().Result;
            }

            string reportName = "Отчёт о продажах по программе лояльности";
            if (!string.IsNullOrEmpty(from)) reportName = reportName + " с " + from;
            if (!string.IsNullOrEmpty(to)) reportName = reportName + " по " + to;
            reportName = reportName + ".xlsx";
            return File(operatorSalesResponse.Report, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", reportName);
        }

        [AuthorizeJwt]
        [HttpPost]
        public FileResult OperatorClientReport(
            string fio,
            string phone,
            string email,
            string birthdate,
            string sex,
            string type,
            string card,
            string level,
            string balance)
        {
            ReportResponse operatorClientResponse = new ReportResponse();
            try
            {
                ReportOperatorClientRequest operatorClientRequest = new ReportOperatorClientRequest
                {
                    Name = string.IsNullOrEmpty(fio) ? null : fio,
                    Phone = string.IsNullOrEmpty(phone) ? null : phone,
                    Email = string.IsNullOrEmpty(email) ? null : email,
                    Birthdate = !string.IsNullOrEmpty(birthdate) ? DateTime.ParseExact(birthdate, "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture) : (DateTime?)null,
                    Type = string.IsNullOrEmpty(type) ? null : type,
                    Card = string.IsNullOrEmpty(card) ? null : card,
                    Level = string.IsNullOrEmpty(level) ? null : level,
                    Balance = string.IsNullOrEmpty(balance) ? null : balance
                };
                try
                {
                    if (!string.IsNullOrEmpty(sex))
                    {
                        if (sex.ToLower().Contains("мужской")) operatorClientRequest.Sex = 1;
                        else if (sex.ToLower().Contains("женский")) operatorClientRequest.Sex = 0;
                        else operatorClientRequest.Sex = 2;
                    }
                }
                catch { }

                try { operatorClientRequest.Operator = JwtProps.GetOperator(); } catch { }
                try { operatorClientRequest.Partner = JwtProps.GetPartner(); } catch { }
                try { operatorClientRequest.Pos = JwtProps.GetPos(); } catch { }

                HttpResponseMessage responseMessage = HttpClientService.PostAsync("api/reports/OperatorClient", operatorClientRequest).Result;
                if (responseMessage.IsSuccessStatusCode)
                {
                    operatorClientResponse = responseMessage.Content.ReadAsAsync<ReportResponse>().Result;
                }
            }
            catch (Exception ex)
            {
                operatorClientResponse.ErrorCode = 2;
                operatorClientResponse.Message = ex.Message;
            }
            string reportName = string.Format("Клиентская база.xlsx", "");
            return File(operatorClientResponse.Report, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", reportName);
        }

        /// ПЕРЕНЕСЕНО в SHOPS Controller
        /// 
        /// <returns></returns>
        //public string GetPoses()
        //{
        //    //string lcpartner = ConfigurationManager.ConnectionStrings["Partner"].ConnectionString;
        //    //HttpClient httpClient = new HttpClient
        //    //{
        //    //    BaseAddress = new Uri(lcpartner)
        //    //};
        //    //httpClient.DefaultRequestHeaders.Accept.Clear();
        //    //httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        //    //var token = HttpContext.Request.Cookies["lcmanageruserdata"].Value;
        //    //httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
        //    OperatorPosResponse operatorPosResponse = new OperatorPosResponse();
        //    try
        //    {
        //        OperatorPosRequest operatorPosRequest = new OperatorPosRequest
        //        {
        //            Operator = JwtProps.GetOperator()
        //        };
        //        HttpResponseMessage responseMessage = HttpClientService.PostAsync("api/values/OperatorPos", operatorPosRequest).Result;
        //        if (responseMessage.IsSuccessStatusCode)
        //        {
        //            operatorPosResponse = responseMessage.Content.ReadAsAsync<OperatorPosResponse>().Result;
        //            if (operatorPosResponse.ErrorCode == 0 && string.IsNullOrEmpty(operatorPosResponse.Message))
        //            {
        //                posdata data = new posdata();
        //                int i = 0;
        //                foreach (var c in operatorPosResponse.Poses)
        //                {
        //                    i++;
        //                    pos g = new pos
        //                    {
        //                        chek = "<input type='checkbox' class='checkbox' name='checkbox" + i.ToString() + "' id='checkbox" + i.ToString() + "'><label for='checkbox" + i.ToString() + "'></label>",
        //                        id = i.ToString(),
        //                        addres = c.Address,
        //                        city = c.City,
        //                        region = c.Region
        //                    };
        //                    data.data.Add(g);
        //                }
        //                return JsonConvert.SerializeObject(data);
        //            }
        //            return JsonConvert.SerializeObject(operatorPosResponse);
        //        }
        //        else
        //        {
        //            operatorPosResponse.ErrorCode = 10;
        //            operatorPosResponse.Message = "Ошибка получения данных";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        operatorPosResponse.ErrorCode = 2;
        //        operatorPosResponse.Message = ex.Message;
        //    }
        //    return JsonConvert.SerializeObject(operatorPosResponse);
        //}

        [AuthorizeJwt]
        public string GetFaq()
        {
            GetFaqRequest request = new GetFaqRequest
            {
                Operator = JwtProps.GetOperator(),
                LCManager = true
            };
            HttpResponseMessage responseMessage = HttpClientService.PostAsync("api/site/GetFaq", request).Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                GetFaqResponse faqResponse = responseMessage.Content.ReadAsAsync<GetFaqResponse>().Result;
                faqdata data = new faqdata();
                //List<Faq> faq = new List<Faq>();
                for (int i = 0, n = faqResponse.FaqData.Count; i < n; i++)
                {
                    Faq f = new Faq
                    {
                        id = (i + 1).ToString(),
                        name = faqResponse.FaqData[i].Question,
                        text = faqResponse.FaqData[i].Answer,
                        chek = "<input type='checkbox' class='checkbox' name='checkbox5' id='checkbox5'><label for='checkbox5'></label>",
                        lorem = @"<tr>
                                    <td></td>
                                    <td colspan='2' style='color:#000;'>" + faqResponse.FaqData[i].Answer + @"</td>
                                </tr>"
                    };
                    data.data.Add(f);
                }
                return JsonConvert.SerializeObject(data);
            }
            return "";
        }

        [HttpPost]
        [AuthorizeJwt(Roles = "ClientsCreateClients")]
        public JsonResult OperatorClientImportFromExcel(HttpPostedFileBase file)
        {
            if (Request.Files.Count > 0)
            {
                short partner = JwtProps.GetPartner();
                if (partner == 0)
                {
                    partner = JwtProps.GetDefaultPartner();
                }

                string posCode = JwtProps.GetPosCode();
                if (String.IsNullOrEmpty(posCode))
                {
                    posCode = JwtProps.GetDefaultPosCode();
                }

                ClientImportRequest importRequest = new ClientImportRequest();
                importRequest.Operator = JwtProps.GetOperator();
                importRequest.Partner = partner;
                importRequest.PosCode = posCode;
                using (var binaryReader = new BinaryReader(Request.Files[0].InputStream))
                {
                    importRequest.ExcelFile = binaryReader.ReadBytes(Request.Files[0].ContentLength);
                }
                ClientImportResponse importResponse = new ClientImportResponse();
                try
                {                    
                    HttpResponseMessage responseMessage = HttpClientService.PostAsync("api/values/ClientImport", importRequest).Result;
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        importResponse = responseMessage.Content.ReadAsAsync<ClientImportResponse>().Result;
                        if (importResponse.ErrorCode == 0 && string.IsNullOrEmpty(importResponse.Message))
                        {
                            return Json(new { success = true });

                        }
                        return Json(new { success = false, message = importResponse.Message });
                    }
                    importResponse.ErrorCode = 10;
                    importResponse.Message = "Ошибка импорта данных";

                }
                catch (Exception e)
                {
                }

                return Json(new { success = true });
            }

            //ПОКА ЗАГЛУШКА ВОЗВРАЩАЮЩАЯ УСПЕХ
            //После уточнения механизма загрузки данных будет реализовано
            return Json(new { success = true });
        }
    }
}