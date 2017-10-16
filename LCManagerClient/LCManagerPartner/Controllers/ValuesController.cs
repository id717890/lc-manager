using LCManagerPartner.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LCManagerPartner.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/values")]
    public class ValuesController : ApiController
    {
        static string connectionString = ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString;
        SqlConnection cnn = new SqlConnection(connectionString);

        //public Partner() { cnn.Open(); }
        //~Partner() { cnn.Close(); }

        [HttpPost]
        [Route("BalanceGet")]
        public BalanceGetResponse BalanceGet(BalanceGetRequest request)
        {
            var result = new ServerBalanceGetResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("Redeem")]
        public RedeemResponse Redeem(RedeemRequest request)
        {
            var result = new ServerRedeemResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("ChequeAdd")]
        public ChequeAddResponse ChequeAdd(ChequeAddRequest request)
        {
            var result = new ServerChequeAddResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }
        [HttpPost]
        [Route("GetAllShopsByPartner")]
        public GetPosesResponse GetAllShopsByPartner(GetPosesRequest request)
        {
            var result = new ServerGetPosesResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("GetCheques")]
        public GetChequesResponse GetCheques(GetChequesRequest request)
        {
            var result = new ServerGetChequesResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("GetConfirmCode")]
        public GetConfirmCodeResponse GetConfirmCode(GetConfirmCodeRequest request)
        {
            var result = new ServerGetConfirmCodeResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("SetClientPassword")]
        public SetClientPasswordResponse SetClientPassword(SetClientPasswordRequest request)
        {
            var result = new ServerSetClientPasswordResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("GetRegistrationUser")]
        public GetRegistrationUserResponse GetRegistrationUser(GetRegistrationUserRequest request)
        {
            var result = new ServerGetRegistrationUserResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("GetSendVerificationCode")]
        public GetSendVerificationCodeResponse GetSendVerificationCode(GetSendVerificationCodeRequest request)
        {
            var result = new ServerGetSendVerificationCodeResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("ClientLogin")]
        public ClientLoginResponse ClientLogin(ClientLoginRequest request)
        {
            var result = new ServerClientLoginResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("SendEmailCode")]
        public SendEmailCodeResponse SendEmailCode(SendEmailCodeRequest request)
        {
            var result = new ServerSendEmailCodeResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("ValidateEmail")]
        public ValidateEmailResponse ValidateEmail(ValidateEmailRequest request)
        {
            var result = new ServerValidateEmailResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("AddPhone")]
        public AddPhoneResponse AddPhone(AddPhoneRequest request)
        {
            var result = new ServerAddPhoneResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("DeletePhone")]
        public DeletePhoneResponse DeletePhone(DeletePhoneRequest request)
        {
            var result = new ServerDeletePhoneResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("PartnerFullInfo")]
        public PartnerFullInfoResponse PartnerFullInfo(PartnerFullInfoRequest request)
        {
            var result = new ServerPartnerFullInfoResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("Refund")]
        public RefundResponse Refund(RefundRequest request)
        {
            var result = new ServerRefundResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("OperatorStatistics")]
        public OperatorStatisticsResponse OperatorStatistics(OperatorStatisticsRequest request)
        {
            var result = new ServerOperatorStatisticsResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("GetCampaigns")]
        public GetCampaignsResponse GetCampaigns(GetCampaignsRequest request)
        {
            var result = new ServerGetCampaignsResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("ClientInfo")]
        public GetClientInfoResponse ClientInfo(GetClientInfoRequest request)
        {
            var result = new ServerGetClientInfoResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("ClientUpdate")]
        public SetClientUpdateResponse ClientUpdate(SetClientUpdateRequest request)
        {
            var result = new ServerSetClientUpdate();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("CancelLastCheque")]
        public CancelLastChequeResponse CancelLastCheque(CancelLastChequeRequest request)
        {
            var result = new ServerCancelLastCheque();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("ClientCreate")]
        public ClientCreateResponse ClientCreate(ClientCreateRequest request)
        {
            var result = new ServerClientCreate();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("GetClient")]
        public GetClientResponse GetClient(GetClientRequest request)
        {
            var result = new ServerGetClientResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("GetChequesByCard")]
        public GetChequesByCardResponse GetChequesByCard(GetChequesByCardRequest request)
        {
            var result = new ServerGetChequesByCardResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("ClientImport")]
        public ClientImportResponse ClientImport(ClientImportRequest request)
        {
            var result = new ServerClientImportResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("Merge")]
        public MergeResponse Merge(MergeRequest request)
        {
            var result = new ServerMergeResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("ClientInfoArray")]
        public GetClientInfoArrayResponse ClientInfoArray(GetClientInfoRequest request)
        {
            var result = new ServerGetClientInfoArrayResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("PosStatistics")]
        public PosStatisticsResponse PosStatistics(PosStatisticsRequest request)
        {
            var result = new ServerPosStatisticsResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("ChequeAggregation")]
        public ChequeAggregationResponse ChequeAggregation(ChequeAggregationRequest request)
        {
            var result = new ServerGetChequeAggregation();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("ClientAggregation")]
        public ClientAggregationResponse ClientAggregation(ClientAggregationRequest request)
        {
            var result = new ServerClientAggregationResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        //[SoapHeader("User", Required = true)]
        [HttpPost]
        [Route("BonusAdd")]
        public BonusAddResponse BonusAdd(BonusAddRequest request)
        {
            //if (User.IsValid(cnn, request.Operator))
            //{
            var result = new ServerBonusAdd();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
            //}
            //else
            //{
            //    var returnValue = new BonusAddResponse();
            //    returnValue.Message = "Неправильные логин/пароль";
            //    return returnValue;
            //}
        }

        [HttpPost]
        [Route("ChequesBonuses")]
        public GetChequesBonusesResponse ChequesBonuses(GetChequesBonusesRequest request)
        {
            var result = new ServerGetChequesBonusesResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("OperatorClients")]
        public OperatorClientResponse OperatorClients(OperatorClientRequest request)
        {
            var result = new ServerOperatorClient();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("BuysImport")]
        public BuysImportResponse BuysImport(BuysImportRequest request)
        {
            var result = new ServerBuysImport();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("CardBonuses")]
        public CardBonusesResponse CardBonuses(CardBonusesRequest request)
        {
            var result = new ServerCardBonuses();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("FastBonus")]
        public FastBonusCreateResponse FastBonus(FastBonusCreateRequest request)
        {
            var result = new ServerFastBonusCreateResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("ClientUpdateLevel")]
        public ClientUpdateLevelResponse ClientUpdateLevel(ClientUpdateLevelRequest request)
        {
            var result = new ServerClientUpdateLevel();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        // GET api/values
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/values/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/values
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/values/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //public void Delete(int id)
        //{
        //}
    }
}
