using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Optimization;
using System.Web.SessionState;

namespace PlizCard
{
    public class ApiConfig
    {
        private static lcsite.SiteSoapClient/* SiteSoapClient*/ lc_site;
        private static lcclient.ServiceClientSoapClient lc_client;

        public static void RegisetrApiConfig()
        {
            reconnect();
        }

        ~ApiConfig()
        {
            close();
        }
        /*
         Повторное подключение к бэку
             */
        public static void reconnect()
        {
            if (lc_site == null) lc_site = new lcsite.SiteSoapClient();// .SiteSoapClient();
            if (lc_client == null) lc_client = new lcclient.ServiceClientSoapClient();

            switch (lc_site.State)
            {
                case System.ServiceModel.CommunicationState.Closed:
                case System.ServiceModel.CommunicationState.Closing:
                case System.ServiceModel.CommunicationState.Faulted:
                    lc_site = new lcsite.SiteSoapClient(); //SiteSoapClient();
                    break;
            }
            switch (lc_client.State)
            {
                case System.ServiceModel.CommunicationState.Closed:
                case System.ServiceModel.CommunicationState.Closing:
                case System.ServiceModel.CommunicationState.Faulted:
                    lc_client = new lcclient.ServiceClientSoapClient();
                    break;
            }
        }

        /*
         Закрыть соединение с бэком
             */
        public static void close()
        {
            if (lc_site.State != System.ServiceModel.CommunicationState.Closed) lc_site.Close();
            if (lc_client.State != System.ServiceModel.CommunicationState.Closed) lc_client.Close();
        }

        /*
         не используется
             */
        public static List<SessionStateItemCollection> getOnlineUsers()
        {
            List<SessionStateItemCollection> activeSessions = new List<SessionStateItemCollection>();
            object obj = typeof(HttpRuntime).GetProperty("CacheInternal", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null, null);
            object[] obj2 = (object[])obj.GetType().GetField("_caches", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(obj);
            for (int i = 0; i < obj2.Length; i++)
            {
                Hashtable c2 = (Hashtable)obj2[i].GetType().GetField("_entries", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(obj2[i]);
                foreach (DictionaryEntry entry in c2)
                {
                    object o1 = entry.Value.GetType().GetProperty("Value", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(entry.Value, null);
                    if (o1.GetType().ToString() == "System.Web.SessionState.InProcSessionState")
                    {
                        SessionStateItemCollection sess = (SessionStateItemCollection)o1.GetType().GetField("_sessionItems", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(o1);
                        if (sess != null)
                        {
                            if (sess!= null)
                            {
                                activeSessions.Add(sess);
                            }
                        }
                    }
                }
            }
            return activeSessions;
        }

        /*
         Получить список городов
             */
        public static lcsite.GetCitiesResponse GetCities()
        {
            reconnect();
            var cities_request = new lcsite.GetCitiesRequest();
            var cities_response = lc_site.GetCities(cities_request);
            return cities_response;
        }

        /*
         Получить список партнеров
             */
        public static lcsite.GetPartnersResponse GetPartners(bool isonminepage=false, bool isCardIssue=false, bool isInCity=false, bool isInInternet=false ,int SegmentID=0, int CategoryID=0)
        {
            reconnect();
            var partners_request = new lcsite.GetPartnersRequest();
//            partners_request.CityID = UserSession.current_city.id;
            partners_request.IsCardIssue = isCardIssue;
            partners_request.IsInCity = isInCity;
            partners_request.IsInInternet = isInInternet;
            partners_request.CategoryID = CategoryID;
            partners_request.SegmentID = SegmentID;
            partners_request.IsMainPage = isonminepage;
            var partners_response = lc_site.GetPartners(partners_request);
            return partners_response;
        }

        /*
         Получить основную информацию о партнере
             */
        public static lcsite.GetPartnerResponse GetPartner(int PartnerID)
        {
            reconnect();
            var partner_request = new lcsite.GetPartnerRequest();
            partner_request.PartnerID = PartnerID;
            var partner_response = lc_site.GetPartner(partner_request);
            return partner_response;
        }

        /*
         Получить дополнительную информацию о партнере
             */
        public static lcsite.GetPartnerInfoResponse GetPartnerInfo(int PartnerID)
        {
            reconnect();
            var partnerinfo_request = new lcsite.GetPartnerInfoRequest();
            partnerinfo_request.PartnerID = PartnerID;
            var partnerinfo_response = lc_site.GetPartnerInfo(partnerinfo_request);
            return partnerinfo_response;
        }



        /*
         Получить список сегментов
             */
        public static lcsite.GetSegmentsResponse GetSegments()
        {
            reconnect();
            var segment_request = new lcsite.GetSegmentsRequest();
            var segment_response = lc_site.GetSegments(segment_request);
            return segment_response;
        }

        /*
         Получить список точек партнера
             */
        public static lcsite.GetPosesResponse GetPoses(int PartnerID)
        {
            reconnect();
            var poses_request = new lcsite.GetPosesRequest();
            poses_request.PartnerID = PartnerID;
            var poses_response = lc_site.GetPoses(poses_request);
            return poses_response;
        }

        /*
         Получить список акций
             */
        public static lcsite.GetCampaignsResponse GetCampaigns(bool isonminepage=false, int PartnerID=0,bool IsSideBar=false,int SegmentID=0, int CategoryID=0, bool isnew=false, bool ispopular=false)
        {
            reconnect();
            var campaigns_request = new lcsite.GetCampaignsRequest();
//            campaigns_request.CityID = UserSession.current_city.id;
            campaigns_request.IsMainPage = isonminepage;
            campaigns_request.PartnerID = PartnerID;
            campaigns_request.IsSideBar = IsSideBar;
            campaigns_request.SegmentID = SegmentID;
            campaigns_request.CategoryID = CategoryID;
            campaigns_request.IsNew = isnew;
            campaigns_request.IsPopular = ispopular;
            var campaigns_response = lc_site.GetCampaigns(campaigns_request);
            return campaigns_response;
        }

        /*
         Получить информацию о клиенте
             */
        public static lcclient.GetClientResponse GetClient(int ClientID)
        {
            reconnect();
            var client_request = new lcclient.GetClientRequest();
            client_request.ClientID = ClientID;
            var client_response = lc_client.GetClient(client_request);
            return client_response;
        }

        /*
         Логин клиента
             */
        public static lcclient.ClientLoginResponse ClientLogin(long login, string passw, string idFB="", string idOK="", string idVK="")
        {
            reconnect();
            var clientlogin_request = new lcclient.ClientLoginRequest();
            clientlogin_request.Login = login;
            clientlogin_request.Password = passw;
            if(idFB!="")
            clientlogin_request.IdFB = idFB;
            if(idOK!="")
            clientlogin_request.IdOK = idOK;
            if(idVK!="")
            clientlogin_request.IdVK = idVK;
            var clientlogin_response = lc_client.ClientLogin(clientlogin_request);
            return clientlogin_response;
        }

        /*
         Получить список категорий
             */
        public static lcsite.GetCategoriesResponse GetCategories()
        {
            reconnect();
            var categories_request = new lcsite.GetCategoriesRequest();
            var categories_response = lc_site.GetCategories(categories_request);
            return categories_response;
        }

        /*
         Получить код для номера телефона
             */
        public static lcclient.GetSendVerificationCodeResponse GetVerificationCode(long phone)
        {
            reconnect();
            var verifiactioncode_request = new lcclient.GetSendVerificationCodeRequest();
            verifiactioncode_request.Phone = phone;
            var verificationcode_response = lc_client.GetSendVerificationCode(verifiactioncode_request);
            return verificationcode_response;
        }

        /*
         Получить список чеков
             */
        public static lcclient.GetChequesResponse GetCheques(int ClientId)
        {
            reconnect();
            var cheques_request = new lcclient.GetChequesRequest();
            cheques_request.ClientId = ClientId;
            var cheques_response = lc_client.GetCheques(cheques_request);
            return cheques_response;
        }

        /*
         Получить список карт клиента
             */
        public static lcclient.GetClientCardsResponse GetClientCards(int ClientId)
        {
            reconnect();
            var cards_request = new lcclient.GetClientCardsRequest();
            cards_request.ClientID = ClientId;
            var cards_response = lc_client.GetClientCards(cards_request);
            return cards_response;
        }

        /*
         Проверить код по номеру телефона
             */
        public static lcclient.GetConfirmCodeResponse GetConfirmCode(long Phone, string Code)
        {
            reconnect();
            var confirmcode_request = new lcclient.GetConfirmCodeRequest();
            confirmcode_request.Code = Code;
            confirmcode_request.Phone = Phone;
            var confirmcode_response = lc_client.GetConfirmCode(confirmcode_request);
            return confirmcode_response;
        }

        /*
         Зарегистровать клиента по номеру телефона + номер карты
             */
        public static lcclient.GetRegistrationUserResponse GetRegistrationUser(long Phone, long Card)
        {
            reconnect();
            var getregistrationuser_request = new lcclient.GetRegistrationUserRequest();
            getregistrationuser_request.Phone = Phone;
            getregistrationuser_request.Card = Card;
            var getregistrationuser_response = lc_client.GetRegistrationUser(getregistrationuser_request);
            return getregistrationuser_response;
        }

        /*
         Изменить профиль клиента
             */
        public static lcclient.ChangeClientResponse ChangeClient(lcclient.Client client)
        {
            reconnect();
            var changeclient_request = new lcclient.ChangeClientRequest();
            changeclient_request.ClientData = client;
            var changeclient_response = lc_client.ChangeClient(changeclient_request);
            return changeclient_response;
        }

        /*
         Установить пароль
             */
        public static lcclient.SetClientPasswordResponse SetClientPassword(long Phone, string Code, string Password)
        {
            reconnect();
            var setpassword_request = new lcclient.SetClientPasswordRequest();
            setpassword_request.Code = Code;
            setpassword_request.Phone = Phone;
            setpassword_request.Password = Password;
            var setpassword_response = lc_client.SetClientPassword(setpassword_request);
            return setpassword_response;
        }

        /*
         Special For Mobile !
         Получить список партнеров (для мобильных клиентов)
         */
        public static lcsite.GetPartnersResponse GetPartnersMobile(bool isonminepage = false, bool isCardIssue = false, bool isInCity = false, bool isInInternet = false, int city_id=0, int SegmentID = 0, int CategoryID = 0)
        {
            reconnect();
            var partners_request = new lcsite.GetPartnersRequest();
            partners_request.CityID = city_id;
            partners_request.IsCardIssue = isCardIssue;
            partners_request.IsInCity = isInCity;
            partners_request.IsInInternet = isInInternet;
            partners_request.CategoryID = CategoryID;
            partners_request.SegmentID = SegmentID;
            partners_request.IsMainPage = isonminepage;
            var partners_response = lc_site.GetPartners(partners_request);
            return partners_response;
        }

        /*
         Получить список акций (для мобильных клиентов)
             */
        public static lcsite.GetCampaignsResponse GetCampaignsMobile(bool isonminepage = false, int city_id=0, int PartnerID = 0, bool IsSideBar = false, int SegmentID = 0, int CategoryID = 0, bool isnew = false, bool ispopular = false)
        {
            reconnect();
            var campaigns_request = new lcsite.GetCampaignsRequest();
            campaigns_request.CityID = city_id;
            campaigns_request.IsMainPage = isonminepage;
            campaigns_request.PartnerID = PartnerID;
            campaigns_request.IsSideBar = IsSideBar;
            campaigns_request.SegmentID = SegmentID;
            campaigns_request.CategoryID = CategoryID;
            campaigns_request.IsNew = isnew;
            campaigns_request.IsPopular = ispopular;
            var campaigns_response = lc_site.GetCampaigns(campaigns_request);
            return campaigns_response;
        }

        /*
         Получить информацию по акции
             */
        public static lcsite.GetCampaignResponse GetCampaign(int CampaignID)
        {
            reconnect();
            var campaign_request = new lcsite.GetCampaignRequest();
            campaign_request.CampaignID = CampaignID;
            var campaign_response = lc_site.GetCampaign(campaign_request);
            return campaign_response;
        }

        /*
         Получить дополнительную информацию по акции
             */
        public static lcsite.GetCampaignInfoResponse GetCampaignInfo(int CampaignID)
        {
            reconnect();
            var campaigninfo_request = new lcsite.GetCampaignInfoRequest();
            campaigninfo_request.CampaignID = CampaignID;
            var campaigninfo_response = lc_site.GetCampaignInfo(campaigninfo_request);
            close();
            return campaigninfo_response;
        }

        /*
         Получить акции для пользователя
             */
        public static lcclient.GetCampaignsResponse ClientGetCampaigns(int ClientID=-1)
        {
            reconnect();
            var request = new lcclient.GetCampaignsRequest();
            if (ClientID > 0)
                request.ClientID = ClientID;
            else
                request.ClientID = UserSession.user_id;
            var response = lc_client.GetCampaigns(request);
            return response;
        }

        /*
         Получить партнеров для пользователя
             */
        public static lcclient.GetPartnersResponse ClientGetPartners(int ClientID=-1)
        {
            reconnect();
            var request = new lcclient.GetPartnersRequest();
            if (ClientID > 0)
                request.ClientID = ClientID;
            else
                request.ClientID = UserSession.user_id;
            var response = lc_client.GetPartners(request);
            return response;
        }

        /*
         Получить/установить акцию в избранное
             */
        public static lcclient.ClientCampaignSelectResponse ClientGetSetCampaignsFav(int CampaignID,bool Remove=false, int ClientID =-1)
        {
            reconnect();
            var request = new lcclient.ClientCampaignSelectRequest();
            if (ClientID > 0)
                request.ClientID = ClientID;
            else
                request.ClientID = UserSession.user_id;
            request.CampaignID = CampaignID;
            request.Remove = Remove;
            var response = lc_client.ClientCampaignSelect(request);
            return response;
        }

        /*
         Получить/установить партнера в избранное
             */
        public static lcclient.ClientPartnerSelectResponse ClientGetSetPartnersFav(int PartnerID, bool Remove = false, int ClientID = -1)
        {
            reconnect();
            var request = new lcclient.ClientPartnerSelectRequest();
            if (ClientID > 0)
                request.ClientID = ClientID;
            else
                request.ClientID = UserSession.user_id;
            request.PartnerID = PartnerID;
            request.Remove = Remove;
            var response = lc_client.ClientPartnerSelect(request);
            return response;
        }

        /*
         Добавить емэйл
             */
        public static lcclient.AddEmailResponse AddEmail(string Email)
        {
            reconnect();
            var request = new lcclient.AddEmailRequest();
            request.ClientID = UserSession.user_id;
            request.Email = Email;
            var response = lc_client.AddEmail(request);
            return response;
        }

        /*
         Добавить ИД фэйсбука
             */
        public static lcclient.AddIDFBResponse AddIDFB(string IDFB)
        {
            reconnect();
            var request = new lcclient.AddIDFBRequest();
            request.ClientID = UserSession.user_id;
            request.CodeFB = IDFB;
            var response = lc_client.AddIDFB(request);
            return response;
        }

        /*
         Добавить ИД одноклассников
             */
        public static lcclient.AddIDOKResponse AddIDOK(string IDOK)
        {
            reconnect();
            var request = new lcclient.AddIDOKRequest();
            request.ClientID = UserSession.user_id;
            request.CodeOK = IDOK;
            var response = lc_client.AddIDOK(request);
            return response;
        }

        /*
         Добавить ИД вконтакте
             */
        public static lcclient.AddIDVKResponse AddIDVK(string IDVK)
        {
            reconnect();
            var request = new lcclient.AddIDVKRequest();
            request.ClientID = UserSession.user_id;
            request.CodeVK = IDVK;
            var response = lc_client.AddIDVK(request);
            return response;
        }

        /*
         Добавить телефон
             */
        public static lcclient.AddPhoneResponse AddPhone(long Phone)
        {
            reconnect();
            var request = new lcclient.AddPhoneRequest();
            request.ClientID = UserSession.user_id;
            request.Phone = Phone;
            var response = lc_client.AddPhone(request);
            return response;
        }

        /*
         Получить баланс
             */
        public static lcclient.BalanceGetResponse GetBalance(long Phone)
        {
            reconnect();
            var request = new lcclient.BalanceGetRequest();
            request.Phone = Phone;
            var response = lc_client.BalanceGet(request);
            return response;
        }

        /*
         Добавить номер карты
             */
        public static lcclient.ClientAddCardResponse ClientAddCard(long Card)
        {
            reconnect();
            var request = new lcclient.ClientAddCardRequest();
            request.ClientID = UserSession.user_id;
            request.Card = Card;
            var response = lc_client.ClientAddCard(request);
            return response;
        }

        /*
         Удалить номер телефона
             */
        public static lcclient.DeletePhoneResponse DeletePhone(long Phone)
        {
            reconnect();
            var request = new lcclient.DeletePhoneRequest();
            request.Phone = Phone;
            var response = lc_client.DeletePhone(request);
            return response;
        }

        /*
         Оставить сообщение (для зарегистрированных)
             */
        public static lcclient.LeaveMessageResponse LeaveMessage(string Subject, string Text)
        {
            reconnect();
            var request = new lcclient.LeaveMessageRequest();
            request.ClientID = UserSession.user_id;
            request.Text = Text;
            request.Subject = Subject;
            var response = lc_client.LeaveMessage(request);
            return response;
        }

        /*
         Получить/установить избранную категорию
             */
        public static lcclient.SelectPreferencesResponse SelectPreferences(int CategoryID, bool Remove)
        {
            reconnect();
            var request = new lcclient.SelectPreferencesRequest();
            request.ClientID = UserSession.user_id;
            request.CategoryID = CategoryID;
            request.Remove = Remove;
            var response = lc_client.SelectPreferences(request);
            return response;
        }

        /*
         Отправить код подтверждения на почту
             */
        public static lcclient.SendEmailCodeResponse SendEmailCode(string Email)
        {
            reconnect();
            var request = new lcclient.SendEmailCodeRequest();
            request.Email = Email;
            var response = lc_client.SendEmailCode(request);
            return response;
        }

        /*
         Проверить емэйл
             */
        public static lcclient.ValidateEmailResponse ValidateEmail(string Email, string Code, int Client = 0)
        {
            reconnect();
            var request = new lcclient.ValidateEmailRequest();
            request.Email = Email;
            request.Code = Code;
            request.Client = Client;
            var response = lc_client.ValidateEmail(request);
            return response;
        }

        /*
         Получить персональные акции
             */
        public static lcclient.GetPersonalCampaignsResponse GetPersonalCampaigns()
        {
            reconnect();
            var request = new lcclient.GetPersonalCampaignsRequest();
            request.ClientID = UserSession.user_id;
            var response = lc_client.GetPersonalCampaigns(request);
            return response;
        }

        /*
         Изменить пароль
             */
        public static lcclient.SetClientPasswordResponse ChangeClientPassword(string Password)
        {
            reconnect();
            var request = new lcclient.SetClientPasswordRequest();
            //request.Phone = Phone;
            request.Password = Password;
            request.ClientID = UserSession.user_id;
            return lc_client.SetClientPassword(request);
        }

        /*
         Получить ФАК
             */
        public static lcsite.GetFaqResponse GetFaq()
        {
            reconnect();
            var request = new lcsite.GetFaqRequest();
            return lc_site.GetFaq(request);
        }

        /*
         Получить предпочтения
             */
        public static lcclient.ClientPreferencesResponse GetClientPreferences()
        {
            reconnect();
            var request = new lcclient.ClientPreferencesRequest();
            request.ClientID = UserSession.user_id;
            var response = lc_client.ClientGetPreferences(request);
            return response;
        }

        /*
         Оставить сообщение (для незарегистрированных)
             */
        public static lcsite.LeaveMessageResponse LeaveMessage(string Email, string Subject,string Text)
        {
            reconnect();
            var request = new lcsite.LeaveMessageRequest();
            request.Email = Email;
            request.Subject = Subject;
            request.Text = Text;
            var response = lc_site.LeaveMessage(request);
            return response;
        }

        /*
         получить детали акции
             */
        public static lcsite.CampaignDetailResponse GetCampaignDetail(int CampaignID)
        {
            reconnect();
            var request = new lcsite.CampaignDetailRequest();
            request.CampaignID = CampaignID;
            var response = lc_site.GetCampaignDetail(request);
            return response;
        }

        /*
         Получить детали чека
             */
        public static lcclient.ChequeDetailResponse GetChequeDetails(int ChequeID)
        {
            reconnect();
            var request = new lcclient.ChequeDetailRequest();
            request.ChequeID = ChequeID;
            var response = lc_client.GetChequeDetail(request);
            return response;
        }

        /*
         Установить токен
             */
        public static lcclient.AddDeviceResponse SetClientDevice(string Token, string OSRegistrator)
        {
            var request = new lcclient.AddDeviceRequest();
            request.Client = UserSession.user_id;
            request.Device_token = Token;
            request.OSRegistrator = OSRegistrator;
            var response = lc_client.SetClientDevice(request);
            return response;
        }

        /// <summary>
        /// Отправить заявку стать партнёром
        /// </summary>
        /// <param name="City">Город</param>
        /// <param name="Site">Адрес сайта</param>
        /// <param name="GoodsSell">Товары, которые продают</param>
        /// <param name="PosQty">Количество ТТ</param>
        /// <param name="CashSoftware">Кассовое программное обеспечение</param>
        /// <param name="Name">Имя</param>
        /// <param name="Phone">Номер телефона</param>
        /// <param name="Email">Адрес электронной почты</param>
        /// <returns></returns>
        public static lcsite.BecomePartnerResponse BecomePartner(string City, string Site, string GoodsSell, int? PosQty, string CashSoftware, string Name, long Phone, string Email)
        {
            var request = new lcsite.BecomePartnerRequest();
            request.City = City;
            request.Site = Site;
            request.GoodsSell = GoodsSell;
            request.PosQty = PosQty;
            request.CashSoftware = CashSoftware;
            request.Name = Name;
            request.Phone = Phone;
            request.Email = Email;
            var response = lc_site.BecomePartner(request);
            return response;
        }
    }
}
