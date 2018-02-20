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
    [Authorize]
    [RoutePrefix("api/client")]
    public class ClientController : ApiController
    {
        static string connectionString = ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString;
        SqlConnection cnn = new SqlConnection(connectionString);


        //Дублирующиеся методы: GetConfirmCode, SetClientPassword, GetRegistrationUser, GetSendVerificationCode, ClientLogin, GetCheques, GetClient, ChangeClient, BalanceGet
        //GetPartners, GetCampaigns, LeaveMessage, SendEmailCode, ValidateEmail, DeletePhone, AddPhone, ClientInfo, ClientCreate, ClientUpdate, CardBonuses, ActivateCard

        [HttpPost]
        public GetConfirmCodeResponse GetConfirmCode(GetConfirmCodeRequest request)
        {
            var result = new ServerGetConfirmCodeResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        public SetClientPasswordResponse SetClientPassword(SetClientPasswordRequest request)
        {
            var result = new ServerSetClientPasswordResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        public GetRegistrationUserResponse GetRegistrationUser(GetRegistrationUserRequest request)
        {
            var result = new ServerGetRegistrationUserResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        public GetSendVerificationCodeResponse GetSendVerificationCode(GetSendVerificationCodeRequest request)
        {
            var result = new ServerGetSendVerificationCodeResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        public ClientLoginResponse ClientLogin(ClientLoginRequest request)
        {
            var result = new ServerClientLoginResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        public GetChequesResponse GetCheques(GetChequesRequest request)
        {
            var result = new ServerGetChequesResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        public GetClientResponse GetClient(GetClientRequest request)
        {
            var result = new ServerGetClientResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        public ChangeClientResponse ChangeClient(ChangeClientRequest request)
        {
            var result = new ServerChangeClientResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        public GetClientCardsResponse GetClientCards(GetClientCardsRequest request)
        {
            var result = new ServerGetClientCardsResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        public BalanceGetResponse BalanceGet(BalanceGetRequest request)
        {
            var result = new ServerBalanceGetResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        public GetPartnersResponse GetPartners(GetPartnersRequest request)
        {
            var result = new ServerGetPartnersResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        public GetCampaignsResponse GetCampaigns(GetCampaignsRequest request)
        {
            var result = new ServerGetCampaignsResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        public ClientPartnerSelectResponse ClientPartnerSelect(ClientPartnerSelectRequest request)
        {
            var result = new ServerClientPartnerSelectResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        public ClientCampaignSelectResponse ClientCampaignSelect(ClientCampaignSelectRequest request)
        {
            var result = new ServerClientCampaignSelectResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        public ClientAddCardResponse ClientAddCard(ClientAddCardRequest request)
        {
            var result = new ServerClientAddCardResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        public LeaveMessageResponse LeaveMessage(LeaveMessageRequest request)
        {
            var result = new ServerLeaveMessageResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        public SendEmailCodeResponse SendEmailCode(SendEmailCodeRequest request)
        {
            var result = new ServerSendEmailCodeResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        public ValidateEmailResponse ValidateEmail(ValidateEmailRequest request)
        {
            var result = new ServerValidateEmailResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        public AddEmailResponse AddEmail(AddEmailRequest request)
        {
            var result = new ServerAddEmailResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        public AddIDFBResponse AddIDFB(AddIDFBRequest request)
        {
            var result = new ServerAddIDFBResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        public AddIDOKResponse AddIDOK(AddIDOKRequest request)
        {
            var result = new ServerAddIDOKResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        public AddIDVKResponse AddIDVK(AddIDVKRequest request)
        {
            var result = new ServerAddIDVKResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        public DeletePhoneResponse DeletePhone(DeletePhoneRequest request)
        {
            var result = new ServerDeletePhoneResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        public GetPersonalCampaignsResponse GetPersonalCampaigns(GetPersonalCampaignsRequest request)
        {
            var result = new ServerGetPersonalCampaignsResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        public SelectPreferencesResponse SelectPreferences(SelectPreferencesRequest request)
        {
            var result = new ServerSelectPreferencesResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        public AddPhoneResponse AddPhone(AddPhoneRequest request)
        {
            var result = new ServerAddPhoneResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        public ClientPreferencesResponse ClientGetPreferences(ClientPreferencesRequest request)
        {
            var result = new ServerClientPreferencesResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        public ChequeDetailResponse GetChequeDetail(ChequeDetailRequest request)
        {
            var result = new ServerChequeDetailResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        public AddDeviceResponse SetClientDevice(AddDeviceRequest request)
        {
            var result = new ServerAddDeviceResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        public SendPushResponse SendPush(SendPushRequest request)
        {
            var result = new ServerSendPushResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        public SendPushResponse SendPushApple(SendPushRequest request)
        {
            var result = new ServerSendPushAppleResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        public CardStatisticsResponse CardStatistics(CardStatisticsRequest request)
        {
            var result = new ServerCardStatisticsResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        public GetClientInfoResponse ClientInfo(GetClientInfoRequest request)
        {
            var result = new ServerGetClientInfoResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        public CardAggregationResponse CardAggregation(CardAggregationRequest request)
        {
            var result = new ServerGetCardAggregation();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        public ClientCreateResponse ClientCreate(ClientCreateRequest request)
        {
            var result = new ServerClientCreate();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        public SetClientUpdateResponse ClientUpdate(SetClientUpdateRequest request)
        {
            var result = new ServerSetClientUpdate();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        public ClientPasswordChangeResponse ClientPasswordChange(ClientPasswordChangeRequest request)
        {
            var result = new ServerClientPasswordChange();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        public ClientUpdateCommunicationResponse ClientUpdateCommunication(ClientUpdateCommunicationRequest request)
        {
            var result = new ServerClientUpdateCommunication();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        public ClientBonusesResponse ClientBonuses(ClientBonusesRequest request)
        {
            var result = new ServerClientBonuses();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        public CardBonusesResponse CardBonuses(CardBonusesRequest request)
        {
            var result = new ServerCardBonuses();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        public ActivateCardResponse ActivateCard(ActivateCardRequest request)
        {
            var result = new ServerActivateCard();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }
    }
}
