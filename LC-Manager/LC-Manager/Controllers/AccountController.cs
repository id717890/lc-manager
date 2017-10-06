using LC_Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace LC_Manager.Controllers
{
    public class AccountController : Controller
    {
        [AllowAnonymous]
        // GET: Account
        public ActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserModel user, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var client = new LoyconClient.ServiceClientSoapClient();
                string phone = user.Phone.Substring(4).Replace(")", "").Replace("-", "").Replace(" ", "");
                LoyconClient.ClientLoginRequest request = new LoyconClient.ClientLoginRequest
                                                        {
                                                            Login = Convert.ToInt64(phone),
                                                            Password = user.Password
                                                        };
                var d = client.ClientLogin(request);
                if (d.ClientID > 0 && d.ErrorCode == 0)
                {
                    FormsAuthentication.SetAuthCookie(user.Phone, true);
                }
                return RedirectToAction("Index", "Home");
                //return RedirectToAction(returnUrl);
            }
            return View(user);
        }

        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
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
                var client = new LoyconClient.ServiceClientSoapClient();
                string phone = model.Phone.Substring(4).Replace(")", "").Replace("-", "").Replace(" ", "");
                LoyconClient.GetSendVerificationCodeRequest request = new LoyconClient.GetSendVerificationCodeRequest
                {
                    Phone = Convert.ToInt64(phone),
                };
                var d = client.GetSendVerificationCode(request);
                if (d.ErrorCode == 0)
                {
                    VerificationCodeModel codeModel = new VerificationCodeModel { Phone = Convert.ToInt64(phone) };
                    TempData["Phone"] = codeModel.Phone;
                    return RedirectToAction("VerificationCode", "Account");
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
                var phone = TempData["Phone"];
                var client = new LoyconClient.ServiceClientSoapClient();
                LoyconClient.GetConfirmCodeRequest request = new LoyconClient.GetConfirmCodeRequest
                {
                    Code = verificationCode.Code,
                    Phone = Convert.ToInt64(phone)
                };
                var confirm = client.GetConfirmCode(request);
                if(confirm.ErrorCode == 0)
                {
                    TempData["Code"] = verificationCode.Code;
                    return RedirectToAction("NewPassword", "Account");
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
            if(ModelState.IsValid)
            {
                var phone = TempData["Phone"];
                var code = TempData["Code"];
                var client = new LoyconClient.ServiceClientSoapClient();
                LoyconClient.SetClientPasswordRequest request = new LoyconClient.SetClientPasswordRequest
                {
                    Code = code.ToString(),
                    Phone = Convert.ToInt64(phone),
                    Password = passwordModel.Password
                };
                var response = client.SetClientPassword(request);
                if(response.ErrorCode == 0)
                {
                    return RedirectToAction("ChangePasswordSuccess", "Account");
                }
            }

            return View(passwordModel);
        }

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }
    }
}