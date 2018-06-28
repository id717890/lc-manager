using LCManagerPartner.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LCManager.Infrastructure.Request;
using LCManagerPartner.Implementation.Request;
using LCManagerPartner.Implementation.Response;
using LCManagerPartner.Implementation.Services;
using ChangeClientRequest = LCManagerPartner.Models.ChangeClientRequest;

namespace LCManagerPartner.Controllers
{
    [Authorize]
    [RoutePrefix("api/client")]
    public class ClientController : ApiController
    {
        static string connectionString = ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString;
        SqlConnection cnn = new SqlConnection(connectionString);

        private readonly BonusService _bonusService;

        public ClientController()
        {
            _bonusService=new BonusService();
        }

        //Дублирующиеся методы: GetConfirmCode, SetClientPassword, GetRegistrationUser, GetSendVerificationCode, ClientLogin, GetCheques, GetClient, ChangeClient, BalanceGet
        //GetPartners, GetCampaigns, LeaveMessage, SendEmailCode, ValidateEmail, DeletePhone, AddPhone, ClientInfo, ClientCreate, ClientUpdate, CardBonuses, ActivateCard

        /// <summary>
        /// Проверка кода и валидация номера телефона
        /// </summary>
        [HttpPost, AllowAnonymous]
        [Route("GetConfirmCode")]
        public GetConfirmCodeResponse GetConfirmCode(GetConfirmCodeRequest request)
        {
            var result = new ServerGetConfirmCodeResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Установка пароля клиента
        /// </summary>
        [HttpPost, AllowAnonymous]
        [Route("SetClientPassword")]
        public SetClientPasswordResponse SetClientPassword(SetClientPasswordRequest request)
        {
            var result = new ServerSetClientPasswordResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }
        /// <summary>
        /// Регистрация Участника в ЛКУ программы лояльности
        /// </summary>
        [AllowAnonymous, HttpPost]
        [Route("GetRegistrationUser")]
        public GetRegistrationUserResponse GetRegistrationUser(GetRegistrationUserRequest request)
        {
            var result = new ServerGetRegistrationUserResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Отправка проверечного кода в SMS сообщении на указанный номер телефона
        /// </summary>
        [HttpPost, AllowAnonymous]
        [Route("GetSendVerificationCode")]
        public GetSendVerificationCodeResponse GetSendVerificationCode(GetSendVerificationCodeRequest request)
        {
            var result = new ServerGetSendVerificationCodeResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }
        /// <summary>
        /// Авторизация Участника в ЛКУ по логину/паролю или через привязанный аккаунт социальной сети.
        /// </summary>
        [HttpPost]
        [Route("ClientLogin")]
        public ClientLoginResponse ClientLogin(ClientLoginRequest request)
        {
            var result = new ServerClientLoginResponse();
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
            var result = new ServerGetChequesResponse();
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
            var result = new ServerGetClientResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }
        /// <summary>
        /// Изменение данных Участника по заданному идентификатору
        /// </summary>
        [HttpPost, AllowAnonymous]
        [Route("ChangeClient")]
        public ChangeClientResponse ChangeClient(ChangeClientRequest request)
        {
            var result = new ServerChangeClientResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }
        /// <summary>
        /// Получение списка карт Участника программы лояльности
        /// </summary>
        [HttpPost]
        [Route("GetClientCards")]
        public GetClientCardsResponse GetClientCards(GetClientCardsRequest request)
        {
            var result = new ServerGetClientCardsResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }
        /// <summary>
        /// Получение баланса бонусных баллов по номеру карты, либо по номеру телефона Участника программы лояльности.
        /// </summary>
        [HttpPost]
        [Route("BalanceGet")]
        public BalanceGetResponse BalanceGet(BalanceGetRequest request)
        {
            var result = new ServerBalanceGetResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Возвращает информацию о Партнере
        /// </summary>
        [HttpPost]
        [Route("GetPartners")]
        public GetPartnersResponse GetPartners(GetPartnersRequest request)
        {
            var result = new ServerGetPartnersResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Возвращает информацию об акции
        /// </summary>
        [HttpPost]
        [Route("GetCampaigns")]
        public GetCampaignsResponse GetCampaigns(GetCampaignsRequest request)
        {
            var result = new ServerGetCampaignsResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Добавление\удаление клиента у партнера
        /// </summary>
        [HttpPost]
        [Route("ClientPartnerSelect")]
        public ClientPartnerSelectResponse ClientPartnerSelect(ClientPartnerSelectRequest request)
        {
            var result = new ServerClientPartnerSelectResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Добавление\удаление клиента из акции
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ClientCampaignSelect")]
        public ClientCampaignSelectResponse ClientCampaignSelect(ClientCampaignSelectRequest request)
        {
            var result = new ServerClientCampaignSelectResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }
        /// <summary>
        /// Добавление карты для Участника программы лояльности
        /// </summary>
        [HttpPost]
        [Route("ClientAddCard")]
        public ClientAddCardResponse ClientAddCard(ClientAddCardRequest request)
        {
            var result = new ServerClientAddCardResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Отправка сообщения по электронной почте
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("LeaveMessage")]
        public LeaveMessageResponse LeaveMessage(LeaveMessageRequest request)
        {
            var result = new ServerLeaveMessageResponse();
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
            var result = new ServerValidateEmailResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }
        /// <summary>
        /// Добавление адреса электронной почты для Участника программы лояльности
        /// </summary>
        [HttpPost]
        [Route("AddEmail")]
        public AddEmailResponse AddEmail(AddEmailRequest request)
        {
            var result = new ServerAddEmailResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }
        /// <summary>
        /// Добавление аккаунта Facebook для Участника программы лояльности
        /// </summary>
        [HttpPost]
        [Route("AddIDFB")]
        public AddIDFBResponse AddIDFB(AddIDFBRequest request)
        {
            var result = new ServerAddIDFBResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }
        /// <summary>
        /// Добавление аккаунта Odnoklassniki для Участника программы лояльности
        /// </summary>
        [HttpPost]
        [Route("AddIDOK")]
        public AddIDOKResponse AddIDOK(AddIDOKRequest request)
        {
            var result = new ServerAddIDOKResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }
        /// <summary>
        /// Добавление аккаунта VK для Участника программы лояльности
        /// </summary>
        [HttpPost]
        [Route("AddIDVK")]
        public AddIDVKResponse AddIDVK(AddIDVKRequest request)
        {
            var result = new ServerAddIDVKResponse();
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
            var result = new ServerDeletePhoneResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Возвращает персональное предложение Участника (потерялся public class Campaing).
        /// </summary>
        [HttpPost]
        [Route("GetPersonalCampaigns")]
        public GetPersonalCampaignsResponse GetPersonalCampaigns(GetPersonalCampaignsRequest request)
        {
            var result = new ServerGetPersonalCampaignsResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Возвращает "любимые" категории товаров
        /// </summary>
        [HttpPost]
        [Route("SelectPreferences")]
        public SelectPreferencesResponse SelectPreferences(SelectPreferencesRequest request)
        {
            var result = new ServerSelectPreferencesResponse();
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
            var result = new ServerAddPhoneResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Возвращает "любимые" категории товаров Участника
        /// </summary>
        [HttpPost]
        [Route("ClientGetPreferences")]
        public ClientPreferencesResponse ClientGetPreferences(ClientPreferencesRequest request)
        {
            var result = new ServerClientPreferencesResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }
        /// <summary>
        /// Запрос детализации чека по заданному ID чека
        /// </summary>
        [HttpPost]
        [Route("GetChequeDetail")]
        public ChequeDetailResponse GetChequeDetail(ChequeDetailRequest request)
        {
            var result = new ServerChequeDetailResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Записать данные об устройстве Участника для отправки PUSH-уведомлений
        /// </summary>
        [HttpPost]
        [Route("SetClientDevice")]
        public AddDeviceResponse SetClientDevice(AddDeviceRequest request)
        {
            var result = new ServerAddDeviceResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Отправка PUSH-уведомления
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SendPush")]
        public SendPushResponse SendPush(SendPushRequest request)
        {
            var result = new ServerSendPushResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Отправка PUSH-уведомления на устройства Apple
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SendPushApple")]
        public SendPushResponse SendPushApple(SendPushRequest request)
        {
            var result = new ServerSendPushAppleResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Получение статистики по карте
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CardStatistics")]
        public CardStatisticsResponse CardStatistics(CardStatisticsRequest request)
        {
            var result = new ServerCardStatisticsResponse();
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
            var result = new ServerGetClientInfoResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Статистика по картам за периода
        /// </summary>
        [HttpPost]
        [Route("CardAggregation")]
        public CardAggregationResponse CardAggregation(CardAggregationRequest request)
        {
            var result = new ServerGetCardAggregation();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }
        /// <summary>
        /// Создание профиля Участника программы лояльности
        /// </summary>
        [HttpPost, AllowAnonymous]
        [Route("ClientCreate")]
        public ClientCreateResponse ClientCreate(ClientCreateRequest request)
        {
            var result = new ServerClientCreate();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Обновление профиля Участника программы лояльности
        /// </summary>
        [HttpPost]
        [Route("ClientUpdate")]
        public SetClientUpdateResponse ClientUpdate(SetClientUpdateRequest request)
        {
            var result = new ServerSetClientUpdate();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Обновление пароля Участника программы лояльности
        /// </summary>
        [HttpPost]
        [Route("ClientPasswordChange")]
        public ClientPasswordChangeResponse ClientPasswordChange(ClientPasswordChangeRequest request)
        {
            var result = new ServerClientPasswordChange();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Обновление способа связи с клиентов
        /// </summary>
        [HttpPost]
        [Route("ClientUpdateCommunication")]
        public ClientUpdateCommunicationResponse ClientUpdateCommunication(ClientUpdateCommunicationRequest request)
        {
            var result = new ServerClientUpdateCommunication();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }
        /// <summary>
        /// Получение бонусов Участника программы лояльности не относящиеся к чекам.
        /// </summary>
        [HttpPost]
        [Route("ClientBonuses")]
        public ClientBonusesResponse ClientBonuses(ClientBonusesRequest request)
        {
            var result = new ServerClientBonuses();
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
            var result = new ServerCardBonuses();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }
        /// <summary>
        /// Активация карты на списание
        /// </summary>
        [HttpPost]
        [Route("ActivateCard")]
        public ActivateCardResponse ActivateCard(ActivateCardRequest request)
        {
            var result = new ServerActivateCard();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Получение списка бонусов не за покупки
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("BonusesNotForPurchases")]
        public BonusesNotForPurchasesResponse BonusesNotForPurchases(BonusesNotForPurchasesRequest request)
        {
            return _bonusService.GetBonusesNotForPurchases(request);
        }
    }
}
