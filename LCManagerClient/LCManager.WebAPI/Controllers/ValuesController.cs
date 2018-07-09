using LCManager.Infrastructure.Request;
using LCManager.Infrastructure.Response;

namespace LCManagerPartner.Controllers
{
    using Implementation.Request;
    using Implementation.Response;
    using Implementation.Services;
    using Models;
    using Serilog;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Web.Http;

    [Authorize]
    [RoutePrefix("api/values")]
    public class ValuesController : ApiController
    {
        static string connectionString = ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString;
        SqlConnection cnn = new SqlConnection(connectionString);

        private readonly GoodService _operatorGoodService;
        private readonly BookkeepingService _bookkeepingService;

        /// <summary>
        /// 
        /// </summary>
        public ValuesController()
        {
            _operatorGoodService = new GoodService();
        }


        /// <summary>
        /// Получение баланса бонусных баллов по номеру карты, либо по номеру телефонаУчастника программы лояльности.
        /// </summary>
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
        /// <summary>
        /// Отправка запроса на списание бонусных баллов Участника программы лояльности.
        /// </summary>
        [HttpPost]
        [Route("Redeem")]
        public RedeemResponse Redeem(RedeemRequest request)
        {
            Log.Information("LCManagerPartner Redeem {phone}", request.Phone);
            var result = new ServerRedeemResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }
        /// <summary>
        /// Регистрация чека покупки на Процессинге
        /// </summary>
        [HttpPost]
        [Route("ChequeAdd")]
        public ChequeAddResponse ChequeAdd(ChequeAddRequest request)
        {
            Log.Information("LCManagerPartner ChequeAdd Operator = {Operator}, Phone = {Phone}, Amount = {Amount}, BonusId = {BonusId}, Card = {Card}, ChequeTime = {ChequeTime}, " +
                                "ItemData = {ItemData}, NoAdd = {NoAdd}, NoRedeem = {NoRedeem}, NoWrite = {NoWrite}, Number = {Number}, PaidByBonus = {PaidByBonus}, Partner = {Partner}, " +
                                "POS = {POS}, Redeemed = {Redeemed}", 
                                request.Operator, request.Phone, request.Amount, request.BonusId, request.Card, request.ChequeTime, request.ItemData, request.NoAdd, request.NoRedeem,
                                request.NoWrite, request.Number, request.PaidByBonus, request.Partner, request.POS, request.Redeemed);
            var result = new ServerChequeAddResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Получение списка магазинов Партнёра
        /// </summary>
        [HttpPost]
        [Route("GetAllShopsByPartner")]
        public GetPosesResponse GetAllShopsByPartner(GetPosesRequest request)
        {
            Log.Information("LCManagerPartner GetAllShopsByPartner {PartnerID}", request.PartnerID);
            var result = new ServerGetPosesResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Получение чеков по заданному номеру карты.
        /// </summary>
        [HttpPost]
        [Route("GetCheques")]
        public GetChequesResponse GetCheques(GetChequesRequest request)
        {
            Log.Information("LCManagerPartner GetCheques {Operator} and {Partner} and {Pos}", request.Operator, request.PartnerId, request.Pos);
            var result = new ServerGetChequesResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Проверка кода и валидация номера телефона.
        /// </summary>
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

        /// <summary>
        /// Установка/изменение пароля Участника.
        /// </summary>
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

        /// <summary>
        /// Регистрация Участника в ЛКУ программы лояльности
        /// </summary>
        [HttpPost]
        [Route("GetRegistrationUser")]
        public GetRegistrationUserResponse GetRegistrationUser(GetRegistrationUserRequest request)
        {
            Log.Information("LCManagerPartner GetRegistrationUser Operator = {Operator}, Phone = {phone}, PosCode = {PosCode}, AgreePersonalData = {AgreePersonalData}, Card = {Card}" +
                            "ClientSetPassword = {ClientSetPassword}, Email = {Email}, FriendPhone = {FriendPhone}, PartnerID = {PartnerID}, Promocode = {Promocode}", 
                                                            request.Operator, request.Phone, request.PosCode, request.AgreePersonalData, request.Card, request.ClientSetPassword, 
                                                            request.Email, request.FriendPhone, request.PartnerID, request.Promocode);
            var result = new ServerGetRegistrationUserResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Отправка проверечного кода в SMS сообщении на указанный номер телефона
        /// </summary>
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

        /// <summary>
        /// Авторизация Участника в ЛКУ по логину/паролю или через привязанный аккаунт социальной сети.
        /// </summary>
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

        /// <summary>
        /// Изменение данных Участника по заданному идентификатору
        /// </summary>
        [HttpPost]
        [Route("ChangeClient")]
        public ChangeClientResponse ChangeClient(ChangeClientRequest request)
        {
            Log.Information("LCManagerPartner ChangeClient Operator = {Operator}, phone = {phone}, address = {address}", 
                            request.Operator, request.ClientData.phone, request.ClientData.address);
            var result = new ServerChangeClientResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Отправка проверечного кода на указанный адрес электронной почты.
        /// </summary>
        [HttpPost]
        [Route("SendEmailCode")]
        public SendEmailCodeResponse SendEmailCode(SendEmailCodeRequest request)
        {
            Log.Information("LCManagerPartner SendEmailCode {Email}", request.Email);
            var result = new ServerSendEmailCodeResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Проверка кода и валидация электронной почты
        /// </summary>
        [HttpPost]
        [Route("ValidateEmail")]
        public ValidateEmailResponse ValidateEmail(ValidateEmailRequest request)
        {
            Log.Information("LCManagerPartner ValidateEmail {Email}", request.Email);
            var result = new ServerValidateEmailResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Привязывает номер телефона к заданному Участнику, устанавливает заданный номер телефона как коммуникационный.
        /// </summary>
        [HttpPost]
        [Route("AddPhone")]
        public AddPhoneResponse AddPhone(AddPhoneRequest request)
        {
            Log.Information("LCManagerPartner AddPhone {Phone}", request.Phone);
            var result = new ServerAddPhoneResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Удаляет заданный номер телефона.
        /// </summary>
        [HttpPost]
        [Route("DeletePhone")]
        public DeletePhoneResponse DeletePhone(DeletePhoneRequest request)
        {
            Log.Information("LCManagerPartner DeletePhone {Phone}", request.Phone);
            var result = new ServerDeletePhoneResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }
        /// <summary>
        /// Получение полной информации по Партнёру за период
        /// </summary>
        [HttpPost]
        [Route("PartnerFullInfo")]
        public PartnerFullInfoResponse PartnerFullInfo(PartnerFullInfoRequest request)
        {
            Log.Information("LCManagerPartner PartnerFullInfo {Partner}", request.Partner);
            var result = new ServerPartnerFullInfoResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Регистрация чека возврата на Процессинге.
        /// </summary>
        [HttpPost]
        [Route("Refund")]
        public RefundResponse Refund(RefundRequest request)
        {
            //Log.Information("LCManagerPartner Refund {Phone}", request.Phone);
            Log.Information("LCManagerPartner GetRegistrationUser Operator = {Operator}, Phone = {phone}, Amount = {Amount}, Card = {Card}, ChequeTime = {ChequeTime}, " +
                                        "Number = {Number}, PaidByBonus = {PaidByBonus}, Partner = {Partner}, Pos = {Pos}, PurchaseDate = {PurchaseDate}, PurchaseId = {PurchaseId}, " +
                                        "PurchaseNumber = {PurchaseNumber}, PurchasePos = {PurchasePos}, PurchaseTerminal = {PurchaseTerminal}",
                                                            request.Operator, request.Phone, request.Amount, request.Card, request.ChequeTime,
                                                            request.Number, request.PaidByBonus, request.Partner, request.Pos, request.PurchaseDate, request.PurchaseId, request.PurchaseNumber,
                                                            request.PurchasePos, request.PurchaseTerminal);
            var result = new ServerRefundResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Получение полной информации по Оператору за период
        /// </summary>
        [HttpPost]
        [Route("OperatorStatistics")]
        public OperatorStatisticsResponse OperatorStatistics(OperatorStatisticsRequest request)
        {
            Log.Information("LCManagerPartner OperatorStatistics {Operator}", request.Operator);
            var result = new ServerOperatorStatisticsResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Получение Акций по заданному идентификатору Участника программы лояльности.
        /// </summary>
        [HttpPost]
        [Route("GetCampaigns")]
        public GetCampaignsResponse GetCampaigns(GetCampaignsRequest request)
        {
            Log.Information("LCManagerPartner GetCampaigns {Operator}", request.Operator);
            var result = new ServerGetCampaignsResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Получение данных Участника по заданному номеру телефона или номеру карты.
        /// </summary>
        [HttpPost]
        [Route("ClientInfo")]
        public GetClientInfoResponse ClientInfo(GetClientInfoRequest request)
        {
            Log.Information("LCManagerPartner ClientInfo {Phone}", request.Phone);
            var result = new ServerGetClientInfoResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Обновление профиля Участника программы лояльности.
        /// </summary>
        [HttpPost]
        [Route("ClientUpdate")]
        public SetClientUpdateResponse ClientUpdate(SetClientUpdateRequest request)
        {
            Log.Information("LCManagerPartner ClientUpdate {Phone}", request.Phone);
            var result = new ServerSetClientUpdate();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Отмена последнего зарегистрированного чека на Процессинге.
        /// </summary>
        [HttpPost]
        [Route("CancelLastCheque")]
        public CancelLastChequeResponse CancelLastCheque(CancelLastChequeRequest request)
        {
            Log.Information("LCManagerPartner CancelLastCheque {Phone}", request.Phone);
            var result = new ServerCancelLastCheque();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Создание профиля Участника программы лояльности
        /// </summary>
        [HttpPost]
        [Route("ClientCreate")]
        public ClientCreateResponse ClientCreate(ClientCreateRequest request)
        {
            Log.Information("LCManagerPartner ClientCreate {Phone}", request.Phone);
            var result = new ServerClientCreate();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Получение информации о картах Участника.
        /// </summary>
        [HttpPost]
        [Route("GetClient")]
        public GetClientResponse GetClient(GetClientRequest request)
        {
            Log.Information("LCManagerPartner GetClient {ClientID}", request.ClientID);
            var result = new ServerGetClientResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Получение информации о чеках привязанных к карте
        /// </summary>
        [HttpPost]
        [Route("GetChequesByCard")]
        public GetChequesByCardResponse GetChequesByCard(GetChequesByCardRequest request)
        {
            Log.Information("LCManagerPartner GetChequesByCard {CardNumber}", request.CardNumber);
            var result = new ServerGetChequesByCardResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Импорт клиентской базы
        /// </summary>
        [HttpPost]
        [Route("ClientImport")]
        public ClientImportResponse ClientImport(ClientImportRequest request)
        {
            Log.Information("LCManagerPartner ClientImport {Operator}", request.Operator);
            var result = new ServerClientImportResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Замена карты клиента
        /// </summary>
        [HttpPost]
        [Route("Merge")]
        public MergeResponse Merge(MergeRequest request)
        {
            Log.Information("LCManagerPartner Merge {Active}", request.Active);
            var result = new ServerMergeResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Запрос информации о клиенте
        /// </summary>
        [HttpPost]
        [Route("ClientInfoArray")]
        public GetClientInfoArrayResponse ClientInfoArray(GetClientInfoRequest request)
        {
            Log.Information("LCManagerPartner ClientInfoArray {Phone}", request.Phone);
            var result = new ServerGetClientInfoArrayResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Статистика по торговой точке за период
        /// </summary>
        [HttpPost]
        [Route("PosStatistics")]
        public PosStatisticsResponse PosStatistics(PosStatisticsRequest request)
        {
            Log.Information("LCManagerPartner PosStatistics {Pos}", request.Pos);
            var result = new ServerPosStatisticsResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Статистика по чекам за период
        /// </summary>
        [HttpPost]
        [Route("ChequeAggregation")]
        public ChequeAggregationResponse ChequeAggregation(ChequeAggregationRequest request)
        {
            Log.Information("LCManagerPartner ChequeAggregation {Operator}", request.Operator);
            var result = new ServerGetChequeAggregation();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Статистика по клиентам за период
        /// </summary>
        [HttpPost]
        [Route("ClientAggregation")]
        public ClientAggregationResponse ClientAggregation(ClientAggregationRequest request)
        {
            Log.Information("LCManagerPartner ClientAggregation {Operator}", request.Operator);
            var result = new ServerClientAggregationResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Ручное начисление бонусов
        /// </summary>
        [HttpPost]
        [Route("BonusAdd")]
        public BonusAddResponse BonusAdd(BonusAddRequest request)
        {
            Log.Information("LCManagerPartner BonusAdd {Phone}", request.Phone);
            var result = new ServerBonusAdd();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Статистика по начислению бонусов
        /// </summary>
        [HttpPost]
        [Route("ChequesBonuses")]
        public GetChequesBonusesResponse ChequesBonuses(GetChequesBonusesRequest request)
        {
            Log.Information("LCManagerPartner ChequesBonuses {Card}", request.Card);
            var result = new ServerGetChequesBonusesResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Статистика по клиентам Оператора
        /// </summary>
        [HttpPost]
        [Route("OperatorClients")]
        public OperatorClientResponse OperatorClients(OperatorClientRequest request)
        {
            Log.Information("LCManagerPartner OperatorClients {Operator}", request.Operator);
            var result = new ServerOperatorClient();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Импорт базы проведённых чеков
        /// </summary>
        [HttpPost]
        [Route("BuysImport")]
        public BuysImportResponse BuysImport(BuysImportRequest request)
        {
            Log.Information("LCManagerPartner BuysImport {Operator}", request.Operator);
            var result = new ServerBuysImport();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Получение бонусов карты не относящиеся к чекам.
        /// </summary>
        [HttpPost]
        [Route("CardBonuses")]
        public CardBonusesResponse CardBonuses(CardBonusesRequest request)
        {
            Log.Information("LCManagerPartner CardBonuses {Card}", request.Card);
            var result = new ServerCardBonuses();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Создание "быстрых бонусов"
        /// </summary>
        [HttpPost]
        [Route("FastBonus")]
        public FastBonusCreateResponse FastBonus(FastBonusCreateRequest request)
        {
            Log.Information("LCManagerPartner FastBonus {Operator}", request.Operator);
            var result = new ServerFastBonusCreateResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Импорт базы уровней клиентов
        /// </summary>
        [HttpPost]
        [Route("ClientUpdateLevel")]
        public ClientUpdateLevelResponse ClientUpdateLevel(ClientUpdateLevelRequest request)
        {
            Log.Information("LCManagerPartner ClientUpdateLevel {Operator}", request.Operator);
            var result = new ServerClientUpdateLevel();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Авторизация на WebApi
        /// </summary>
        [HttpPost]
        [Route("ManagerLogin")]
        public ManagerLoginResponse ManagerLogin(ManagerLoginRequest request)
        {
            Log.Information("LCManagerPartner ManagerLogin {Login}", request.Login);
            var result = new ServerManagerLogin();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Возвращает максимальную сумму списания в чек
        /// </summary>
        [HttpPost]
        [Route("ChequeMaxSumRedeem")]
        public ChequeMaxSumRedeemResponse ChequeMaxSumRedeem(ChequeMaxSumRedeemRequest request)
        {
            Log.Information("LCManagerPartner ChequeMaxSumRedeem {Card}", request.Card);
            var result = new ServerChequeMaxSumRedeem();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Возращает информацию о клиентах на страницу клиентов Оператора
        /// </summary>
        [HttpPost]
        [Route("OperatorClientsManager")]
        public OperatorClientsManagerResponse OperatorClientsManager(OperatorClientsManagerRequest request)
        {
            Log.Information("LCManagerPartner OperatorClients {Operator}", request.Operator);
            var result = new ServerOperatorClientsManager();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Данные для страницы Аналитика LCManager
        /// </summary>
        [HttpPost]
        [Route("SegmentationAge")]
        public SegmentationAgeResponse SegmentationAge(SegmentationAgeRequest request)
        {
            Log.Information("LCManagerPartner SegmentationAge {Operator}", request.Operator);
            var result = new ServerSegmentationAge();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Данные для страницы Аналитика LCManager, блок БОНУСЫ
        /// </summary>
        [HttpPost]
        [Route("GetClientBonuses")]
        public BonusesResponse GetClientBonuses(BonusesRequest request)
        {
            Log.Information("LCManagerPartner SegmentationAge {Operator}", request.Operator);
            var result = new ServerBonuses();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Данные для страницы Аналитика LCManager
        /// </summary>
        [HttpPost]
        [Route("ClientBaseStructure")]
        public ClientBaseStructureResponse ClientBaseStructure(ClientBaseStructureRequest request)
        {
            Log.Information("LCManagerPartner ClientBaseStructure {Operator}", request.Operator);
            var result = new ServerClientBaseStructure();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Данные для страницы Аналитика LCManager
        /// </summary>
        [HttpPost]
        [Route("ClientBaseActive")]
        public ClientBaseActiveResponse ClientBaseActive(ClientBaseActiveRequest request)
        {
            Log.Information("LCManagerPartner ClientBaseActive {Operator}", request.Operator);
            var result = new ServerClientBaseActive();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Данные для страницы Аналитика LCManager
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("ClientAnalyticMoney")]
        public ClientAnalyticMoneyResponse ClientAnalyticMoney(ClientAnalyticMoneyRequest request)
        {
            Log.Information("LCManagerPartner ClientAnalyticMoney {Operator}", request.Operator);
            var result = new ServerClientAnalyticMoney();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Возращает средний чек и выручку Оператора\Партнёра\ТТ по месяцам
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GainOperatorPeriod")]
        public GainOperatorPeriodResponse GainOperatorPeriod(GainOperatorPeriodRequest request)
        {
            Log.Information("LCManagerPartner GainOperatorPeriod {Operator}", request.Operator);
            var result = new ServerGainOperatorPeriod();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Возращает сумму возратов Оператора\Партнёра\ТТ по месяцам
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("RefundOperatorPeriod")]
        public RefundOperatorPeriodResponse RefundOperatorPeriod(RefundOperatorPeriodRequest request)
        {
            Log.Information("LCManagerPartner RefundOperatorPeriod {Operator}", request.Operator);
            var result = new ServerRefundOperatorPeriod();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Возвращает количество зарегестрированных клиентов Оператора\Партнёра\ТТ по месяцам
        /// </summary>
        [HttpPost]
        [Route("ClientOperatorPeriod")]
        public ClientOperatorPeriodResponse ClientOperatorPeriod(ClientOperatorPeriodRequest request)
        {
            Log.Information("LCManagerPartner ClientOperatorPeriod {Operator}", request.Operator);
            var result = new ServerClientOperatorPeriod();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Отправка кода подтверждения менеджером
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [Route("ManagerSendCode")]
        public ManagerSendCodeResponse ManagerSendCode(ManagerSendCodeRequest request)
        {
            Log.Information("LCManagerPartner ManagerSendCode {Phone}", request.Login);
            var result = new ServerManagerSendCode();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Установка/изменение пароля Участника.
        /// </summary>
        [AllowAnonymous]
        [HttpPost]
        [Route("SetManagerPassword")]
        public SetManagerPasswordResponse SetManagerPassword(SetManagerPasswordRequest request)
        {
            Log.Information("LCManagerPartner SetClientPassword {phone}", request.Phone);
            var result = new ServerSetManagerPasswordResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Информация об операторе
        /// </summary>
        [HttpPost]
        [Route("OperatorInfo")]
        public OperatorInfoResponse OperatorInfo(OperatorInfoRequest request)
        {
            Log.Information("LCManagerPartner OperatorInfo {Operator}", request.Operator);
            var result = new ServerOperatorInfo();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Активация карты на списание
        /// </summary>
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

        #region Товары и списки товаров

        /// <summary>
        /// Выборка товаров Оператора
        /// </summary>
        [HttpPost]
        [Route("ImportOperatorGoods")]
        public OperatorGoodImportResponse OperatorGoodImport(OperatorGoodImportRequest request)
        {
            return _operatorGoodService.ImportGoodsFromExcel(request);
        }

        /// <summary>
        /// Выборка товаров Оператора
        /// </summary>
        [HttpPost]
        [Route("OperatorGoods")]
        public OperatorGoodsResponse OperatorGoods(OperatorGoodsRequest request)
        {
            Log.Information("LCManagerPartner OperatorGoods {Operator}", request.Operator);
            var result = new ServerOperatorGoods();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Сохранение списка товаров
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveOperatorGoodList")]
        public OperatorGoodListResponse OperatorGoodListSave(OperatorGoodListCreateRequest request)
        {
            return _operatorGoodService.SaveOperatorGoodList(request);
        }

        /// <summary>
        /// Получает списки товаров, отдельного оператора
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("OperatorGoodList")]
        public OperatorGoodListResponse GetGoodListByOperator(OperatorGoodsRequest request)
        {
            return _operatorGoodService.GetGoodListByOperator(request);
        }

        /// <summary>
        /// Удалени списка товаров
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("RemoveOperatorGoodList")]
        public DefaultResponse RemoveOperatorPos(OperatorGoodRemoveRequest request)
        {
            return _operatorGoodService.RemoveOperatorGoodList(request);
        }

        #endregion

        /// <summary>
        /// Проверка промокода
        /// </summary>
        [HttpPost]
        [Route("VerificationPromocode")]
        public VerificationPromocodeResponse VerificationPromocode(VerificationPromocodeRequest request)
        {
            var result = new ServerVerificationPromocode();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Получить список ролей по идентификатору роли
        /// </summary>
        //[HttpPost]
        //[Route("GetRoles")]
        //public GetRolesResponse GetRoles(GetRolesRequest request)
        //{
        //    Log.Information("LCManagerPartner GetRoles {Login}", request.Login);
        //    var result = new ServerGetRoles();
        //    var returnValue = result.ProcessRequest(cnn, request);
        //    return returnValue;
        //}


        /// <summary>
        /// Получает сверку по оператору, партнеру или торговой точке
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetBookkeepings")]
        public BookkeepingsResponse GetBookkeepings(BookkeepingRequest request)
        {
            return _bookkeepingService.GetAllBookkeeping(request);
        }
    }
}
