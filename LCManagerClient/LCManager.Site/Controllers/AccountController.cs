namespace LC_Manager.Controllers
{
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Net.Http;
    using System.Web;
    using System.Web.Mvc;
    using Implementation;
    using Serilog;

    public class AccountController : Controller
    {
        [AllowAnonymous]
        // GET: Account
        public ActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View(new UserModel { Login = "9286669315", Password = "Unk-bonus" });
            //return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserModel user, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["Partner"].ConnectionString;
                HttpClient httpClient = new HttpClient
                {
                    BaseAddress = new Uri(connectionString)
                };
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var pairs = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("username", user.Login),
                    new KeyValuePair<string, string>("password", user.Password),
                    new KeyValuePair<string, string>("grant_type", "password")
                };

                var content = new FormUrlEncodedContent(pairs);

                Log.Information("LCManagerUI. Send auth request for user:{login}", user.Login);
                HttpResponseMessage responseMessage = httpClient.PostAsync("ManagerLogin", content).Result;
                Log.Information("LCManagerUI. Response message IsSuccessStatusCode={state}", responseMessage.IsSuccessStatusCode);
                if (responseMessage.IsSuccessStatusCode)
                {
                    Token token = responseMessage.Content.ReadAsAsync<Token>().Result;
                    #region Обязательно записываем токен в куки
                    Response.SetCookie(new HttpCookie("lcmanager_token")
                    {
                        Value = token.access_token,
                        Expires = DateTime.Now.AddDays(Convert.ToInt32(ConfigurationManager.AppSettings["jwt_token_cookie_expiration"]))
                    });
                    #endregion

                    Log.Information("LCManagerUI. Set refresh_token to cookie. Token = {token}", token.refresh_token);
                    #region Записываем refresh_token в куки
                    Response.SetCookie(new HttpCookie("lcmanager_refresh_token")
                    {
                        Value = token.refresh_token,
                        Expires = DateTime.Now.AddDays(Convert.ToInt32(ConfigurationManager.AppSettings["jwt_refresh_token_cookie_expiration"]))
                }); 
                    #endregion

                    #region Получаем информацию об операторе и записываем в куки
                    OperatorInfoRequest operatorInfoRequest = new OperatorInfoRequest
                    {
                        Operator = JwtProps.GetOperator()
                    };

                    HttpResponseMessage httpResponseMessage = HttpClientService.PostAsync("api/values/OperatorInfo", operatorInfoRequest).Result;
                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        OperatorInfoResponse operatorInfoResponse = httpResponseMessage.Content.ReadAsAsync<OperatorInfoResponse>().Result;
                        Response.SetCookie(new HttpCookie("operatorName")
                        {
                            Value = operatorInfoResponse.OperatorName,
                            Expires = DateTime.Now.AddDays(365)
                        });
                    } 
                    #endregion
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(user);
        }

        public ActionResult Logoff()
        {
            JwtProps.Logout();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgetPassword(ForgetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["Partner"].ConnectionString;
                HttpClient httpClient = new HttpClient
                {
                    BaseAddress = new Uri(connectionString)
                };
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                //var client = new LoyconClient.ServiceClientSoapClient();
                //string phone = model.Phone.Substring(4).Replace(")", "").Replace("-", "").Replace(" ", "");
                ManagerSendCodeRequest request = new ManagerSendCodeRequest
                {
                    Login = model.Login
                };
                HttpResponseMessage responseMessage = httpClient.PostAsJsonAsync("api/values/ManagerSendCode", request).Result;

                //var d = client.GetSendVerificationCode(request);
                if (responseMessage.IsSuccessStatusCode)
                {
                    ManagerSendCodeResponse verificationCodeResponse = responseMessage.Content.ReadAsAsync<ManagerSendCodeResponse>().Result;
                    if (verificationCodeResponse.ErrorCode == 0)
                    {
                        VerificationCodeModel codeModel = new VerificationCodeModel { Phone = verificationCodeResponse.Phone };
                        TempData["Phone"] = codeModel.Phone;
                        return RedirectToAction("VerificationCode", "Account");
                    }
                }
            }

            return View(model);
        }

        public ActionResult VerificationCode()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult VerificationCode(VerificationCodeModel verificationCode)
        {
            if (ModelState.IsValid)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["Partner"].ConnectionString;
                HttpClient httpClient = new HttpClient
                {
                    BaseAddress = new Uri(connectionString)
                };
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var phone = TempData["Phone"];
                //var client = new LoyconClient.ServiceClientSoapClient();
                GetConfirmCodeRequest request = new GetConfirmCodeRequest
                {
                    Code = verificationCode.Code,
                    Phone = Convert.ToInt64(phone)
                };
                HttpResponseMessage responseMessage = httpClient.PostAsJsonAsync("api/values/GetConfirmCode", request).Result;
                if (responseMessage.IsSuccessStatusCode)
                {
                    GetConfirmCodeResponse confirm = responseMessage.Content.ReadAsAsync<GetConfirmCodeResponse>().Result;
                    if (confirm.ErrorCode == 0)
                    {
                        TempData["Code"] = verificationCode.Code;
                        return RedirectToAction("NewPassword", "Account");
                    }
                }
            }
            return View(verificationCode);
        }

        public ActionResult NewPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult NewPassword(NewPasswordModel passwordModel)
        {
            if (ModelState.IsValid)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["Partner"].ConnectionString;
                HttpClient httpClient = new HttpClient
                {
                    BaseAddress = new Uri(connectionString)
                };
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var phone = TempData["Phone"];
                var code = TempData["Code"];
                //var client = new LoyconClient.ServiceClientSoapClient();
                SetClientPasswordRequest request = new SetClientPasswordRequest
                {
                    Code = code.ToString(),
                    Phone = Convert.ToInt64(phone),
                    Password = passwordModel.Password
                };
                HttpResponseMessage response = httpClient.PostAsJsonAsync("api/values/SetManagerPassword", request).Result;
                if (response.IsSuccessStatusCode)
                {
                    SetManagerPasswordResponse clientPasswordResponse = response.Content.ReadAsAsync<SetManagerPasswordResponse>().Result;
                    if (clientPasswordResponse.ErrorCode == 0)
                    {
                        return RedirectToAction("ChangePasswordSuccess", "Account");
                    }
                }
            }

            return View(passwordModel);
        }

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        /// <summary>
        /// Отправка refresh_token на сервер
        /// </summary>
        public static bool RefreshToken(string user, string refreshToken)
        {
            Log.Information("LCManagerUI. Start RefreshToken for user:{user}, token:{token}", user, refreshToken);
            string connectionString = ConfigurationManager.ConnectionStrings["Partner"].ConnectionString;
            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new Uri(connectionString)
            };
            httpClient.DefaultRequestHeaders.Clear();
            var pairs = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("refresh_token", refreshToken),
                new KeyValuePair<string, string>("grant_type", "refresh_token")
            };
            var content = new FormUrlEncodedContent(pairs);
            HttpResponseMessage responseMessage = httpClient.PostAsync("ManagerLogin", content).Result;
            Log.Information("LCManagerUI. RefreshToken Response message IsSuccessStatusCode={state}", responseMessage.IsSuccessStatusCode);
            if (responseMessage.IsSuccessStatusCode)
            {
                Token token = responseMessage.Content.ReadAsAsync<Token>().Result;
                #region Обязательно записываем токен в куки
                System.Web.HttpContext.Current.Response.SetCookie(new HttpCookie("lcmanager_token")
                {
                    Value = token.access_token,
                    Expires = DateTime.Now.AddDays(Convert.ToInt32(ConfigurationManager.AppSettings["jwt_token_cookie_expiration"]))
                });
                Log.Information("LCManagerUI. RefreshToken New RefreshToken = {token}", token.refresh_token);
                System.Web.HttpContext.Current.Response.SetCookie(new HttpCookie("lcmanager_refresh_token")
                {
                    Value = token.refresh_token,
                    Expires = DateTime.Now.AddDays(Convert.ToInt32(ConfigurationManager.AppSettings["jwt_refresh_token_cookie_expiration"]))
                });
                Log.Information("LCManagerUI. RefreshToken success");
                return true;
                #endregion
            }
            Log.Error("LCManagerUI. RefreshToken error. {@response}", responseMessage);
            return false;
        }
    }
}