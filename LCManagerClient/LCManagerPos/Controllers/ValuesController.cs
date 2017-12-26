using LCManagerPos.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LCManagerPos.Controllers
{
    [Authorize]
    [RoutePrefix("api/values")]
    public class ValuesController : ApiController
    {
        static string connectionString = ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString;
        SqlConnection cnn = new SqlConnection(connectionString);

        [HttpPost]
        [Route("BalanceGet")]
        public BalanceGetResponse BalanceGet(BalanceGetRequest request)
        {
            Log.Information("LCManagerPos BalanceGet {Phone}", request.Phone);
            var result = new ServerBalanceGetResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("Redeem")]
        public RedeemResponse Redeem(RedeemRequest request)
        {
            Log.Information("LCManagerPos Redeem {Phone}", request.Phone);
            var result = new ServerRedeemResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("ChequeAdd")]
        public ChequeAddResponse ChequeAdd(ChequeAddRequest request)
        {
            Log.Information("LCManagerPos ChequeAdd {Phone}", request.Phone);
            var result = new ServerChequeAddResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;            
        }

        [HttpPost]
        [Route("Refund")]
        public RefundResponse Refund(RefundRequest request)
        {
            Log.Information("LCManagerPos Refund {Phone}", request.Phone);
            var result = new ServerRefundResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;            
        }

        [HttpPost]
        [Route("CancelLastCheque")]
        public CancelLastChequeResponse CancelLastCheque(CancelLastChequeRequest request)
        {
            Log.Information("LCManagerPos CancelLastCheque {Operator}", request.Operator);
            var result = new ServerCancelLastCheque();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("GetConfirmCode")]
        public GetConfirmCodeResponse GetConfirmCode(GetConfirmCodeRequest request)
        {
            Log.Information("LCManagerPos GetConfirmCode {phone}", request.Phone);
            var result = new ServerGetConfirmCodeResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("SetClientPassword")]
        public SetClientPasswordResponse SetClientPassword(SetClientPasswordRequest request)
        {
            Log.Information("LCManagerPos SetClientPassword {phone}", request.Phone);
            var result = new ServerSetClientPasswordResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("GetRegistrationUser")]
        public GetRegistrationUserResponse GetRegistrationUser(GetRegistrationUserRequest request)
        {
            Log.Information("LCManagerPos GetRegistrationUser {phone}", request.Phone);
            var result = new ServerGetRegistrationUserResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("GetSendVerificationCode")]
        public GetSendVerificationCodeResponse GetSendVerificationCode(GetSendVerificationCodeRequest request)
        {
            Log.Information("LCManagerPos GetSendVerificationCode {phone}", request.Phone);
            var result = new ServerGetSendVerificationCodeResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("ManagerLogin")]
        public ManagerLoginResponse ManagerLogin(ManagerLoginRequest request)
        {
            Log.Information("LCManagerPos ManagerLogin {Phone}", request.Phone);
            var result = new ServerManagerLogin();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("ChangeClient")]
        public ChangeClientResponse ChangeClient(ChangeClientRequest request)
        {
            Log.Information("LCManagerPos ChangeClient {phone}", request.ClientData.phone);
            var result = new ServerChangeClientResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("ClientCreate")]
        public ClientCreateResponse ClientCreate(ClientCreateRequest request)
        {
            Log.Information("LCManagerPos ClientCreate {Phone}", request.Phone);
            var result = new ServerClientCreate();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }
    }
}
