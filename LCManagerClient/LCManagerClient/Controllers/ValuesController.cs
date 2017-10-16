using LCManagerClient.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Services;

namespace LCManagerClient.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/values")]
    public class ValuesController : ApiController
    {        
        static string connectionString = ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString;
        SqlConnection cnn = new SqlConnection(connectionString);

        //public ServiceClient() { cnn.Open(); }
        //~ServiceClient() { cnn.Close(); }

        //[WebMethod]
        [HttpPost]
        [Route("GetConfirmCode")]
        public GetConfirmCodeResponse GetConfirmCode(GetConfirmCodeRequest request)
        {
            Log.Information("Call GetConfirmCode");
            var result = new ServerGetConfirmCodeResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        //[WebMethod]
        [HttpPost]
        [Route("SetClientPassword")]
        public SetClientPasswordResponse SetClientPassword(SetClientPasswordRequest request)
        {
            var result = new ServerSetClientPasswordResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        //[WebMethod]
        [HttpPost]
        [Route("GetRegistrationUser")]
        public GetRegistrationUserResponse GetRegistrationUser(GetRegistrationUserRequest request)
        {
            Log.Information("Call GetRegistrationUser");
            var result = new ServerGetRegistrationUserResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        //[WebMethod]
        [HttpPost]
        [Route("GetSendVerificationCode")]
        public GetSendVerificationCodeResponse GetSendVerificationCode(GetSendVerificationCodeRequest request)
        {
            Log.Information("Call GetSendVerificationCode");
            var result = new ServerGetSendVerificationCodeResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        //[WebMethod]
        [HttpPost]
        [Route("ClientLogin")]
        public ClientLoginResponse ClientLogin(ClientLoginRequest request)
        {
            var result = new ServerClientLoginResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        //[WebMethod]
        [HttpPost]
        [Route("GetCheques")]
        public GetChequesResponse GetCheques(GetChequesRequest request)
        {
            var result = new ServerGetChequesResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        //[WebMethod]
        [HttpPost]
        [Route("GetClient")]
        public GetClientResponse GetClient(GetClientRequest request)
        {
            var result = new ServerGetClientResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        //[WebMethod]
        [HttpPost]
        [Route("ChangeClient")]
        public ChangeClientResponse ChangeClient(ChangeClientRequest request)
        {
            var result = new ServerChangeClientResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        //[WebMethod]
        [HttpPost]
        [Route("GetClientCards")]
        public GetClientCardsResponse GetClientCards(GetClientCardsRequest request)
        {
            var result = new ServerGetClientCardsResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        //[WebMethod]
        [HttpPost]
        [Route("BalanceGet")]
        public BalanceGetResponse BalanceGet(BalanceGetRequest request)
        {
            var result = new ServerBalanceGetResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        //[WebMethod]
        [HttpPost]
        [Route("GetPartners")]
        public GetPartnersResponse GetPartners(GetPartnersRequest request)
        {
            var result = new ServerGetPartnersResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        //[WebMethod]
        [HttpPost]
        [Route("GetCampaigns")]
        public GetCampaignsResponse GetCampaigns(GetCampaignsRequest request)
        {
            var result = new ServerGetCampaignsResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        //[WebMethod]
        [HttpPost]
        [Route("ClientPartnerSelect")]
        public ClientPartnerSelectResponse ClientPartnerSelect(ClientPartnerSelectRequest request)
        {
            var result = new ServerClientPartnerSelectResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        //[WebMethod]
        [HttpPost]
        [Route("ClientCampaignSelect")]
        public ClientCampaignSelectResponse ClientCampaignSelect(ClientCampaignSelectRequest request)
        {
            var result = new ServerClientCampaignSelectResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        //[WebMethod]
        [HttpPost]
        [Route("ClientAddCard")]
        public ClientAddCardResponse ClientAddCard(ClientAddCardRequest request)
        {
            var result = new ServerClientAddCardResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        //[WebMethod]
        [HttpPost]
        [Route("LeaveMessage")]
        public LeaveMessageResponse LeaveMessage(LeaveMessageRequest request)
        {
            var result = new ServerLeaveMessageResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        //[WebMethod]
        [HttpPost]
        [Route("SendEmailCode")]
        public SendEmailCodeResponse SendEmailCode(SendEmailCodeRequest request)
        {
            var result = new ServerSendEmailCodeResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        //[WebMethod]
        [HttpPost]
        [Route("ValidateEmail")]
        public ValidateEmailResponse ValidateEmail(ValidateEmailRequest request)
        {
            var result = new ServerValidateEmailResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        //[WebMethod]
        [HttpPost]
        [Route("AddEmail")]
        public AddEmailResponse AddEmail(AddEmailRequest request)
        {
            var result = new ServerAddEmailResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        //[WebMethod]
        [HttpPost]
        [Route("AddIDFB")]
        public AddIDFBResponse AddIDFB(AddIDFBRequest request)
        {
            var result = new ServerAddIDFBResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        //[WebMethod]
        [HttpPost]
        [Route("AddIDOK")]
        public AddIDOKResponse AddIDOK(AddIDOKRequest request)
        {
            var result = new ServerAddIDOKResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        //[WebMethod]
        [HttpPost]
        [Route("AddIDVK")]
        public AddIDVKResponse AddIDVK(AddIDVKRequest request)
        {
            var result = new ServerAddIDVKResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        //[WebMethod]
        [HttpPost]
        [Route("DeletePhone")]
        public DeletePhoneResponse DeletePhone(DeletePhoneRequest request)
        {
            var result = new ServerDeletePhoneResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        //[WebMethod]
        [HttpPost]
        [Route("GetPersonalCampaigns")]
        public GetPersonalCampaignsResponse GetPersonalCampaigns(GetPersonalCampaignsRequest request)
        {
            var result = new ServerGetPersonalCampaignsResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        //[WebMethod]
        [HttpPost]
        [Route("SelectPreferences")]
        public SelectPreferencesResponse SelectPreferences(SelectPreferencesRequest request)
        {
            var result = new ServerSelectPreferencesResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        //[WebMethod]
        [HttpPost]
        [Route("AddPhone")]
        public AddPhoneResponse AddPhone(AddPhoneRequest request)
        {
            var result = new ServerAddPhoneResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        //[WebMethod]
        [HttpPost]
        [Route("ClientGetPreferences")]
        public ClientPreferencesResponse ClientGetPreferences(ClientPreferencesRequest request)
        {
            var result = new ServerClientPreferencesResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        //[WebMethod]
        [HttpPost]
        [Route("GetChequeDetail")]
        public ChequeDetailResponse GetChequeDetail(ChequeDetailRequest request)
        {
            var result = new ServerChequeDetailResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        //[WebMethod]
        [HttpPost]
        [Route("SetClientDevice")]
        public AddDeviceResponse SetClientDevice(AddDeviceRequest request)
        {
            var result = new ServerAddDeviceResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        //[WebMethod]
        [HttpPost]
        [Route("SendPush")]
        public SendPushResponse SendPush(SendPushRequest request)
        {
            var result = new ServerSendPushResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        //[WebMethod]
        [HttpPost]
        [Route("SendPushApple")]
        public SendPushResponse SendPushApple(SendPushRequest request)
        {
            var result = new ServerSendPushAppleResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        //[WebMethod]
        [HttpPost]
        [Route("CardStatistics")]
        public CardStatisticsResponse CardStatistics(CardStatisticsRequest request)
        {
            var result = new ServerCardStatisticsResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        //[WebMethod]
        [HttpPost]
        [Route("ClientInfo")]
        public GetClientInfoResponse ClientInfo(GetClientInfoRequest request)
        {
            var result = new ServerGetClientInfoResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        //[WebMethod]
        [HttpPost]
        [Route("CardAggregation")]
        public CardAggregationResponse CardAggregation(CardAggregationRequest request)
        {
            var result = new ServerGetCardAggregation();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        //[WebMethod]
        [HttpPost]
        [Route("ClientCreate")]
        public ClientCreateResponse ClientCreate(ClientCreateRequest request)
        {
            var result = new ServerClientCreate();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        //[WebMethod]
        [HttpPost]
        [Route("ClientUpdate")]
        public SetClientUpdateResponse ClientUpdate(SetClientUpdateRequest request)
        {
            var result = new ServerSetClientUpdate();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        //[WebMethod]
        [HttpPost]
        [Route("ClientPasswordChange")]
        public ClientPasswordChangeResponse ClientPasswordChange(ClientPasswordChangeRequest request)
        {
            var result = new ServerClientPasswordChange();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        //[WebMethod]
        [HttpPost]
        [Route("ClientUpdateCommunication")]
        public ClientUpdateCommunicationResponse ClientUpdateCommunication(ClientUpdateCommunicationRequest request)
        {
            var result = new ServerClientUpdateCommunication();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        //[WebMethod]
        [HttpPost]
        [Route("ClientBonuses")]
        public ClientBonusesResponse ClientBonuses(ClientBonusesRequest request)
        {
            var result = new ServerClientBonuses();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        //[WebMethod]
        [HttpPost]
        [Route("CardBonuses")]
        public CardBonusesResponse CardBonuses(CardBonusesRequest request)
        {
            var result = new ServerCardBonuses();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("ChequeMaxSumRedeem")]
        public ChequeMaxSumRedeemResponse ChequeMaxSumRedeem(ChequeMaxSumRedeemRequest request)
        {
            var result = new ServerChequeMaxSumRedeem();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }
        //// GET api/values
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
