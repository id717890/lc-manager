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
        public ActionResult Terminal(RegisterModel model)
        {
            string connectionstring = ConfigurationManager.ConnectionStrings["Webapi"].ConnectionString;
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
                            return View();
                        }
                    }
                }
            }
            else
            {
                return View(confirmResponse);
            }
            return View();
        }

        [HttpPost]
        public ActionResult AjaxRegister(RegisterModel data)
        {
            string connectionstring = ConfigurationManager.ConnectionStrings["Webapi"].ConnectionString;
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

            string connectionstring = ConfigurationManager.ConnectionStrings["Webapi"].ConnectionString;
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
    }
}