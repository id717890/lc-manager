using LC_Manager.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace LC_Manager.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Sales()
        {
            

            return View();
        }

        public ActionResult Terminal()
        {
            return View();
        }

        [HttpPost]
        public string Terminal(RegisterModel model)
        {
            string connectionstring = ConfigurationManager.ConnectionStrings["Client"].ConnectionString;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(connectionstring);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var phone = model.Phone.Substring(4).Replace(")", "").Replace("-", "").Replace(" ", "");

            //var client = new LoyconClient.ServiceClientSoapClient();
            //var phone = model.Phone.Substring(4).Replace(")", "").Replace("-", "").Replace(" ", "");
            GetConfirmCodeRequest confirmRequest = new GetConfirmCodeRequest
            {
                Code = model.Code,
                Phone = Convert.ToInt64(phone)
            };
            //var confirmResponse = client.GetConfirmCode(confirmRequest);
            HttpResponseMessage confirmResponse = client.PostAsJsonAsync("api/values/GetConfirmCode", confirmRequest).Result;
            if (confirmResponse.IsSuccessStatusCode)
            {
                GetConfirmCodeResponse confirmCodeResponse = confirmResponse.Content.ReadAsAsync<GetConfirmCodeResponse>().Result;
                if (confirmCodeResponse.ErrorCode == 0)
                {
                    GetRegistrationUserRequest registrationRequest = new GetRegistrationUserRequest
                    {
                        AgreePersonalData = true,
                        Phone = Convert.ToInt64(phone)
                    };
                    if (!string.IsNullOrEmpty(model.FriendPhone))
                    {
                        string friendPhone = model.FriendPhone.Substring(4).Replace(")", "").Replace("-", "").Replace(" ", "");
                        registrationRequest.FriendPhone = Convert.ToInt64(friendPhone);
                    }
                    //var registrationResponse = client.GetRegistrationUser(registrationRequest);
                    HttpResponseMessage registrationResponse = client.PostAsJsonAsync("api/values/GetRegistrationUser", registrationRequest).Result;
                    if (registrationResponse.IsSuccessStatusCode)
                    {
                        GetRegistrationUserResponse registrationUserResponse = registrationResponse.Content.ReadAsAsync<GetRegistrationUserResponse>().Result;
                        if (registrationUserResponse.ErrorCode == 0)
                        {
                            int gender = 0;
                            if (model.Gender.Equals("Мужской"))
                            {
                                gender = 1;
                            }
                            else if (model.Gender.Equals("Женский"))
                            {
                                gender = -1;
                            }
                            Client clientData = new Client
                            {
                                allowemail = true,
                                allowpush = true,
                                allowsms = true,
                                email = model.Email,
                                birthdate = !string.IsNullOrEmpty(model.BirthDate) ? Convert.ToDateTime(model.BirthDate) : new DateTime(1900, 1, 1),
                                firstname = model.Name,
                                lastname = model.Surname,
                                middlename = model.Patronymic,
                                id = registrationUserResponse.Client,
                                phone = Convert.ToInt64(phone),
                                gender = gender
                            };
                            ChangeClientRequest clientRequest = new ChangeClientRequest
                            {
                                ClientData = clientData
                            };

                            HttpResponseMessage changeResponse = client.PostAsJsonAsync("api/values/ChangeClient", clientRequest).Result;
                            if(changeResponse.IsSuccessStatusCode)
                            {
                                ChangeClientResponse changeClientResponse = changeResponse.Content.ReadAsAsync<ChangeClientResponse>().Result;
                            }
                        }
                        else
                        {
                            return JsonConvert.SerializeObject(registrationUserResponse);
                            //return View(registrationUserResponse);
                        }
                    }
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
        public ActionResult AjaxRegister(RegisterModel data)
        {
            string connectionstring = ConfigurationManager.ConnectionStrings["Client"].ConnectionString;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(connectionstring);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var phone = data.Phone.Substring(4).Replace(")", "").Replace("-", "").Replace(" ", "");
            GetSendVerificationCodeRequest request = new GetSendVerificationCodeRequest
            {
                Phone = Convert.ToInt64(phone)
            };

            HttpResponseMessage response = client.PostAsJsonAsync<GetSendVerificationCodeRequest>("api/values/GetSendVerificationCode", request).Result;
            if (response.IsSuccessStatusCode)
            {
                return Json("Ok");                
            }
            else
            {
                return Json("Error");
            }

            //var client = new LoyconClient.ServiceClientSoapClient();
            //var phone = data.Phone.Substring(4).Replace(")", "").Replace("-", "").Replace(" ", "");
            //LoyconClient.GetSendVerificationCodeRequest request = new LoyconClient.GetSendVerificationCodeRequest
            //{
            //    Phone = Convert.ToInt64(phone)
            //};
            //var response = client.GetSendVerificationCode(request);
            return Json("");
        }

        [HttpPost]
        public string SearchClient(string searchClient)
        {
            GetClientInfoRequest clientInfoRequest = new GetClientInfoRequest();
            if(searchClient.Length == 4 || searchClient.Length == 6 || searchClient.Length == 10)
            {
                try
                {
                    clientInfoRequest.Phone = Convert.ToInt64(searchClient);
                }
                catch { }
            }
            else if(searchClient.Length == 8 || searchClient.Length == 13)
            {
                try
                {
                    clientInfoRequest.Card = Convert.ToInt64(searchClient);
                }
                catch { }
            }

            string connectionstring = ConfigurationManager.ConnectionStrings["Client"].ConnectionString;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(connectionstring);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.PostAsJsonAsync("api/values/ClientInfo", clientInfoRequest).Result;
            if (response.IsSuccessStatusCode)
            {
                GetClientInfoResponse clientInfoResponse = response.Content.ReadAsAsync<GetClientInfoResponse>().Result;
                if(clientInfoResponse.ErrorCode == 0)
                {
                    return JsonConvert.SerializeObject(clientInfoResponse);
                    //return Json(clientInfoResponse);
                }
                return JsonConvert.SerializeObject("Ok");
            }
            else
            {
                return JsonConvert.SerializeObject("Error");
            }            
        }

        public ActionResult Clients()
        {
            return View();
        }

        [HttpPost]
        public string TerminalMaxSumRedeem(TerminalRedeemSumModel model)
        {
            ChequeMaxSumRedeemRequest chequeMaxSumRedeemRequest = new ChequeMaxSumRedeemRequest();
            chequeMaxSumRedeemRequest.Operator = 2;
            chequeMaxSumRedeemRequest.Phone = Convert.ToInt64(model.Phone.Substring(4).Replace(")", "").Replace("-", "").Replace(" ", ""));
            chequeMaxSumRedeemRequest.ChequeSum = Convert.ToDecimal(model.Sum);
            string connectionstring = ConfigurationManager.ConnectionStrings["Client"].ConnectionString;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(connectionstring);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.PostAsJsonAsync("api/values/ChequeMaxSumRedeem", chequeMaxSumRedeemRequest).Result;
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
        public string TerminalChequeAdd(TerminalChequeModel chequeModel)
        {
            string lcclient = ConfigurationManager.ConnectionStrings["Partner"].ConnectionString;
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(lcclient);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));            
            if(chequeModel.PaidByBonus > 0)
            {
                RedeemRequest redeemRequest = new RedeemRequest();
                redeemRequest.Bonus = chequeModel.PaidByBonus;
                redeemRequest.Card = chequeModel.Card;
                redeemRequest.Partner = 2;
                HttpResponseMessage responseRedeem = httpClient.PostAsJsonAsync("api/values/Redeem", redeemRequest).Result;
                if(responseRedeem.IsSuccessStatusCode)
                {
                    RedeemResponse redeemResponse = responseRedeem.Content.ReadAsAsync<RedeemResponse>().Result;
                    if(redeemResponse.ErrorCode > 0)
                    {
                        return JsonConvert.SerializeObject(redeemResponse);
                    }
                }
            }

            ChequeAddRequest chequeAdd = new ChequeAddRequest();
            /*номер карты, сумма покупки, сумма списаных бонусов, номер, время, код ТТ*/
            chequeAdd.Amount = chequeModel.Amount;
            chequeAdd.Card = chequeModel.Card;
            chequeAdd.ChequeTime = DateTime.Now;
            chequeAdd.PaidByBonus = chequeModel.PaidByBonus;
            chequeAdd.Number = DateTime.Now.ToString("HHmmss");
            chequeAdd.Partner = 2;
            HttpResponseMessage responseCheque = httpClient.PostAsJsonAsync("api/values/ChequeAdd", chequeAdd).Result;
            if(responseCheque.IsSuccessStatusCode)
            {
                ChequeAddResponse chequeAddResponse = responseCheque.Content.ReadAsAsync<ChequeAddResponse>().Result;
                TerminalChequeAddResult result = new TerminalChequeAddResult
                {
                    Amount = chequeAdd.Amount.ToString(),
                    BonusAdd = chequeAddResponse.Bonus.ToString(),
                    BonusRedeem = chequeAdd.PaidByBonus.ToString(),
                    Cash = (chequeAdd.Amount - chequeAdd.PaidByBonus).ToString()
                };
                return JsonConvert.SerializeObject(result);
            }
            return "";
        }

        [HttpPost]
        public string TerminalGetCheques(long Card)
        {
            string lcpartner = ConfigurationManager.ConnectionStrings["Partner"].ConnectionString;
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(lcpartner);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            GetChequesByCardRequest chequesByCardRequest = new GetChequesByCardRequest
            {
                CardNumber = Card
            };
            HttpResponseMessage responseCheques = httpClient.PostAsJsonAsync("api/values/GetChequesByCard", chequesByCardRequest).Result;
            if(responseCheques.IsSuccessStatusCode)
            {
                GetChequesByCardResponse chequesByCardResponse = responseCheques.Content.ReadAsAsync<GetChequesByCardResponse>().Result;
                return JsonConvert.SerializeObject(chequesByCardResponse);
            }
            return "";
        }

        [HttpPost]
        public string TerminalRefund(TerminalRefundModel refundModel)
        {
            string lcpartner = ConfigurationManager.ConnectionStrings["Partner"].ConnectionString;
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(lcpartner);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            RefundRequest refundRequest = new RefundRequest
            {
                Amount = refundModel.ChequeSum,
                Card = refundModel.Card,
                ChequeTime = DateTime.Now,
                PurchaseNumber = refundModel.ChequeNum,
                PurchaseDate = refundModel.ChequeDate,
                Partner = 2
            };
            HttpResponseMessage responseRefund = httpClient.PostAsJsonAsync("api/values/Refund", refundRequest).Result;
            if (responseRefund.IsSuccessStatusCode)
            {
                RefundResponse refundResponse = responseRefund.Content.ReadAsAsync<RefundResponse>().Result;
                return JsonConvert.SerializeObject(refundResponse);
            }
            return "";
        }
    }
}