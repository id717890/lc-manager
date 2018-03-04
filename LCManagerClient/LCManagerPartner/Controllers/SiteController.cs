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
    [RoutePrefix("api/site")]
    public class SiteController : ApiController
    {
        static string connectionString = ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString;
        SqlConnection cnn = new SqlConnection(connectionString);

        //Дублирующиеся методы: GetCampaigns, GetPoses, GetClient

        /// <summary>
        /// Получение списка сегментов
        /// </summary>
        [HttpPost]
        [Route("GetSegments")]
        public GetSegmentsResponse GetSegments(GetSegmentsRequest request)
        {
            var result = new ServerGetSegmentsResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Получения списка категорий?
        /// </summary>
        [HttpPost]
        [Route("GetCategories")]
        public GetCategoriesResponse GetCategories(GetCategoriesRequest request)
        {
            var result = new ServerGetCategoriesResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Получение списка городов
        /// </summary>
        [HttpPost]
        [Route("GetCities")]
        public GetCitiesResponse GetCities(GetCitiesRequest request)
        {
            var result = new ServerGetCitiesResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Получение списка партнеров по заданным параметрам
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
        /// Получение подробной информации о партнере
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetPartner")]
        public GetPartnerResponse GetPartner(GetPartnerRequest request)
        {
            var result = new ServerGetPartnerResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Получение списка акций
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetCampaigns")]
        public GetCampaignsResponse GetCampaigns(GetCampaignsRequest request)
        {
            var result = new ServerGetCampaignsResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Получение подробной информации об акции
        /// </summary>

        [HttpPost]
        [Route("GetCampaign")]
        public GetCampaignResponse GetCampaign(GetCampaignRequest request)
        {
            var result = new ServerGetCampaignResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Получение списка торговых терминалов
        /// </summary>
        [HttpPost]
        [Route("GetPoses")]
        public GetPosesResponse GetPoses(GetPosesRequest request)
        {
            var result = new ServerGetPosesResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Получение информации о партнере
        /// </summary>
        [HttpPost]
        [Route("GetPartnerInfo")]
        public GetPartnerInfoResponse GetPartnerInfo(GetPartnerInfoRequest request)
        {
            var result = new ServerGetPartnerInfoResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Получение информации об акции
        /// </summary>
        [HttpPost]
        [Route("GetCampaignInfo")]
        public GetCampaignInfoResponse GetCampaignInfo(GetCampaignInfoRequest request)
        {
            var result = new ServerGetCampaignInfoResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Получение информации об участнике программы лояльности
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
        /// Отправка сообщения по электронной почте
        /// </summary>
        [HttpPost]
        [Route("LeaveMessage")]
        public LeaveMessageResponse LeaveMessage(LeaveMessageRequest request)
        {
            var result = new ServerLeaveMessageResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Получение FAQ
        /// </summary>
        [HttpPost]
        [Route("GetFaq")]
        public GetFaqResponse GetFaq(GetFaqRequest request)
        {
            var result = new ServerFaqResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Получение детализированной информации об акции
        /// </summary>
        [HttpPost]
        [Route("GetCampaignDetail")]
        public CampaignDetailResponse GetCampaignDetail(CampaignDetailRequest request)
        {
            var result = new ServerCampaignDetailResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        /// <summary>
        /// Регистрация нового партнера программы лояльности
        /// </summary>
        [HttpPost]
        [Route("BecomePartner")]
        public BecomePartnerResponse BecomePartner(BecomePartnerRequest request)
        {
            var result = new ServerBecomePartner();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }
    }
}
