﻿using LCManagerPartner.Models;
using Serilog;
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
    [RoutePrefix("api/values")]
    public class ValuesController : ApiController
    {
        static string connectionString = ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString;
        SqlConnection cnn = new SqlConnection(connectionString);

        //public Partner() { cnn.Open(); }
        //~Partner() { cnn.Close(); }

        [Authorize]
        [HttpPost]
        [Route("BalanceGet")]
        public BalanceGetResponse BalanceGet(BalanceGetRequest request)
        {
            Log.Information("LCManagerPartner BalanceGet {phone}", request.Phone);
            var result = new ServerBalanceGetResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("Redeem")]
        public RedeemResponse Redeem(RedeemRequest request)
        {
            Log.Information("LCManagerPartner Redeem {phone}", request.Phone);
            var result = new ServerRedeemResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("ChequeAdd")]
        public ChequeAddResponse ChequeAdd(ChequeAddRequest request)
        {
            Log.Information("LCManagerPartner ChequeAdd {phone}", request.Phone);
            var result = new ServerChequeAddResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }
        [HttpPost]
        [Route("GetAllShopsByPartner")]
        public GetPosesResponse GetAllShopsByPartner(GetPosesRequest request)
        {
            Log.Information("LCManagerPartner GetAllShopsByPartner {PartnerID}", request.PartnerID);
            var result = new ServerGetPosesResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("GetCheques")]
        public GetChequesResponse GetCheques(GetChequesRequest request)
        {
            Log.Information("LCManagerPartner GetCheques {Operator}", request.Operator);
            var result = new ServerGetChequesResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("GetConfirmCode")]
        public GetConfirmCodeResponse GetConfirmCode(GetConfirmCodeRequest request)
        {
            Log.Information("LCManagerPartner GetConfirmCode {phone}", request.Phone);
            var result = new ServerGetConfirmCodeResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("SetClientPassword")]
        public SetClientPasswordResponse SetClientPassword(SetClientPasswordRequest request)
        {
            Log.Information("LCManagerPartner SetClientPassword {phone}", request.Phone);
            var result = new ServerSetClientPasswordResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("GetRegistrationUser")]
        public GetRegistrationUserResponse GetRegistrationUser(GetRegistrationUserRequest request)
        {
            Log.Information("LCManagerPartner GetRegistrationUser {phone}", request.Phone);
            var result = new ServerGetRegistrationUserResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("GetSendVerificationCode")]
        public GetSendVerificationCodeResponse GetSendVerificationCode(GetSendVerificationCodeRequest request)
        {
            Log.Information("LCManagerPartner GetSendVerificationCode {phone}", request.Phone);
            var result = new ServerGetSendVerificationCodeResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("ClientLogin")]
        public ClientLoginResponse ClientLogin(ClientLoginRequest request)
        {
            Log.Information("LCManagerPartner ClientLogin {Login}", request.Login);
            var result = new ServerClientLoginResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("ChangeClient")]
        public ChangeClientResponse ChangeClient(ChangeClientRequest request)
        {
            Log.Information("LCManagerPartner ChangeClient {phone}", request.ClientData.phone);
            var result = new ServerChangeClientResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("SendEmailCode")]
        public SendEmailCodeResponse SendEmailCode(SendEmailCodeRequest request)
        {
            Log.Information("LCManagerPartner SendEmailCode {Email}", request.Email);
            var result = new ServerSendEmailCodeResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("ValidateEmail")]
        public ValidateEmailResponse ValidateEmail(ValidateEmailRequest request)
        {
            Log.Information("LCManagerPartner ValidateEmail {Email}", request.Email);
            var result = new ServerValidateEmailResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("AddPhone")]
        public AddPhoneResponse AddPhone(AddPhoneRequest request)
        {
            Log.Information("LCManagerPartner AddPhone {Phone}", request.Phone);
            var result = new ServerAddPhoneResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("DeletePhone")]
        public DeletePhoneResponse DeletePhone(DeletePhoneRequest request)
        {
            Log.Information("LCManagerPartner DeletePhone {Phone}", request.Phone);
            var result = new ServerDeletePhoneResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("PartnerFullInfo")]
        public PartnerFullInfoResponse PartnerFullInfo(PartnerFullInfoRequest request)
        {
            Log.Information("LCManagerPartner PartnerFullInfo {Partner}", request.Partner);
            var result = new ServerPartnerFullInfoResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("Refund")]
        public RefundResponse Refund(RefundRequest request)
        {
            Log.Information("LCManagerPartner Refund {Phone}", request.Phone);
            var result = new ServerRefundResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("OperatorStatistics")]
        public OperatorStatisticsResponse OperatorStatistics(OperatorStatisticsRequest request)
        {
            Log.Information("LCManagerPartner OperatorStatistics {Operator}", request.Operator);
            var result = new ServerOperatorStatisticsResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("GetCampaigns")]
        public GetCampaignsResponse GetCampaigns(GetCampaignsRequest request)
        {
            Log.Information("LCManagerPartner GetCampaigns {Operator}", request.Operator);
            var result = new ServerGetCampaignsResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("ClientInfo")]
        public GetClientInfoResponse ClientInfo(GetClientInfoRequest request)
        {
            Log.Information("LCManagerPartner ClientInfo {Phone}", request.Phone);
            var result = new ServerGetClientInfoResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("ClientUpdate")]
        public SetClientUpdateResponse ClientUpdate(SetClientUpdateRequest request)
        {
            Log.Information("LCManagerPartner ClientUpdate {Phone}", request.Phone);
            var result = new ServerSetClientUpdate();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("CancelLastCheque")]
        public CancelLastChequeResponse CancelLastCheque(CancelLastChequeRequest request)
        {
            Log.Information("LCManagerPartner CancelLastCheque {Phone}", request.Phone);
            var result = new ServerCancelLastCheque();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("ClientCreate")]
        public ClientCreateResponse ClientCreate(ClientCreateRequest request)
        {
            Log.Information("LCManagerPartner ClientCreate {Phone}", request.Phone);
            var result = new ServerClientCreate();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("GetClient")]
        public GetClientResponse GetClient(GetClientRequest request)
        {
            Log.Information("LCManagerPartner GetClient {ClientID}", request.ClientID);
            var result = new ServerGetClientResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("GetChequesByCard")]
        public GetChequesByCardResponse GetChequesByCard(GetChequesByCardRequest request)
        {
            Log.Information("LCManagerPartner GetChequesByCard {CardNumber}", request.CardNumber);
            var result = new ServerGetChequesByCardResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("ClientImport")]
        public ClientImportResponse ClientImport(ClientImportRequest request)
        {
            Log.Information("LCManagerPartner ClientImport {Operator}", request.Operator);
            var result = new ServerClientImportResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("Merge")]
        public MergeResponse Merge(MergeRequest request)
        {
            Log.Information("LCManagerPartner Merge {Active}", request.Active);
            var result = new ServerMergeResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("ClientInfoArray")]
        public GetClientInfoArrayResponse ClientInfoArray(GetClientInfoRequest request)
        {
            Log.Information("LCManagerPartner ClientInfoArray {Phone}", request.Phone);
            var result = new ServerGetClientInfoArrayResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("PosStatistics")]
        public PosStatisticsResponse PosStatistics(PosStatisticsRequest request)
        {
            Log.Information("LCManagerPartner PosStatistics {Pos}", request.Pos);
            var result = new ServerPosStatisticsResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("ChequeAggregation")]
        public ChequeAggregationResponse ChequeAggregation(ChequeAggregationRequest request)
        {
            Log.Information("LCManagerPartner ChequeAggregation {Operator}", request.Operator);
            var result = new ServerGetChequeAggregation();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("ClientAggregation")]
        public ClientAggregationResponse ClientAggregation(ClientAggregationRequest request)
        {
            Log.Information("LCManagerPartner ClientAggregation {Operator}", request.Operator);
            var result = new ServerClientAggregationResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }
        
        [HttpPost]
        [Route("BonusAdd")]
        public BonusAddResponse BonusAdd(BonusAddRequest request)
        {
            Log.Information("LCManagerPartner BonusAdd {Phone}", request.Phone);
            var result = new ServerBonusAdd();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("ChequesBonuses")]
        public GetChequesBonusesResponse ChequesBonuses(GetChequesBonusesRequest request)
        {
            Log.Information("LCManagerPartner ChequesBonuses {Card}", request.Card);
            var result = new ServerGetChequesBonusesResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("OperatorClients")]
        public OperatorClientResponse OperatorClients(OperatorClientRequest request)
        {
            Log.Information("LCManagerPartner OperatorClients {Operator}", request.Operator);
            var result = new ServerOperatorClient();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("BuysImport")]
        public BuysImportResponse BuysImport(BuysImportRequest request)
        {
            Log.Information("LCManagerPartner BuysImport {Operator}", request.Operator);
            var result = new ServerBuysImport();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("CardBonuses")]
        public CardBonusesResponse CardBonuses(CardBonusesRequest request)
        {
            Log.Information("LCManagerPartner CardBonuses {Card}", request.Card);
            var result = new ServerCardBonuses();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("FastBonus")]
        public FastBonusCreateResponse FastBonus(FastBonusCreateRequest request)
        {
            Log.Information("LCManagerPartner FastBonus {Operator}", request.Operator);
            var result = new ServerFastBonusCreateResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("ClientUpdateLevel")]
        public ClientUpdateLevelResponse ClientUpdateLevel(ClientUpdateLevelRequest request)
        {
            Log.Information("LCManagerPartner ClientUpdateLevel {Operator}", request.Operator);
            var result = new ServerClientUpdateLevel();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("ManagerLogin")]
        public ManagerLoginResponse ManagerLogin(ManagerLoginRequest request)
        {
            Log.Information("LCManagerPartner ManagerLogin {Phone}", request.Phone);
            var result = new ServerManagerLogin();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("ChequeMaxSumRedeem")]
        public ChequeMaxSumRedeemResponse ChequeMaxSumRedeem(ChequeMaxSumRedeemRequest request)
        {
            Log.Information("LCManagerPartner ChequeMaxSumRedeem {Phone}", request.Phone);
            var result = new ServerChequeMaxSumRedeem();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("OperatorClientsManager")]
        public OperatorClientsManagerResponse OperatorClientsManager(OperatorClientsManagerRequest request)
        {
            Log.Information("LCManagerPartner OperatorClients {Operator}", request.Operator);
            var result = new ServerOperatorClientsManager();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("SegmentationAge")]
        public SegmentationAgeResponse SegmentationAge(SegmentationAgeRequest request)
        {
            Log.Information("LCManagerPartner SegmentationAge {Operator}", request.Operator);
            var result = new ServerSegmentationAge();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("ClientBaseStructure")]
        public ClientBaseStructureResponse ClientBaseStructure(ClientBaseStructureRequest request)
        {
            Log.Information("LCManagerPartner ClientBaseStructure {Operator}", request.Operator);
            var result = new ServerClientBaseStructure();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("ClientBaseActive")]
        public ClientBaseActiveResponse ClientBaseActive(ClientBaseActiveRequest request)
        {
            Log.Information("LCManagerPartner ClientBaseActive {Operator}", request.Operator);
            var result = new ServerClientBaseActive();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("ClientAnalyticMoney")]
        public ClientAnalyticMoneyResponse ClientAnalyticMoney(ClientAnalyticMoneyRequest request)
        {
            Log.Information("LCManagerPartner ClientAnalyticMoney {Operator}", request.Operator);
            var result = new ServerClientAnalyticMoney();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("GainOperatorPeriod")]
        public GainOperatorPeriodResponse GainOperatorPeriod(GainOperatorPeriodRequest request)
        {
            Log.Information("LCManagerPartner GainOperatorPeriod {Operator}", request.Operator);
            var result = new ServerGainOperatorPeriod();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("RefundOperatorPeriod")]
        public RefundOperatorPeriodResponse RefundOperatorPeriod(RefundOperatorPeriodRequest request)
        {
            Log.Information("LCManagerPartner RefundOperatorPeriod {Operator}", request.Operator);
            var result = new ServerRefundOperatorPeriod();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("ClientOperatorPeriod")]
        public ClientOperatorPeriodResponse ClientOperatorPeriod(ClientOperatorPeriodRequest request)
        {
            Log.Information("LCManagerPartner ClientOperatorPeriod {Operator}", request.Operator);
            var result = new ServerClientOperatorPeriod();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("ManagerSendCode")]
        public ManagerSendCodeResponse ManagerSendCode(ManagerSendCodeRequest request)
        {
            Log.Information("LCManagerPartner ManagerSendCode {Phone}", request.Phone);
            var result = new ServerManagerSendCode();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("OperatorInfo")]
        public OperatorInfoResponse OperatorInfo(OperatorInfoRequest request)
        {
            Log.Information("LCManagerPartner OperatorInfo {Operator}", request.Operator);
            var result = new ServerOperatorInfo();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("ActivateCard")]
        public ActivateCardResponse ActivateCard(ActivateCardRequest request)
        {
            Log.Information("LCManagerPartner ActivateCard {Operator}", request.Operator);
            var result = new ServerActivateCard();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("OperatorGoods")]
        public OperatorGoodsResponse OperatorGoods(OperatorGoodsRequest request)
        {
            Log.Information("LCManagerPartner OperatorGoods {Operator}", request.Operator);
            var result = new ServerOperatorGoods();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("OperatorPos")]
        public OperatorPosResponse OperatorPos(OperatorPosRequest request)
        {
            var result = new ServerOperatorPos();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }
    }
}
