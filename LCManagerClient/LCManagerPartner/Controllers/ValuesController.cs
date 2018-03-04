using LCManagerPartner.Models;
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


        /// <summary>
        /// Получение баланса бонусных баллов по номеру карты, либо по номеру телефонаУчастника программы лояльности.
        /// </summary>

        /// <returns></returns>
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
        /// <param name="Card">номер карты</param>
        /// <param name="ChequeTime">дата и время чека</param>
        /// <param name="Phone">номер телефона</param>
        /// <param name="Partner">идентификатор Партнера программы лояльности</param>
        /// <param name="POS">код Торговой точки Партнера</param>
        /// <param name="Amount">сумма чека</param>
        /// <param name="PaidByBonus">оплачено бонусными баллами</param>
        /// <param name="Number">номер чека</param>
        /// <param name="Operator">идентификатор Оператора программы лояльности</param>
        /// <param name="ItemData">список позиций чека</param>
        /// <param name="Position">номер позиции</param>
        /// <param name="Code">код позиции</param>
        /// <param name="Price">цена</param>
        /// <param name="Quantity">количество</param>
        /// <param name="Amount">итоговая сумма</param>
        /// <param name="PaidByBonus">оплачено бонусными баллами</param>
        /// <returns></returns>
        [HttpPost]
        [Route("ChequeAdd")]
        public ChequeAddResponse ChequeAdd(ChequeAddRequest request)
        {
            Log.Information("LCManagerPartner ChequeAdd {phone}", request.Phone);
            var result = new ServerChequeAddResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Получение списка магазинов Партнёра
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
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
        /// <param name="CardNumber">номер карты Участника программы лояльности</param>
        /// <param name="ClientId">идентификатор Участника программы лояльности</param>
        /// <param name="Operator">идентификатор Оператора программы лояльности</param>  
        /// <returns></returns>
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
        /// <param name="Phone">номер телефона</param>
        /// <param name="Code">полученный в SMS сообщении проверочный код</param>
        /// <returns></returns>
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
        /// <param name="Phone">номер телефона</param>
        /// <param name="Code">проверочный код</param>
        /// <param name="Password">новый пароль</param>
        /// <param name="ClientID">идентификатор Участника</param>
        /// <returns></returns>
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
        /// <param name="Phone">номер телефона</param>
        /// <param name="Card">номер карты</param>
        /// <param name="PartnerID">идентификатор Партнера программы лояльности</param>
        /// <param name="PosCode">код Торговой точки в которой производится регистрация</param>
        /// <param name="AgreePersonalData">согласие на обработку персональных данных</param>
        /// <param name="Operator">идентификатор Оператора програмы лояльности</param>
        /// <param name="FriendPhone">телефон друга/подруги для механики “Приведи друга”</param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetRegistrationUser")]
        public GetRegistrationUserResponse GetRegistrationUser(GetRegistrationUserRequest request)
        {
            Log.Information("LCManagerPartner GetRegistrationUser {phone}", request.Phone);
            var result = new ServerGetRegistrationUserResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Отправка проверечного кода в SMS сообщении на указанный номер телефона
        /// </summary>
        /// <param name="Phone">номер телефона</param>
        /// <param name="Partner">идентификатор Партнера программы лояльности</param>
        /// <param name="Operator">идентификатор Оператора программы лояльности</param>
        /// <returns></returns>
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
        /// <param name="Login">номер телефона или номер карты</param>
        /// <param name="Password">пароль</param>
        /// <param name="IdFB">токен (временный код) Участника в социальной сети “Facebook”</param>
        /// <param name="IdOK">токен (временный код) Участника в “Одноклассники”</param>
        /// <param name="IdVK">токен (временный код) Участника в “ВКонтакте”</param>
        /// <param name="Operator">идентификатор Оператора Программы лояльности</param>
        /// <returns></returns>
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
        /// <param name="id">идентификатор Участника программы лояльности</param>
        /// <param name="password">пароль</param>
        /// <param name="firstname">имя</param>
        /// <param name="middlename">отчетсво</param>
        /// <param name="lastname">фамилия</param>
        /// <param name="gender">пол (1 - муж., -1 - жен., 0 - не определен)</param>
        /// <param name="birthdate">дата рождения</param>
        /// <param name="address">адрес</param>
        /// <param name="haschildren">признак наличия детей</param>
        /// <param name="description">произвольное описание</param>
        /// <param name="phone">номер телефона</param>
        /// <param name="email">электронная почта</param>
        /// <param name="allowsms">получать уведомления по SMS</param>
        /// <param name="allowemail">получать уведомления по E-mail</param>
        /// <param name="balance">активный баланс бонусных баллов</param>
        /// <param name="allowpush">получать push-уведомления</param>
        /// <param name="lasturchaseamount">сумма последней покупки</param>
        /// <param name="lastpurchasedate">дата последней покупки</param>
        /// <param name="Operator">идентификатор Оператора программы лояльности</param>
        /// <returns></returns>
        [HttpPost]
        [Route("ChangeClient")]
        public ChangeClientResponse ChangeClient(ChangeClientRequest request)
        {
            Log.Information("LCManagerPartner ChangeClient {phone}", request.ClientData.phone);
            var result = new ServerChangeClientResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Отправка проверечного кода на указанный адрес электронной почты.
        /// </summary>
        /// <param name="Email">электронная почта</param>
        /// <returns></returns>
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
        /// <param name="Email">электронная почта</param>
        /// <param name="Client">идентификатор Участника при валидации электронной почты по ссылке</param>
        /// <param name="Code"> полученный в сообщении проверочный код</param>
        /// <returns></returns>
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
        /// <param name="ClientID">идентификатор Участника</param>
        /// <param name="Phone">номер телефона</param>
        /// <returns></returns>
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
        /// <param name="Phone">номер телефона</param>
        /// <returns></returns>
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
            Log.Information("LCManagerPartner Refund {Phone}", request.Phone);
            var result = new ServerRefundResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Получение полной информации по Оператору за период
        /// </summary>
        /// <param name="Operator">оператор</param>
        /// <param name="Start_date">начало периода</param>
        /// <param name="End_date">конец приода</param>
        /// <returns></returns>
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
        /// <param name="ClientID">идентификатор Участника программы лояльности</param>
        /// <param name="Operator">идентификатор Оператора программы лояльности</param>
        /// <returns></returns>
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
        /// <param name="Operator">идентификатор Оператора программы лояльности</param>
        /// <param name="Card">номер карты</param>
        /// <param name="Phone">номер телефона</param>
        /// <returns></returns>
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
        /// <param name="Client">идентификатор Участника программы лояльности</param>
        /// <param name="Name">имя</param>
        /// <param name="Surname">фамилия</param>
        /// <param name="Patronymic">отчество</param>
        /// <param name="AllowSms">получать уведомления по SMS</param>
        /// <param name="AllowEmail">получать уведомления по E-mail</param>
        /// <param name="Birthdate">дата рождения</param>
        /// <param name="Phone">номер телефона</param>
        /// <param name="Email">электронная почта</param>
        /// <param name="Operator">идентификатор Оператора программы лояльности</param>
        /// <param name="Gender">пол (1 - муж., -1 - жен., 0 - не определен)</param>
        /// <returns></returns>
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
        /// <param name="Partner">идентификатор Партнера программы лояльности</param>
        /// <param name="Card">номер карты</param>
        /// <param name="ChequeTime">дата чека</param>
        /// <param name="Phone">номер телефона</param>
        /// <param name="Pos">код Торговой точки Партнера</param>
        /// <param name="Number">номер чека</param>
        /// <param name="Terminal">код терминала Торговой точки</param>
        /// <param name="Operator">идентификатор Оператора программы лояльности</param>
        /// <returns></returns>
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
        /// <param name="Operator">идентификатор Оператора программы лояльности</param>
        /// <param name="Operator">идентификатор Оператора программы лояльности</param>
        /// <param name="Card">номер карты</param>
        /// <param name="Phone">номер телефона</param>
        /// <param name="Name">имя</param>
        /// <param name="Surname">фамилия</param>
        /// <param name="Patronymic">отчество</param>
        /// <param name="Email">адрес электронной почты?</param>
        /// <param name="Birthdate">идентификатор Участника программы лояльности</param>
        /// <param name="AllowSms">получать уведомления по SMS</param>
        /// <param name="AllowEmail">получать уведомления по E-mail</param>
        /// <param name="Gender">пол (1 - муж., -1 - жен., 0 - не определен)</param>
        /// <param name="AgreePersonalData">согласие на обработку персональных данных</param>
        /// <param name="PosCode">код Торговой точки в которой производится создание</param>
        /// <returns></returns>
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
        /// <param name="ClientID">идентификатор Участника программы лояльности</param>
        /// <param name="Operator">идентификатор Оператора программы лояльности</param>
        /// <returns></returns>
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
        /// <param name="CardNumber">номер карты</param>
        /// <param name="Pos">код Торговой точки Партнера</param>
        /// <returns></returns>
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
        /// <param name="ExcelFile">файл базы</param>
        /// <param name="Partner">идентификатор Партнера программы лояльност</param>
        /// <param name="Operator">идентификатор Оператора программы лояльности</param>
        /// <returns></returns>
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
        /// <param name="Active">Номер актуальной карты</param>
        /// <param name="Merged">Номер устаревшей карты</param>
        /// <returns></returns>
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
        /// <param name="Operator">идентификатор Оператора программы лояльности</param>
        /// <param name="Card">номер карты</param>
        /// <param name="Phone">номер телефона</param>
        /// <returns></returns>
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
        /// <param name="Pos">код Торговой точки Партнера</param>
        /// <param name="Start_date">начало периода</param>
        /// <param name="End_date">конец периода</param>
        /// <returns></returns>
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
        /// <param name="Operator">идентификатор Оператора программы лояльности</param>
        /// <param name="Partner">идентификатор Партнера программы лояльност</param>
        /// <param name="Pos">код Торговой точки Партнера</param>
        /// <param name="From">начало периода</param>
        /// <param name="To">конец периода</param>
        /// <param name="Layout">форма агрегирования</param>
        /// <returns></returns>
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
        /// <param name="Operator">идентификатор Оператора программы лояльности</param>
        /// <param name="Partner">идентификатор Партнера программы лояльност</param>
        /// <param name="Pos">код Торговой точки Партнера</param>
        /// <param name="From">начало периода</param>
        /// <param name="To">конец периода</param>
        /// <param name="Layout">форма агрегирования</param>
        /// <returns></returns>
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
        /// <param name="Operator">идентификатор Оператора программы лояльности</param>
        /// <param name="Card">номер карты</param>
        /// <param name="Phone">номер телефона</param>
        /// <param name="Bonus">размер бонуса</param>
        /// <returns></returns>
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
        /// <param name="Card">номер карты</param>
        /// <param name="Client">клиент</param>
        /// <param name="Operator">идентификатор Оператора программы лояльности</param>
        /// <param name="Partner">идентификатор Партнера программы лояльност</param>
        /// <param name="Pos">код Торговой точки Партнера</param>
        /// <param name="Page">страница</param>
        /// <param name="PageSize">размер страницы</param>
        /// <returns></returns>
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
        /// <param name="Operator">идентификатор Оператора программы лояльности</param>
        /// <returns></returns>
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
        /// <param name="ExcelFile">файл базы</param>
        /// <param name="Partner">идентификатор Партнера программы лояльност</param>
        /// <param name="Operator">идентификатор Оператора программы лояльности</param>
        /// <returns></returns>
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
        /// <param name="Operator">идентификатор Оператора программы лояльности</param>
        /// <param name="Card">номер карты</param>
        /// <returns></returns>
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
        /// <param name="ExcelFile">файл базы</param>
        /// <param name="FastBonusName">именование "быстрых бонусов"</param>
        /// <param name="Partner">идентификатор Партнера программы лояльност</param>
        /// <param name="Operator">идентификатор Оператора программы лояльности</param>
        /// <returns></returns>
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
        /// <param name="ExcelFile">файл базы</param>
        /// <param name="Partner">идентификатор Партнера программы лояльност</param>
        /// <param name="Operator">идентификатор Оператора программы лояльности</param>
        /// <param name="Level">уровень клиента</param>
        /// <returns></returns>
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
        /// <param name="Phone">номер телефона</param>
        /// <param name="Password">пароль</param>
        /// <returns></returns>
        [HttpPost]
        [Route("ManagerLogin")]
        public ManagerLoginResponse ManagerLogin(ManagerLoginRequest request)
        {
            Log.Information("LCManagerPartner ManagerLogin {Phone}", request.Phone);
            var result = new ServerManagerLogin();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Возвращает максимальную сумму списания в чек
        /// </summary>
        /// <param name="Card">номер карты</param>
        /// <param name="Partner">идентификатор Партнера программы лояльност</param>
        /// <param name="ChequeSum">сумма по чеку</param>
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
        /// <param name="Operator">идентификатор Оператора программы лояльности</param>
        /// <param name="Partner">идентификатор Партнера программы лояльност</param>
        /// <param name="Pos">код Торговой точки Партнера</param>
        /// <param name="From">конец периода</param>
        /// <param name="To">начало периода</param>
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
        /// <param name="Operator">идентификатор Оператора программы лояльности</param>
        /// <param name="Partner">идентификатор Партнера программы лояльност</param>
        /// <param name="Pos">код Торговой точки Партнера</param>
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
        /// Данные для страницы Аналитика LCManager
        /// </summary>
        /// <param name="Operator">идентификатор Оператора программы лояльности</param>
        /// <param name="Partner">идентификатор Партнера программы лояльност</param>
        /// <param name="Pos">код Торговой точки Партнера</param>
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
        /// <param name="Operator">идентификатор Оператора программы лояльности</param>
        /// <param name="Partner">идентификатор Партнера программы лояльност</param>
        /// <param name="Pos">код Торговой точки Партнера</param>
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
        /// <param name="Operator">идентификатор Оператора программы лояльности</param>
        /// <param name="Partner">идентификатор Партнера программы лояльност</param>
        /// <param name="Pos">код Торговой точки Партнера</param>
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
        /// <param name="Operator">идентификатор Оператора программы лояльности</param>
        /// <param name="Partner">идентификатор Партнера программы лояльност</param>
        /// <param name="Pos">код Торговой точки Партнера</param>
        /// <param name="From">начало периода</param>
        /// <param name="To">конец периода</param>
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
        /// <param name="Operator">идентификатор Оператора программы лояльности</param>
        /// <param name="Partner">идентификатор Партнера программы лояльност</param>
        /// <param name="Pos">код Торговой точки Партнера</param>
        /// <param name="From">начало периода</param>
        /// <param name="To">конец периода</param>
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
        /// <param name="Operator">идентификатор Оператора программы лояльности</param>
        /// <param name="Partner">идентификатор Партнера программы лояльност</param>
        /// <param name="Pos">код Торговой точки Партнера</param>
        /// <param name="From">начало периода</param>
        /// <param name="To">конец периода</param>
        /// <returns></returns>
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
        /// <param name="Phone">номер телефона</param>
        /// <returns></returns>
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

        /// <summary>
        /// Информация об операторе
        /// </summary>
        /// <param name="Operator">идентификатор Оператора программы лояльности</param>
        /// <returns></returns>
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
        /// <param name="Phone">номер телефона</param>
        /// <param name="Card">номер карты</param>
        /// <param name="Code">код подтверждения</param>
        /// <param name="Operator">идентификатор Оператора программы лояльности</param>
        /// <returns></returns>
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

        /// <summary>
        /// Выборка товаров Оператора
        /// </summary>
        /// <param name="Operator">идентификатор Оператора программы лояльности</param>
        /// <returns></returns>
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
        /// Выборка торговых точек Оператора
        /// </summary>
        /// <param name="Operator">идентификатор Оператора программы лояльности</param>
        /// <returns></returns>
        [HttpPost]
        [Route("OperatorPos")]
        public OperatorPosResponse OperatorPos(OperatorPosRequest request)
        {
            var result = new ServerOperatorPos();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Проверка промокода
        /// </summary>
        /// <param name="Operator">идентификатор Оператора программы лояльности</param>
        /// <param name="Promocode">промо-код</param>
        /// <returns></returns>
        [HttpPost]
        [Route("VerificationPromocode")]
        public VerificationPromocodeResponse VerificationPromocode(VerificationPromocodeRequest request)
        {
            var result = new ServerVerificationPromocode();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }
    }
}
